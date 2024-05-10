using UnityEngine;

public class GenerateAndStoreKeyPairInUnityOverrideMono : MonoBehaviour {

    public GenerateAndStoreKeyPairInUnityMono m_toOverride;
    public bool m_removeWhenOverride;
    [Header("Insert key here and call context menu")]

    [TextArea(0, 5)]
    public string m_privateKeyXmlToOverride;



    [ContextMenu("Override RSA Private key")]
    public void CallOverrideRsaPrivateKeyXml() {
        m_toOverride.OverridePrivateKey(m_privateKeyXmlToOverride);
        if (m_removeWhenOverride)
            m_privateKeyXmlToOverride = "";
    }
}
