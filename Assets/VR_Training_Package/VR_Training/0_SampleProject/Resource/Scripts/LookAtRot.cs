using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LookAtRot : MonoBehaviour
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
                    tp = new Vector3(this.transform.rotation.x, mainView.rotation.y, mainView.rotation.z);
                    this.transform.LookAt(tp);
                    break;

                case Axis.axis_Y:
                    tp = new Vector3(mainView.rotation.x, this.transform.rotation.y, mainView.rotation.z);
                    this.transform.LookAt(tp);
                    break;

                case Axis.axis_Z:
                    tp = new Vector3(mainView.rotation.x, mainView.rotation.y, this.transform.rotation.z);
                    this.transform.LookAt(tp);
                    break;

                case Axis.Hold:
                    this.transform.LookAt(mainView);
                    break;
            }
        }
    }
}
