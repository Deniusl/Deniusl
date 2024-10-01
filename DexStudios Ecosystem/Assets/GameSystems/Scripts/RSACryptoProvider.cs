using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

public class RSACryptoProvider
{
    public static byte[] EncryptData(string data, string publicKeyPath)
    {
        string publicKey = AesEncryptionUtility.DecryptString(PublicKeyProvider.Key);

        var rsaKey = RSA.Create();
        rsaKey.FromXmlString(publicKey);
        
        var encryptedData = rsaKey.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);

        var encryptedJson = new JObject();
        encryptedJson["command"] = Convert.ToBase64String(encryptedData);
        string encryptedJsonStr = encryptedJson.ToString();

        return Encoding.UTF8.GetBytes(encryptedJsonStr);
    }
}