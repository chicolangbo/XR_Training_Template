using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enter : MonoBehaviour
{
    #region Public Variables
    public Text textValue;
    public Text userName,passWord;
    public Button button;
    public bool pass = false, end = false;
    #endregion
    
    void Start()
    {
        //SetFunction_UI();
        Function_Button();
    }
    void Update()
    {
        /*
        // �� �Է� ���� �� ������
        if (!end)
        {   // ���̵� �Է� ����ȭ
            if (!pass)
            {
                string txt = textValue.text;
                userName.text = txt;
            }// �н����� �Է� ����ȭ
            else if (pass)
            {
                string txt = textValue.text;
                passWord.text = txt;
            }
        }*/
    }
    void SetFunction_UI()
    {
        button.onClick.AddListener(Function_Button);
    }

    // ���̵� �н����� �� �޾ƿ���
    void Function_Button()
    {
        /*
        userName.text = "Inventis";
        passWord.text = "Inventis!1";
        */
        
        if (!end)
        {
            if (!pass)
            {
                string txt = textValue.text;
                userName.text = txt;
                textValue.text = "";
                pass = true;
            }
            else if (pass)
            {
                string txt = textValue.text;
                passWord.text = txt;
                textValue.text = "";
                end = true;
            }
        }
    }

}
