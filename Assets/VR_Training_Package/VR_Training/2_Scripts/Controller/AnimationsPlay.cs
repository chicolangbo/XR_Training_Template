using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsPlay : MonoBehaviour
{
    public Animator[] _ani;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayAnimations() //�⺻ Ʈ����
    {
        if (_ani.Length > 0)
        {
            for(int i=0; i<_ani.Length; i++)
            {
                _ani[i].SetTrigger("ON");
            }
        }
    }
    public void StopAnimations() //�⺻ Ʈ����
    {
        if (_ani.Length > 0)
        {
            for (int i = 0; i < _ani.Length; i++)
            {
                _ani[i].SetTrigger("OFF");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
