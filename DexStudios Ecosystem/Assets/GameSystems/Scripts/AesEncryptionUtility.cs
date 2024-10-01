using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AesEncryptionUtility
{
    static byte[] key = Encoding.UTF8.GetBytes("T#Ea&+yK^7y1dN@p!b%z*uOZkFj0DcQ2");

    public static string EncryptString(string plainText)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encrypted;
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV();
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var msEncrypt = new System.IO.MemoryStream())
            {
                msEncrypt.Write(aes.IV, 0, aes.IV.Length);
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                }

                encrypted = msEncrypt.ToArray();
            }
        }

        return Convert.ToBase64String(encrypted);
    }

    public static string DecryptString(string cipherText)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        byte[] iv = new byte[16];
        Array.Copy(cipherBytes, 0, iv, 0, 16);
        byte[] encrypted = new byte[cipherBytes.Length - 16];
        Array.Copy(cipherBytes, 16, encrypted, 0, encrypted.Length);
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (var msDecrypt = new System.IO.MemoryStream(encrypted))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}