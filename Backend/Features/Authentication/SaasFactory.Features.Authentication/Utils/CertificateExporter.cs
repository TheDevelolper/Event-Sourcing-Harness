using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using ILogger = Serilog.ILogger;

namespace SaasFactory.Features.Authentication.Utils;

public static class CertificateExporter
{
    /// <summary>
    /// Exports the .NET development HTTPS certificate to PEM-encoded certificate (.crt) and private key (.pem) files.
    /// </summary>
    /// <remarks>
    /// This method is intended for development use only. If both the certificate and key files already exist,
    /// the method exits without performing any action.
    /// </remarks>
    /// <param name="secretsPath">The directory where the certificate and key files will be written.</param>
    /// <param name="crtFileName">The filename for the exported certificate (typically ends with .crt).</param>
    /// <param name="keyFileName">The filename for the exported private key (typically ends with .pem).</param>
    public static void ExportToCrtAndPem(string secretsPath, string crtFileName, string keyFileName, ILogger logger)
    {
        var certPath = Path.Combine(secretsPath, crtFileName);
        var keyPath = Path.Combine(secretsPath, keyFileName);
        var tempPfxPath = Path.Combine(secretsPath, "temp.pfx");
        var tempPassword = Guid.NewGuid().ToString();

        if (File.Exists(certPath))
            File.Delete(certPath);

        if (File.Exists(keyPath))
            File.Delete(keyPath);

        logger.Information("Exporting dev certificate to PFX...");
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"dev-certs https -ep \"{tempPfxPath}\" -p {tempPassword}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        proc.Start();
        proc.WaitForExit();

        if (proc.ExitCode != 0)
        {
            var error = proc.StandardError.ReadToEnd();
            throw new Exception($"dotnet dev-certs export failed: {error}");
        }

        logger.Information("Importing PFX with exportable keys...");
        var cert = X509CertificateLoader.LoadPkcs12CollectionFromFile(tempPfxPath, tempPassword,
            X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);

        ExportPfxToPemWithBouncyCastle(tempPfxPath, tempPassword, certPath, keyPath);

        File.Delete(tempPfxPath);
    }

    private static void ExportPfxToPemWithBouncyCastle(string pfxPath, string password, string certPath, string keyPath)
    {
        using var fs = File.OpenRead(pfxPath);
        var pkcs12 = new Pkcs12StoreBuilder().Build();
        pkcs12.Load(fs, password.ToCharArray());
        
        var alias = string.Empty;
        foreach (string a in pkcs12.Aliases)
        {
            if (!pkcs12.IsKeyEntry(a)) continue;
            alias = a;
            break;
        }

        var keyEntry = pkcs12.GetKey(alias);
        var certEntry = pkcs12.GetCertificate(alias);

        // Write cert PEM
        using (var certWriter = new StreamWriter(certPath))
        {
            var pemWriter = new PemWriter(certWriter);
            pemWriter.WriteObject(certEntry.Certificate);
        }

        // Write private key PEM
        using (var keyWriter = new StreamWriter(keyPath))
        {
            var pemWriter = new PemWriter(keyWriter);
            pemWriter.WriteObject(keyEntry.Key);
        }
    }
}
