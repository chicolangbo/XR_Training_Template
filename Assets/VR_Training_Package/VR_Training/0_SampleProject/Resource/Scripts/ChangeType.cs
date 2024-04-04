using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeType : MonoBehaviour
{
    public InputField passwordType;
    private bool x = false;
    public Button pwChange;
    void Start()
    {
        SetPassword();
    }

    void SetPassword()
    {
        pwChange.onClick.AddListener(SecretPassword);

    }

    public void SecretPassword()
    {
        Debug.Log("Click");
        if (!x)
        {
            passwordType.contentType = InputField.ContentType.Password;
            x = true;
        }else if (x)
        {
            passwordType.contentType = InputField.ContentType.Standard;
            x = false;
        }
    }
}
