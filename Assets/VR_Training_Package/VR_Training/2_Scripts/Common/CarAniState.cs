using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAniState : MonoBehaviour
{
    public Animator main;
    public Animator center;
    const string CENTER_MOVE = "Center_Move";
    const string MAIN_MOVE = "Main_Move";
    // Start is called before the first frame update
    void Start()
    {
        main.SetFloat(CENTER_MOVE, 1);
        center.SetFloat(CENTER_MOVE, 1);
    }

   
}
