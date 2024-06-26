using System;
using System.Security.Cryptography;
using System.Xml;
using UnityEngine;

public class GenerateKeyPairInUnityMono : AbstractKeyPairRsaHolderMono
{
    

    [TextArea(0, 10)]
    public string m_publicXmlKey;
    [TextArea(0,2)]
    public string m_privateXmlKey;

    public string m_messageToSign="Bonjour";
    public string m_messageToSigned;
    public bool isSignatureValid;

    void Start()
    {
        GeneratePrivatePublicRsaKey();
    }

    [ContextMenu("Generate Random Public Private RSA Key")]
    private void GeneratePrivatePublicRsaKey()
    {
        RSA rsa = RSA.Create();
        
        
            rsa.KeySize = 1024;
            m_privateXmlKey = rsa.ToXmlString(true);
            m_publicXmlKey = rsa.ToXmlString(false);
 
    }


    [ContextMenu("Sign message and check signature")]
    public void SignAndCheckMessage() {
        RSA rsa = RSA.Create();
        rsa.KeySize = 1024;
        rsa.FromXmlString(m_privateXmlKey);
        SignMessage(rsa);
        CheckSignMessage(rsa);
    }


    private void CheckSignMessage(RSA rsa)
    {
        byte[] messageBytes, signature;
        messageBytes = Convert.FromBase64String(m_messageToSigned);

        signature = rsa.SignData(messageBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        // Verify the signature using the public key
        isSignatureValid = rsa.VerifyData(messageBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        Debug.Log("Signature verification result: " + isSignatureValid);
    }

    private void SignMessage(RSA rsa)
    {

        byte[] messageBytes, signature;
      
        messageBytes = System.Text.Encoding.UTF8.GetBytes(m_messageToSign);
        signature = rsa.SignData(messageBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        m_messageToSigned = Convert.ToBase64String(signature);
    }

    public override void GetKeyPairXml(out string keyPrivate, out string keyPublic)
    {
        keyPrivate = m_privateXmlKey;
        keyPublic = m_publicXmlKey;
    }

    public override void GetPrivateXml(out string key)
    {
        key = m_privateXmlKey;
    }

    public override void GetPublicXml(out string key)
    {
        key = m_publicXmlKey;
    }
}

