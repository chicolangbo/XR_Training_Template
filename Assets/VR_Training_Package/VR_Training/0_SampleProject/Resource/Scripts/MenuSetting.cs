using UnityEngine;

public class MenuSetting : MonoBehaviour
{
    public GameObject MenuSelects;
    public GameObject SettingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        //SettingsMenu.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
    // 환경설정 키고 메인 끄기
    public void SettingsON()
    {
        MenuSelects.SetActive(false);
        SettingsMenu.SetActive(true);
    }
    // 환경설정 끄고 메인 키기
    public void SettingsOFF()
    {

        MenuSelects.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void MainMenus()
    {

    }

    public void XRRigKeyOnOff()
    {
        //X 키 누를 시 온/오프



    }


}
