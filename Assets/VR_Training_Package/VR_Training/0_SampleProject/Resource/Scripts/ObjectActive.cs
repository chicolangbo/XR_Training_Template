using UnityEngine;

public class ObjectActive : MonoBehaviour
{
    private bool activeState = false;
    void Start()
    {
                
    }
    void Update()
    {
        if (activeState == true)
        {
            ActiveON();
        }
        else if (activeState == false)
        {
            ActiveOFF();
        }
    }
    //(호버,클릭,액티브) 시 보이기
    public void ActiveON()
    {
        activeState = true;
        this.gameObject.SetActive(activeState);
    }

    //(호버,클릭,액티브) 아닐 시 끄기
    public void ActiveOFF()
    {
        activeState = false;
        this.gameObject.SetActive(activeState);
    }
}
