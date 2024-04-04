using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ChangeColorRay : MonoBehaviour
{
    public GameObject rayLine;
    Renderer liner;
    void Start()
    {
        ChangeRayColor();
    }

    void ChangeRayColor()
    {
        liner = rayLine.GetComponent<Renderer>();
        liner.material.color = new Color32(255, 255, 255, 100);
    }
}