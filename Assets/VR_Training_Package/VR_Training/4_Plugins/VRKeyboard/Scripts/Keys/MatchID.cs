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
        // 아이디 패스워드 일치할 시 차고 씬으로 전환
        if (InventisID == userName.text && InventisPW == passWord.text)
        {
            SceneManager.LoadScene("NEW_SHOP", LoadSceneMode.Single);
        } 
        else if (userName.text == "" && passWord.text == "")
        {
            text.text = "모든 입력을 완료해 주시기 바랍니다";
        } 
        else if (InventisID != userName.text || InventisPW != passWord.text)
        {
            text.text = "계정이 존재하지 않거나 비밀번호가 다릅니다.";
        }

    }

}
