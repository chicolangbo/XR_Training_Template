using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ThermalRunawayTrainingManager : MonoBehaviour
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

    public VideoPlayer videoPlayer;

    public GameObject effect;
    public MeshRenderer mesh;
    public Material material;

    public AudioSource audio;

    private static ThermalRunawayTrainingManager instance;
    public static ThermalRunawayTrainingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(ThermalRunawayTrainingManager)) as ThermalRunawayTrainingManager;
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
            if (MissionEnvController.instance.curMissionData.id == "5")
            {
                isFirst = true;
                videoPlayer.Play();
            }
        }
        if (isFirst2 == false)
        {
            if (MissionEnvController.instance.curMissionData.id == "7")
            {
                isFirst2 = true;
                videoPlayer.enabled = false;
                mesh.material = material;
                Invoke("EffectOn",1.5f);
                LoadScene("Thermal Runaway_Result");
            }
        }
    }

    public void EffectOn()
    {
        effect.SetActive(true);
        audio.Play();
    }

    public void SetPosition(int num)
    {
        player.position = savePosition[num].transform.position;
        map.rotation = savePosition[num].transform.localRotation;
        mainCamera.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
    }
    public void SetCharacter(GameObject a)
    {
        character = a;
    }

    public void LoadScene(string sceneName)
    {
        ProcessManager.instance.SetProcessFromTutorial(sceneName);
        StartCoroutine(SceneLoad(sceneName));
    }
    IEnumerator SceneLoad(string sceneName)
    {
        yield return new WaitForSeconds(20.0f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        yield return Resources.UnloadUnusedAssets();
        Debug.Log($"{sceneName} - Scene Load Complete");


    }
}
