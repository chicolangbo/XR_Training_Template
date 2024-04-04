using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoginWindow : MonoBehaviour
{

    [Header("Reference")]
    /// <summary>
    /// Referenced UI field
    /// </summary>
    [SerializeField]
    protected InputField username;
    /// <summary>
    /// Referenced UI field
    /// </summary>
    [SerializeField]
    protected InputField password;
    /// <summary>
    /// <summary>
    /// Referenced UI field
    /// </summary>
    [SerializeField]
    protected Button loginButton;
    [SerializeField]
    protected GameObject loadingIndicator;
    /*
    protected override void OnStart()
    {
        base.OnStart();
        username.text = PlayerPrefs.GetString("username", string.Empty);
        password.text = PlayerPrefs.GetString("password", string.Empty);

        if (loadingIndicator != null)
        {
            loadingIndicator.SetActive(false);
        }

        EventHandler.Register("OnLogin", OnLogin);
        EventHandler.Register("OnFailedToLogin", OnFailedToLogin);

        loginButton.onClick.AddListener(LoginUsingFields);
    }

    public void LoginUsingFields()
    {
        LoginManager.LoginAccount(username.text, password.text);
        loginButton.interactable = false;
        if (loadingIndicator != null)
        {
            loadingIndicator.SetActive(true);
        }
    }

    private void OnLogin()
    {
        if (rememberMe != null && rememberMe.isOn)
        {
            PlayerPrefs.SetString("username", username.text);
            PlayerPrefs.SetString("password", password.text);
        }
        else
        {
            PlayerPrefs.DeleteKey("username");
            PlayerPrefs.DeleteKey("password");
        }
        Execute("OnLogin", new CallbackEventData());
        if (LoginManager.DefaultSettings.loadSceneOnLogin)
        {
            SceneManager.LoadScene("Garage");
        }
    }

    private void OnFailedToLogin()
    {
        Execute("OnFailedToLogin", new CallbackEventData());
        username.text = "";
        password.text = "";
        LoginManager.Notifications.loginFailed.Show(delegate (int result) { Show(); }, "OK");
        loginButton.interactable = true;
        if (loadingIndicator != null)
        {
            loadingIndicator.SetActive(false);
        }
        Close();
    }*/
}
