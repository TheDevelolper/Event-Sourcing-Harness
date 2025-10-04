using System.Security.Cryptography;

namespace SaasFactory.AppHost;

public class CredentialsGenerator
{
    private const string UsernameChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const string PasswordChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{};:,.<>?";

    public static string GenerateUsername(int length = 12)
    {
        return GenerateSecureRandomString(length, UsernameChars);
    }

    public static string GeneratePassword(int length = 16)
    {
        return GenerateSecureRandomString(length, PasswordChars);
    }

    private static string GenerateSecureRandomString(int length, string allowedChars)
    {
        if (length <= 0) throw new ArgumentException("Length must be positive", nameof(length));
        var result = new char[length];
        var buffer = new byte[length];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(buffer);
            for (var i = 0; i < length; i++)
            {
                result[i] = allowedChars[buffer[i] % allowedChars.Length];
            }
        }
        return new string(result);
    }
}