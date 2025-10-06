using System.Security.Cryptography;

namespace SaasFactory.AppHost;

public class Crypto
{
    public string GenerateText(int bitLength)
    {
        var byteLength = bitLength / 8; // 8 bits in a byte. 
        var bytes = new byte[byteLength];
        RandomNumberGenerator.Fill(bytes);
        var result = Convert.ToBase64String(bytes);
        return result;
    }
}