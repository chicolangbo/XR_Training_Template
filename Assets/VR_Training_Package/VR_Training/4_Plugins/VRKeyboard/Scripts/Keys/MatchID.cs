using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchID : MonoBehaviour
{
    #region Public Variables
    public InputField userName, passWord;
    public Button login;
    public Text text;
    #endregion

    private string InventisID = "qwe", InventisPW = "123";
    // Start is called before the first frame update
    void Start()
    {
        SetLogin();
    }

    void Update()
    {
       
    }

    void SetLogin()
    {
        login.onClick.AddListener(SearchAccount);
        
    }
    public void SearchAccount()
    {
        // ���̵� �н����� ��ġ�� �� ���� ������ ��ȯ
        if (InventisID == userName.text && InventisPW == passWord.text)
        {
            SceneManager.LoadScene("NEW_SHOP", LoadSceneMode.Single);
        } 
        else if (userName.text == "" && passWord.text == "")
        {
            text.text = "��� �Է��� �Ϸ��� �ֽñ� �ٶ��ϴ�";
        } 
        else if (InventisID != userName.text || InventisPW != passWord.text)
        {
            text.text = "������ �������� �ʰų� ��й�ȣ�� �ٸ��ϴ�.";
        }

    }

}
