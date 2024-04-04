using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThermalRunawayResultManager : MonoBehaviour
{
    public GameObject[] savePosition;

    // Start is called before the first frame update
    public Transform player;
    public Transform mainCamera;
    public Transform map;

    public GameObject character;
    bool isPlaying = false;


    bool isFirst = false;
    bool isFirst2 = false;
    bool isFirst3 = false;
    bool isFirst4 = false;
    bool isFirst5 = false;
    bool isFirst6 = false;
    bool isFirst7 = false;

    public GameObject effect;
    public MeshRenderer mesh;
    public Material material;

    public RawImage screenImage;
    public GameObject result3;
    public GameObject result4;
    public GameObject result5;

    public GameObject screenGraph;

    public Texture2D[] screens;

    public GameObject startHighlight;

    private static ThermalRunawayResultManager instance;
    public static ThermalRunawayResultManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(ThermalRunawayResultManager)) as ThermalRunawayResultManager;
            }
            return instance;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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

        //Debug.Log(" s " + MissionEnvController.instance.curMissionData.id);
        if (isFirst == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "1")
            {
                isFirst = true;
                character.SetActive(false);
                SetPosition(0);

            }
        }
        if (isFirst2 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "8")
            {
                isFirst2 = true;
                screenGraph.SetActive(false);
                screenImage.texture = screens[0];
            }
        }
        if (isFirst3 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "9")
            {
                isFirst3 = true;
                screenImage.texture = screens[1];
                Invoke("Result3", 1.0f);
            }
        }
        if (isFirst4 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "10")
            {
                isFirst4 = true;
                result3.SetActive(false);
                screenImage.texture = screens[2];
                Invoke("Result4", 1.0f);
            }
        }
        if (isFirst5 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "11")
            {
                isFirst5 = true;
                result4.SetActive(false);
                screenImage.gameObject.SetActive(false);
                result5.SetActive(true);
            }
        }
        if (isFirst6 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "5")
            {
                isFirst6 = true;
                startHighlight.SetActive(true);
            }
        }
        if (isFirst7 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "6")
            {
                isFirst7 = true;
                startHighlight.SetActive(false);
                screenImage.gameObject.SetActive(true);
                screenGraph.SetActive(true);
            }
        }
    }
    public void Result3()
    {
        result3.SetActive(true);
    }
    public void Result4()
    {
        result4.SetActive(true);
    }

    public void EffectOn()
    {
        effect.SetActive(true);
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

}
