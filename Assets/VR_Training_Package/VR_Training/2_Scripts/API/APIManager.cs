using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;
    const string BaseURL = "http://localhost:5000/";
    const string get_current_user = "get_current_user";
    const string post = "post"; 
    private void Start()
    {
        //if (!instance)
           // instance = this;

        //DontDestroyOnLoad(this.gameObject);

        //post
        //StartCoroutine(APIRequestByPost(BaseURL + post));
        //get
        //StartCoroutine(APIRequestByGet(BaseURL + get_current_user)); 
    }

    public void Get()
    {
        Debug.Log("Get Method");
    }


    IEnumerator APIRequestByPost(string url)
    {
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();

        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddField("test", "testmessaage");

        // Upload to a cgi script
        using (var w = UnityWebRequest.Post(BaseURL,form))
        {
            //w.SetRequestHeader("Content-Type", "application/json");
            yield return w.SendWebRequest();
            if (w.result != UnityWebRequest.Result.Success)
            {
                FailCallback(url, w);
            }
            else
            {
                SuccessCallback(url, w);
            }
        }
    }

    IEnumerator APIRequestByGet(string url)
    {
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();

        // Upload to a cgi script
        using (var w = UnityWebRequest.Get(url))
        {
            yield return w.SendWebRequest();
            if (w.result != UnityWebRequest.Result.Success)
            {
                FailCallback(url, w); 
            }
            else
            {
                SuccessCallback(url,w);
            }
        }
    }

    void SuccessCallback(string url,UnityWebRequest w)
    {
        switch(url)
        {
            case BaseURL + post:
                Debug.Log(w.downloadHandler.text);
                break; 

            case BaseURL + get_current_user:
                Debug.Log(w.downloadHandler.text);
                UserInfomation user = JsonUtility.FromJson<UserInfomation>(w.downloadHandler.text);
                Debug.Log(user.username);
                Debug.Log(user.email);
                Debug.Log(user.id); 
                break; 
        }
    }

    void FailCallback(string url,UnityWebRequest w)
    {
        switch (url)
        {

            case BaseURL + post:
                Debug.Log(w.error);
                break;

            case BaseURL + get_current_user:
                Debug.Log(w.error);
                break;
        }
    }

}
