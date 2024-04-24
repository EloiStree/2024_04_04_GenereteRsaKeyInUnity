using System.Security.Cryptography;

public class KeyPairRsaHolderToSignMessageUtility {


    public static byte[] SignData(byte[] data, IKeyPairRsaHolderMono keyPair)
    {
        keyPair.GetPrivateXml(out string xmlPrivate);
        return SignData(data, xmlPrivate);
    }

    public static byte[] SignData(byte[] data, string xmlPrivate)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = 1024;
            rsa.FromXmlString(xmlPrivate);
            RSAParameters privateKey = rsa.ExportParameters(true);
            rsa.ImportParameters(privateKey);
            return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
    public static bool VerifySignature(byte[] data, byte[] signature, IKeyPairRsaHolderMono keyPair)
    {
        using (RSA rsa = RSA.Create())
        {
            keyPair.GetPrivateXml(out string xmlPrivate);
            rsa.KeySize = 1024;
            rsa.FromXmlString(xmlPrivate);
            RSAParameters publicKey = rsa.ExportParameters(false);
            rsa.ImportParameters(publicKey);
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
    public static byte[] SignData(byte[] data, RSAParameters privateKey)
    {

        using (RSA rsa = RSA.Create())
        {
            rsa.ImportParameters(privateKey);
            return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
    public static bool VerifySignature(byte[] data, byte[] signature, RSAParameters publicKey)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportParameters(publicKey);
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    } 
    public static bool VerifySignature(byte[] data, byte[] signature, string publicKeyXml)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.FromXmlString(publicKeyXml);
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
