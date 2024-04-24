using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;



public interface IKeyPairRsaHolderMono {

    public void GetPrivateXml(out string key);
    public void GetPublicXml(out string key);
    public void GetKeyPairXml(out string keyPrivate, out string keyPublic);
}

public abstract class AbstractKeyPairRsaHolderMono : MonoBehaviour, IKeyPairRsaHolderMono
{
    public abstract void GetKeyPairXml(out string keyPrivate, out string keyPublic);
    public abstract void GetPrivateXml(out string key);
    public abstract void GetPublicXml(out string key);
}
