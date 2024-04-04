using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LookAtCamera : MonoBehaviour
{
    public enum Axis
    {
        axis_X,
        axis_Y,
        axis_Z,
        Hold
    }

    Vector3 tp;
    public Axis axis = Axis.Hold;
    public Transform mainView; 
    public bool LookAtUse = true;
    
    void Start()
    {
        Debug.Log(this.gameObject.name);
        if(mainView == null) 
            mainView = GameObject.FindWithTag("CameraView").transform;
    }

    void Update()
    {
        FindCamera();
    }

    void FindCamera()
    {
        if (mainView && !LookAtUse)
        {
            switch (axis)
            {
                case Axis.axis_X:
                    tp = new Vector3(this.transform.position.x, mainView.position.y, mainView.position.z);
                    this.transform.LookAt(tp);
                    break;

                case Axis.axis_Y:
                    tp = new Vector3(mainView.position.x, this.transform.position.y, mainView.position.z);
                    this.transform.LookAt(tp);
                    break;

                case Axis.axis_Z:
                    tp = new Vector3(mainView.position.x, mainView.position.y, this.transform.position.z);
                    this.transform.LookAt(tp);
                    break;

                case Axis.Hold:
                    this.transform.LookAt(mainView);
                    break;
            }
        }
    }
}
