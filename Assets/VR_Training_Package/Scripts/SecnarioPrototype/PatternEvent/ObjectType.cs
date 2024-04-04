using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectType : MonoBehaviour
{
    public TypeDefinition.ObjectType objectType;
    MeshRenderer mr;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color)
    {
        mr.material.color = color;
    }

    private void OnMouseDown()
    {
        SetColor(Color.red);
    }
    private void OnMouseUp()
    {
        SetColor(Color.white);
    }

}
