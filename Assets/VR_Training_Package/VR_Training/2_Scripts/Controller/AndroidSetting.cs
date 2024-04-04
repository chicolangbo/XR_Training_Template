using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class AndroidData
{
    [Tooltip("노말맵 숨기기 설정 여부")] public bool NormalMapHide;
    [Tooltip("메탈릭 설정 여부")] public bool Metallic;
    [Range(0, 1.0f)]
    public float MetallicValue;
    [Tooltip("메탈릭스무스 설정 여부")] public bool MetallicSmooth;
    [Range(0, 1.0f)]
    public float MetallicSmoothValue;
    [Tooltip("베이스 칼라 색상 설정 여부")] public bool ColorAdjust;
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
        //안드로이드 아닐시 리턴 
        if(Application.platform != RuntimePlatform.Android) return; 

        m_Renderer = GetComponent<Renderer>();
 
        //오브젝트 hide
        if(GameObjectHide)
        {
            gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < androidDataList.Count; i++)
        {
            //노멀맵 삭제
            if(androidDataList[i].NormalMapHide)
            {
                m_Renderer.materials[i].SetTexture("_CubeMap", null);
                m_Renderer.materials[i].SetTexture("_BumpMap", null);
                m_Renderer.materials[i].SetTexture("_ParallaxMap", null);
                m_Renderer.materials[i].SetTexture("Texture2D_80da41e58a1049bdb8bc19f52cb7537a", null);
            }

            //메탈릭 조정
            if (androidDataList[i].Metallic)
            {
                m_Renderer.materials[i].SetFloat("_Metallic", androidDataList[i].MetallicValue);
            }

            //스무스 조정
            if (androidDataList[i].MetallicSmooth)
            {
                m_Renderer.materials[i].SetFloat("_Smoothness", androidDataList[i].MetallicSmoothValue);
            }

            //칼라adjust 
            if (androidDataList[i].ColorAdjust)
            {
                m_Renderer.materials[i].SetColor("_BaseColor", androidDataList[i].ColorValue);
                m_Renderer.materials[i].SetColor("Color_754d9587a1d04e5792344f0264fd3bd5", androidDataList[i].ColorValue);
            }
        }

    }

//#endif


}
