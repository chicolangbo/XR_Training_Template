using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim : MonoBehaviour
{
    public Animator anim;
    private float blendValue;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        blendValue = 0;
        speed = 0.5f;
        //anim.Play("handle.Any State");
    }

    private void Update()
    {
        blendValue += Time.deltaTime * speed;
        if(blendValue < 1.1f)
            anim.SetFloat("ON", blendValue);
    }
}
