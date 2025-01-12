using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class ImportPrivateKeyFromClipboardMono : MonoBehaviour
{

    public string m_privateXmlKey;
    public UnityEvent<string> m_privateKeyDetectedInClipboard;

    public string m_clipboardPreviousValue;
    public string m_clipboardCurrentValue;

    public void Start()
    {
        InvokeRepeating("CheckClipboard", 0, 1);
    }

    private void CheckClipboard()
    {
        if (Application.isFocused)
        {
            m_clipboardCurrentValue = GUIUtility.systemCopyBuffer;
            if (m_clipboardPreviousValue != m_clipboardCurrentValue)
            {
                m_clipboardPreviousValue = m_clipboardCurrentValue;
                if (m_clipboardCurrentValue.Contains("<RSAKeyValue>"))
                {
                    Debug.Log("RSA key detected in XML format");
                    if (m_clipboardCurrentValue.ToUpper().Contains("<DP>") || m_clipboardCurrentValue.ToUpper().Contains("<DQ>"))
                    {
                        try
                        {
                            Rsa4096SignVerifyUtility.LoadPrivateKey4096FromXml(m_clipboardCurrentValue, out RSA rsa);
                            m_privateXmlKey = m_clipboardCurrentValue;
                            m_privateKeyDetectedInClipboard.Invoke(m_privateXmlKey);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Error while importing RSA key from XML: " + e.Message);
                        }
                    }

                }

              
            }
        }
    }
}
