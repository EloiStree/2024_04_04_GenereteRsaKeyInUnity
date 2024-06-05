using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GenerateBlockyForRsa1024Mono : MonoBehaviour
{
 
    public string m_publicXmlKey;

    public byte[] m_publicXmlKeyAsByte;

    public Texture2D m_texture;
    public UnityEvent<Texture2D> m_onTextureGenerated;


    public void SetPublicKey(string publicKey)
    {
        m_publicXmlKey = publicKey;
        GenerateBlockyFromPublicKeyAndPush();
    }
    public Texture2D GetBlocky() {
        return m_texture;
    }

  
    private void OnValidate()
    {
        GenerateBlockyFromPublicKeyAndPush();
    }

    [ContextMenu("Generate and Push")]
    public void GenerateBlockyFromPublicKeyAndPush()
    {
        GenerateBlockyFromPublicKey(m_publicXmlKey, out m_texture);
        m_onTextureGenerated.Invoke(m_texture);
    }

    public bool m_mip;
    public bool m_linear;
    private void GenerateBlockyFromPublicKey(string publicKeyXml1024, out Texture2D texture)
    {
        if (m_publicXmlKey.Length > 0)
        {
            string rsaKeyValueXml = publicKeyXml1024;

            XElement rsaKeyValue = XElement.Parse(rsaKeyValueXml);
            string modulusBase64 = rsaKeyValue.Element("Modulus").Value;
            byte[] mByte = Convert.FromBase64String(modulusBase64);
            m_publicXmlKeyAsByte = mByte;


            int maxColor = (int)(mByte.Length / 3f);

            Color32[] c = new Color32[maxColor];
            for (int i = 0; i < maxColor; i++)
            {
                int iRelative = i * 3;
                if (iRelative < mByte.Length)
                    c[i] = new Color32(mByte[iRelative], mByte[iRelative + 1], mByte[iRelative + 2], 255);
            }
            int width = (int)Mathf.Sqrt(maxColor);
            Texture2D t = new Texture2D(width, width, TextureFormat.RGBA32, m_mip, m_linear);
            t.filterMode = FilterMode.Point;
            int maxColorWidth = width * width;
            for (int i = 0; i < maxColorWidth; i++)
            {
                int x = i % width;
                int y = i / width;
                t.SetPixel(x, y, c[i]);
            }
            t.Apply();
            texture = t;
        }
        else {

            texture = new Texture2D(2, 2);
            for (int i = 0; i < 4; i++)
            {
                texture.SetPixel(i % 2, i / 2, Color.black);
            }
        }
    }
}
