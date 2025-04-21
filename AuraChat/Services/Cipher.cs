using System.Security.Cryptography;
using System.Text;

namespace AuraChat.Services;

/// <summary>
/// Advanced Encryption Standard (AES) cipher providing encrypting and decrypting, IV is concatinated at the end of the cipher text and extracted with constant byte size (16)
/// </summary>
public static class Cipher
{
    private static readonly byte[] Key = new byte[32];
    private static readonly int IvLength = 16;

    static Cipher()
    {
        string keyString = Environment.GetEnvironmentVariable("CipherPrivateKey")!;
        byte[] keyBytes = Encoding.UTF8.GetBytes(keyString);
        Array.Copy(keyBytes, Key, Math.Min(keyBytes.Length, Key.Length));
    }

    public static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.GenerateIV();

            byte[] encryptedBytes;
            using (var encryptor = aes.CreateEncryptor())
            using (var ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, IvLength);
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                encryptedBytes = ms.ToArray();
            }

            return Convert.ToBase64String(encryptedBytes);
        }
    }

    public static string Decrypt(string cipherText)
    {
        byte[] encryptedBytes = Convert.FromBase64String(cipherText);
        if (encryptedBytes.Length < IvLength)
            throw new ArgumentException("Invalid ciphertext was detected while decrypting");

        byte[] iv = new byte[IvLength];
        byte[] cipherBytes = new byte[encryptedBytes.Length - IvLength];
        Array.Copy(encryptedBytes, 0, iv, 0, IvLength);
        Array.Copy(encryptedBytes, IvLength, cipherBytes, 0, cipherBytes.Length);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = iv;

            using (var decrypter = aes.CreateDecryptor())
            using (var ms = new MemoryStream(cipherBytes))
            using (var cs = new CryptoStream(ms, decrypter, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}