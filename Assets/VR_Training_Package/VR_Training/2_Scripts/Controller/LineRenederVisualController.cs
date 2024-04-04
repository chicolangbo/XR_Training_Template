using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenederVisualController : MonoBehaviour
{

    public LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }


    public void SetLineRendererThickness(float thickness)
    {
        lineRenderer.SetWidth((float)thickness, (float)thickness);
    }


    void Update()
    {
        
    }
}
