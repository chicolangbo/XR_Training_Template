using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraUI : MonoBehaviour
{
    //public Text caution;
    //public Text course;

    public GameObject course_title;
    //public TMP_Text course;
    public Text course;
    public Text course2;
    public GameObject caution_title;
    //public TMP_Text caution;
    public Text caution;
    //public TMP_Text addexplain;
    public Text addexplain;
    public GameObject addexplain_title;
    public Image under_bg;
    public GameObject circle;
    public Text narrText;

    public Image joystickTutorial;
    public Sprite[] joyTexture;

    public void Start()
    {
        gameObject.SetActive(false); 
        Invoke("SetTutoImage", 0.5f);
    }

    public void SetTutoImage()
    {
        if(joystickTutorial == null)            
            return;

        if (Secnario_UserContext.instance.currentPlayModeType == EnumDefinition.PlayModeType.TUTORIAL)
        {
            joystickTutorial.gameObject.SetActive(true);
        }




        if (LanguageManager.instance.langType == EnumDefinition.LANGUAGE_TYPE.KR)
        {
            if (XR_ControllerBase.instance.isOnOculus)
                joystickTutorial.sprite = joyTexture[0];
            else
                joystickTutorial.sprite = joyTexture[1];
        }
        else
        {
            if (XR_ControllerBase.instance.isOnOculus)
                joystickTutorial.sprite = joyTexture[2];
            else
                joystickTutorial.sprite = joyTexture[3];
        }
        

        Debug.Log("Change TUTO UI");
    }

    public void SetCourse(string text)
    {
        course.text = text;
        if(course2 !=null)
            course2.text = text;
    }

    public void SetCaution(string text)
    {
        caution.gameObject.SetActive(true);
        caution_title.gameObject.SetActive(true); 
        addexplain.gameObject.SetActive(false);
        addexplain_title.gameObject.SetActive(false); 
        caution.text = text;
        under_bg.enabled = true;
        circle.SetActive(true); 
    }

    public void SetAddExplain(string text)
    {
        caution.gameObject.SetActive(false);
        caution_title.gameObject.SetActive(false);
        addexplain.gameObject.SetActive(true);
        addexplain_title.gameObject.SetActive(true);
        addexplain.text = text;
        under_bg.enabled = true;
        circle.SetActive(true);
    }

    public void HideCautionAndAddExplain()
    {
        caution_title.gameObject.SetActive(false);
        caution.gameObject.SetActive(false);
        addexplain_title.gameObject.SetActive(false);
        addexplain.gameObject.SetActive(false);
        under_bg.enabled = false;
        circle.SetActive(false);
    }

    public void ResetText()
    {
        caution.text = addexplain.text = course.text = "";
        if (course2 != null)
            course2.text = "";
    }

    // how far to stay away fromt he center
    public float offsetRadius = 0.3f;
    public float distanceToHead = 4;

    // This is a value between 0 and 1 where
    // 0 object never moves
    // 1 object jumps to targetPosition immediately
    // 0.5 e.g. object is placed in the middle between current and targetPosition every frame
    // you can play around with this in the Inspector
    [Range(0, 10)]
    public float smoothPosFactor = 5f;

    [Range(0, 10)]
    public float smoothRotFactor = 5f;

    [Range(0, 1)]
    public float leftPosition = 0.2f;

    //private void Update()
    //{
    //    // make the UI always face towards the camera
    //    transform.rotation = Quaternion.Lerp(transform.rotation, Camera.main.transform.rotation, smoothRotFactor);

    //    var cameraCenter = Camera.main.transform.position + Camera.main.transform.forward * distanceToHead - Camera.main.transform.right * leftPosition;

    //    var currentPos = transform.position;

    //    // in which direction from the center?
    //    var direction = currentPos - cameraCenter; 

    //    // target is in the same direction but offsetRadius
    //    // from the center
    //    var targetPosition = cameraCenter + direction.normalized * offsetRadius;

    //    // finally interpolate towards this position
    //    transform.position = Vector3.Lerp(currentPos, targetPosition, Time.deltaTime * smoothPosFactor);

    //}
}
