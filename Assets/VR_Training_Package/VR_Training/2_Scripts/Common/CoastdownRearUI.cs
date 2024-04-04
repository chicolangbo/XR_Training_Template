using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastdownRearUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] result;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnResult(int n)
    {
        result[n].SetActive(true);
        result[n].transform.localScale = new Vector3(1, 1, 1);
        result[n].GetComponent<Animator>().enabled = true;
    }
    public void OffResult(int n)
    {
        result[n].SetActive(false);
    }
}
