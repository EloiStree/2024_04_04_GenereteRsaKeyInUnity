
using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

public class GenerateAndStoreKeyPairInUnityMono : AbstractKeyPairRsaHolderMono
{


    [TextArea(0, 10)]
    public string m_publicXmlKey;
    string m_privateXmlKey;

    public UnityEvent<string> m_onPublicXmlLoaded;
    public UnityEvent<string> m_onPrivateXmlLoaded;
    public UnityEvent m_onKeyPairLoaded;
    public bool m_createNewOneEveryTime = false;
    public string m_subFolderName = "Default";
    void Start()
    {
        GeneratePrivatePublicRsaKey();
    }

    public void OverrideFromInspectorValue()
    {
        OverridePrivateKey(m_privateXmlKey);
    }
    public void OverridePrivateKey(string privateKeyRsa)
    {
      
        GenerateAndReadKeyPairInPermaSubfolder.OverrideWithPrivateKey(m_subFolderName, privateKeyRsa, out _);
        GeneratePrivatePublicRsaKey();
    }

    [ContextMenu("Generate Random Public Private RSA Key")]
    private void GeneratePrivatePublicRsaKey()
    {
        GenerateAndReadKeyPairInPermaSubfolder.GetOrCreatePrivatePublicRsaKey(m_subFolderName, out m_privateXmlKey, out m_publicXmlKey, false);

      
        m_onPublicXmlLoaded.Invoke(m_publicXmlKey);
        m_onPrivateXmlLoaded.Invoke(m_privateXmlKey);
        Debug.Log("Public and private keys Stored in:\n " + GenerateAndReadKeyPairInPermaSubfolder.GetDirectoryPathOfDevice(m_subFolderName));
        m_onKeyPairLoaded.Invoke();



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

    public void DebugLog(string message)
    {
        Debug.Log(message);

    }
    public void DebugLogPrivateKey(string message)
    {
        Debug.Log("Private Key: " + message);

    }
    public void DebugLogPublicKey(string message)
    {
        Debug.Log("Public Key: "+message);

    }
}


public static class GenerateAndReadKeyPairInPermaSubfolder
{


    public static void GetOrCreatePrivatePublicRsaKey(string subFolderName, out string privateXmlKey, out string publicXmlKey, bool createNewOneEveryTime = false)
    {
        RSA rsa = RSA.Create();
        rsa.KeySize = 1024;
        string path = Path.Combine(Application.persistentDataPath, Path.Combine("KeyPair", subFolderName));
        string pathPublic = Path.Combine(path, "RSA_PUBLIC_XML.txt");
        string pathPrivate = Path.Combine(path, "RSA_PRIVATE_XML.txt");

        privateXmlKey = rsa.ToXmlString(true);
        publicXmlKey = rsa.ToXmlString(false);


        if (createNewOneEveryTime || !Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            System.IO.File.WriteAllText(pathPublic, publicXmlKey);
            System.IO.File.WriteAllText(pathPrivate, privateXmlKey);
        }


        publicXmlKey = System.IO.File.ReadAllText(pathPublic);
        privateXmlKey = System.IO.File.ReadAllText(pathPrivate);

    }
    public static void OverrideWithPrivateKey(string subFolderName,  string privateXmlKey, out string publicXmlKey)
    {
        RSA rsa = RSA.Create();
        rsa.KeySize = 1024;
        rsa.FromXmlString(privateXmlKey);
        string path = Path.Combine(Application.persistentDataPath, Path.Combine("KeyPair", subFolderName));
        string pathPublic = Path.Combine(path, "RSA_PUBLIC_XML.txt");
        string pathPrivate = Path.Combine(path, "RSA_PRIVATE_XML.txt");

        privateXmlKey = rsa.ToXmlString(true);
        publicXmlKey = rsa.ToXmlString(false);

            Directory.CreateDirectory(path);
            System.IO.File.WriteAllText(pathPublic, publicXmlKey);
            System.IO.File.WriteAllText(pathPrivate, privateXmlKey);

    }

    public  static string GetDirectoryPathOfDevice(string subFolderName="Default")
    {
        return Path.Combine(Application.persistentDataPath, Path.Combine("KeyPair", subFolderName));
    }
}