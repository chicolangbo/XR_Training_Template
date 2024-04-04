using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpController : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        //animator.Play("red_on");
        animator.SetBool("Lamp_on", true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
