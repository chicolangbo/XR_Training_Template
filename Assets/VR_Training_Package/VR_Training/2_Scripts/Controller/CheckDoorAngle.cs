using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckDoorAngle : MonoBehaviour
{
    HingeJoint hinge;
    Rigidbody rb;
    public bool IsMaxAngle = false;
    public bool IsMinAngle = false;
    public bool IsDoorMoving = false;
    public GameObject HandleParent;
    public BoxCollider HandleBox;
    public XRGrabInteractable HandleGrab;
    public FollowHandle follow;
    public bool IsOpenAction = false;
    Vector3 originPos;
    float originAnchorY;
    GameObject car; 

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        car = GameObject.Find("Car"); 
        if(car == null)
        {
            car = GameObject.Find("CAR/EV_ROOT/main");
        }

        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
        SetDoor(false);
        originAnchorY = hinge.connectedAnchor.y;
    }

    public void SetDoor(bool enable)
    {
        follow.enabled = enable; 
        rb.isKinematic = !enable; 
        this.enabled = enable;
        HandleBox.enabled = enable;
        HandleGrab.enabled = enable;
        HandleParent.SetActive(enable);


    }

    // Update is called once per frame
    void Update()
    {
        //현가장치일경우 리프트 업,다운에 따른 hinge y 축 조정 auto connect diable 해야함 
        if(car)
        {
            Vector3 anchor = hinge.connectedAnchor;
            anchor.y = car.transform.position.y + originAnchorY;
            hinge.connectedAnchor = anchor;
        }


        if (IsDoorMoving)
        {

            if (IsMaxAngle == false)
            {
                if (transform.localEulerAngles.z >= hinge.limits.max - 0.1f)
                { 
                    rb.isKinematic = true;
                    IsMaxAngle = true; 
                }
            }

            if (IsMinAngle == false)
            {
                if (transform.localEulerAngles.z <= hinge.limits.min + 3)
                {
                    if (IsOpenAction)
                        return; 

                    Vector3 angle = transform.localEulerAngles;
                    angle.z = 0;
                    transform.localEulerAngles = angle; 
                    IsMinAngle = true;
                    rb.isKinematic = true;
                }
            }

            if (transform.localEulerAngles.z < hinge.limits.max - 1)
            {
              
                IsMaxAngle = false;
            }

            if (transform.localEulerAngles.z > hinge.limits.min + 3)
            {
                IsMinAngle = false;
            }

        }


    }
}
