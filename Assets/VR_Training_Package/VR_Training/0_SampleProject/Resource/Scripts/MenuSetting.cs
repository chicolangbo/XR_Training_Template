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
    // ȯ�漳�� Ű�� ���� ����
    public void SettingsON()
    {
        MenuSelects.SetActive(false);
        SettingsMenu.SetActive(true);
    }
    // ȯ�漳�� ���� ���� Ű��
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
        //X Ű ���� �� ��/����



    }


}
