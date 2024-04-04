using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalRunawayManager : MonoBehaviour
{
    public GameObject[] savePosition;

    // Start is called before the first frame update
    public Transform player;
    public Transform mainCamera;
    public Transform map;

    public GameObject character;
    bool isPlaying = false;

    public int hightlightNum = 0;

    public Highlighter[] highlighers;
    public Highlighter lineHighligher;
    public GameObject HighlighterOBJ;

    bool isFirst = false;
    bool isFirst2 = false;
    bool isFirst3 = false;
    bool isFirst4 = false;
    bool isFirst5 = false;
    bool isFirst6 = false;

    public GameObject battery;
    public GameObject hand_character;

    public GameObject temperatureLine;
    public Animator lineAni;
    public GameObject LastLine;

    public Material mm;
    public Material mm2;
    public Material mm3;
    public Material mm4;
    Shader shader;
    Shader shader1;
    Shader shader2;
    Shader shader3;

    public GameObject[] ChangeLine;
    public GameObject Achril_OBJ;
    private static ThermalRunawayManager instance;
    public static ThermalRunawayManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(ThermalRunawayManager)) as ThermalRunawayManager;
            }
            return instance;
        }
    }

    void Start()
    {
        battery.SetActive(false);
        //mm = lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[0];
        shader = lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[0].shader;
        shader1 = lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[1].shader;
        shader2 = lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[2].shader;
        shader3 = lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[3].shader;
        //mm2 = lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[1];
        //mm3 = lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[2];
        //mm4 = lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[3];
        lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[2].shader = shader3;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[3].shader = Shader.Find("Shader Graphs/Lit_Rim_Line");
        //    lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[3].SetFloat("Rim_intensity", 0f);
        //}
        if (Input.GetKeyDown(KeyCode.C))
        {
            lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[2] = mm4;
            lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[0] = mm4;
        }
        if (!MissionEnvController.instance.GetNarrationPlayer().isPlaying)
        {
            if (isPlaying)
            {
                if (character != null)
                    character.GetComponent<Animator>().SetTrigger("IDLE");

                isPlaying = false;
            }
        }
        else
        {
            if (isPlaying == false)
            {
                if (character != null)
                    character.GetComponent<Animator>().SetTrigger("PLAY");

                isPlaying = true;
            }
        }

        if (isFirst4 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "11")
            {
                hand_character.SetActive(false);
                temperatureLine.SetActive(false);
                isFirst4 = true;
            }
        }
        if (isFirst3 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "12")
            {
                lineAni.gameObject.SetActive(false);
                LastLine.SetActive(true);
                isFirst3 = true;
            }
        }

        //Debug.Log(" s " + MissionEnvController.instance.curMissionData.id);
        if (isFirst == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "6")
            {
                isFirst = true;
                battery.SetActive(true);
                character.SetActive(false);
                SetPosition(1);

            }
        }
        if (isFirst2 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "14")
            {
                isFirst2 = true;
                ChangeLine[0].SetActive(false);
                ChangeLine[1].SetActive(true);
                SetPosition(2);
            }
        }
        if (isFirst6 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "13")
            {
                isFirst6 = true;
                Achril_OBJ.SetActive(true);
            }
        }
        if (isFirst5 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "9")
            {
                isFirst5 = true;
                //lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[2] = mm3;
                //lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[0] = mm2;

                lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[2].shader = shader2;
                lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[0].shader = shader1;
                //lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[3].shader = Shader.Find("Shader Graphs/Lit_Rim_Line");
                //lineAni.gameObject.GetComponent<SkinnedMeshRenderer>().materials[3].SetFloat("Rim_intensity", 0f);
            }
        }
    }


    public void SetPosition(int num)
    {
        player.position = savePosition[num].transform.position;
        map.rotation = savePosition[num].transform.localRotation;
        mainCamera.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void SetCharacter(GameObject a)
    {
        character = a;
    }

    public void HighlighterOn()
    {
        highlighers[hightlightNum].HighlightOn();
    }

    public void HighlighterOff()
    {
        highlighers[hightlightNum].HighlightOff();
        hightlightNum++;
    }
    
    public void HighlighterOnOFF()
    {
        //HighlighterOBJ.SetActive(!HighlighterOBJ.activeSelf);
    }

    public IEnumerator LineAni()
    {
        lineAni.SetTrigger(A.ON);
        yield return new WaitForSeconds(3.0f);
        highlighers[hightlightNum].HighlightOff();
        lineHighligher.HighlightOn();
        highlighers[hightlightNum] = lineHighligher;
    }

}
