using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

[InitializeOnLoad]
public class PublicKeyLoader : IPreprocessBuildWithReport
{
    static PublicKeyLoader()
    {
        LoadPublicKey();
    }

    public int callbackOrder { get { return 0; } }
    
    private static void LoadPublicKey()
    {
        string publicKeyPath = Application.streamingAssetsPath + "/public_key.pem";

        if (!File.Exists(publicKeyPath))
        {
            return;
        }
        
        string publicKey = File.ReadAllText(publicKeyPath); 
        File.Delete(publicKeyPath);
        
        publicKey = publicKey.Replace("\r\n", "\n");

        using (var reader = new StringReader(publicKey))
        {
            var pemReader = new PemReader(reader);
            var keyPair = (RsaKeyParameters)pemReader.ReadObject();

            var publicKeyParams = new RSAParameters
            {
                Modulus = keyPair.Modulus.ToByteArrayUnsigned(),
                Exponent = keyPair.Exponent.ToByteArrayUnsigned()
            };

            var rsa = RSA.Create();
            rsa.ImportParameters(publicKeyParams);

            string xmlPublicKey = rsa.ToXmlString(false);
            string encryptedXml = AesEncryptionUtility.EncryptString(xmlPublicKey);
            string code = $"public static class PublicKeyProvider {{ public static readonly string Key = @\"{encryptedXml}\"; }}";
            string className = "PublicKeyProvider";
            string fieldName = "Key";
        
            Type publicKeyProviderType = typeof(PublicKeyLoader).Assembly.GetType(className);
            if (publicKeyProviderType == null)
            {
                string filePath = Application.dataPath + "/Scripts/PublicKeyProvider.cs";
                File.WriteAllText(filePath, code);
            }
            else
            {
                FieldInfo keyField = publicKeyProviderType.GetField(fieldName);
                keyField.SetValue(null, xmlPublicKey);
            }
        }
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        LoadPublicKey();
    }
}