using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class AndroidData
{
    [Tooltip("�븻�� ����� ���� ����")] public bool NormalMapHide;
    [Tooltip("��Ż�� ���� ����")] public bool Metallic;
    [Range(0, 1.0f)]
    public float MetallicValue;
    [Tooltip("��Ż�������� ���� ����")] public bool MetallicSmooth;
    [Range(0, 1.0f)]
    public float MetallicSmoothValue;
    [Tooltip("���̽� Į�� ���� ���� ����")] public bool ColorAdjust;
    public Color ColorValue;
}

public class AndroidSetting : MonoBehaviour
{
    public bool GameObjectHide;
  
    [SerializeField]
    List<AndroidData> androidDataList;

    Renderer m_Renderer;


//#if UNITY_ANDROID && !UNITY_EDITOR

    // Start is called before the first frame update
    void Start()
    {
        //�ȵ���̵� �ƴҽ� ���� 
        if(Application.platform != RuntimePlatform.Android) return; 

        m_Renderer = GetComponent<Renderer>();
 
        //������Ʈ hide
        if(GameObjectHide)
        {
            gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < androidDataList.Count; i++)
        {
            //��ָ� ����
            if(androidDataList[i].NormalMapHide)
            {
                m_Renderer.materials[i].SetTexture("_CubeMap", null);
                m_Renderer.materials[i].SetTexture("_BumpMap", null);
                m_Renderer.materials[i].SetTexture("_ParallaxMap", null);
                m_Renderer.materials[i].SetTexture("Texture2D_80da41e58a1049bdb8bc19f52cb7537a", null);
            }

            //��Ż�� ����
            if (androidDataList[i].Metallic)
            {
                m_Renderer.materials[i].SetFloat("_Metallic", androidDataList[i].MetallicValue);
            }

            //������ ����
            if (androidDataList[i].MetallicSmooth)
            {
                m_Renderer.materials[i].SetFloat("_Smoothness", androidDataList[i].MetallicSmoothValue);
            }

            //Į��adjust 
            if (androidDataList[i].ColorAdjust)
            {
                m_Renderer.materials[i].SetColor("_BaseColor", androidDataList[i].ColorValue);
                m_Renderer.materials[i].SetColor("Color_754d9587a1d04e5792344f0264fd3bd5", androidDataList[i].ColorValue);
            }
        }

    }

//#endif


}
