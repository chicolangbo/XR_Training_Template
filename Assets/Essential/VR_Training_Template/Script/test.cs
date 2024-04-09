using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public void OnHoverEntered()
    {
        Debug.Log($"{gameObject.name}OnHoverEntered");
    }

    public void OnHoverExited()
    {
        Debug.Log($"{gameObject.name}OnHoverExited");
    }
    public void OnSelectEntered()
    {
        Debug.Log($"{gameObject.name}OnSelectEntered");
    }
    public void OnSelectExited()
    {
        Debug.Log($"{gameObject.name}OnSelectExited");
    }
}
