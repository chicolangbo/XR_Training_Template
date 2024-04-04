using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCreate : MonoBehaviour
{
    public GameObject Yellow_Box;
    public GameObject Red_Box;
    public GameObject Green_Box;

    Vector3 gp = new Vector3(1.5f, 0, 2.5f);
    Vector3 rp = new Vector3(0, 0, 2.5f);
    Vector3 yp = new Vector3(-1.5f, 0, 2.5f);

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame

    public void BoxCreateOn()
    {
        int random_v = Random.Range(1, 4);

        if (random_v == 1)
            Instantiate(Yellow_Box, yp, Quaternion.identity);
        if (random_v == 2)
            Instantiate(Red_Box, rp, Quaternion.identity);
        if (random_v == 3)
            Instantiate(Green_Box, gp, Quaternion.identity);

    }
}
