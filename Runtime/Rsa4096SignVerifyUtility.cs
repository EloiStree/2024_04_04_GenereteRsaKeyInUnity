using System.Security.Cryptography;

public class Rsa4096SignVerifyUtility {

    public const int KEY_SIZE = 4096;

    public static void CreateRsaKey4096(out string privateKeyXml, out string publicKeyXml)
    {
        privateKeyXml = null;
        publicKeyXml = null;
        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = KEY_SIZE;
            string publicXml = rsa.ToXmlString(false);
            string privateXml = rsa.ToXmlString(true);
        }
    }

    public static void CreateRsaKey4096(out RSA rsa)
    {
        rsa = RSA.Create();
        rsa.KeySize = KEY_SIZE;

    }
    public static void CreateRsaKey4096(out RSAParameters privateKey)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = KEY_SIZE;
            string privateXml = rsa.ToXmlString(true);
            LoadPrivateKey4096FromXml(privateXml, out privateKey);
            return;
        }
    }
    public static void LoadPrivateKey4096FromXml(in string privateKeyXml, out RSAParameters privateKey)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = KEY_SIZE;
            rsa.FromXmlString(privateKeyXml);
            privateKey = rsa.ExportParameters(true);
        }
    }
    public static void LoadPrivateKey4096FromXml(in string privateKeyXml, out RSA privateKey)
    {
        privateKey = RSA.Create();
        privateKey.KeySize = KEY_SIZE;
        privateKey.FromXmlString(privateKeyXml);
    }

    public static byte[] SignData(in byte[] data, in IKeyPairRsaHolderMono keyPair)
    {
        keyPair.GetPrivateXml(out string xmlPrivate);
        return SignData(data, xmlPrivate);
    }

    public static byte[] SignData(in byte[] data, in string xmlPrivate)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = KEY_SIZE;
            rsa.FromXmlString(xmlPrivate);
            RSAParameters privateKey = rsa.ExportParameters(true);
            rsa.ImportParameters(privateKey);
            return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
    public static bool VerifySignature(in byte[] data, in byte[] signature, IKeyPairRsaHolderMono keyPair)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = KEY_SIZE;
            keyPair.GetPrivateXml(out string xmlPrivate);
            rsa.FromXmlString(xmlPrivate);
            RSAParameters publicKey = rsa.ExportParameters(false);
            rsa.ImportParameters(publicKey);
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
    public static byte[] SignData(in byte[] data, in RSAParameters privateKey)
    {

        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = KEY_SIZE;
            rsa.ImportParameters(privateKey);
            return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
    public static bool VerifySignature(in byte[] data, in byte[] signature, in RSAParameters publicKey)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = KEY_SIZE;
            rsa.ImportParameters(publicKey);
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    } 
    public static bool VerifySignature(in byte[] data, in byte[] signature, in string publicKeyXml)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = KEY_SIZE;
            rsa.FromXmlString(publicKeyXml);
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
