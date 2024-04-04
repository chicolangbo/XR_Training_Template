using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseCertificationManager : MonoBehaviour
{

    public GameObject[] img;
    public GameObject[] savePosition;
    
    public int autoNum = 0;
    // Start is called before the first frame update
    public Transform player;
    public Transform mainCamera;
    public Transform map;

    public bool noShowUI = false;

    public GameObject character;
    bool isPlaying = false;
    private static NoiseCertificationManager instance;
    public static NoiseCertificationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(NoiseCertificationManager)) as NoiseCertificationManager;
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
            if(isPlaying == false)
            {
                if (character != null)
                    character.GetComponent<Animator>().SetTrigger("PLAY");

                isPlaying = true;
            }
        }
    }

    public void Set_Img(int num, bool active)
    {
        if(img[num] != null)
        {
            img[num].SetActive(active);
        }
        else
        {
            Debug.Log("NoiseCertificationManager : img is Null");
        }
    }

    public void AutoImageOn()
    {
        if (img[autoNum] != null)
        {
            img[autoNum].SetActive(true);
        }
        else
        {
            Debug.Log("NoiseCertificationManager : img is Null");
        }
    }
    public void AutoImageOff()
    {
        if (img[autoNum] != null)
        {
            img[autoNum].SetActive(false);
            autoNum += 1;
            Debug.Log("autoNum = " + autoNum);
        }
        else
        {
            Debug.Log("NoiseCertificationManager : img is Null");
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
}
