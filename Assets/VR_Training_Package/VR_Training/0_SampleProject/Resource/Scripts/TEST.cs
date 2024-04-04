using UnityEngine;

public class TEST : MonoBehaviour
{
    Transform beginPos;
    Transform changePos;
    public bool actOn;
    GameObject fbolt;
    Transform fboltRot, fleft, pvot;

    // Start is called before the first frame update
    void Start()
    {
        fboltRot = GameObject.FindWithTag("Bolt").transform;
        fleft = GameObject.FindWithTag("LH").transform;
        pvot = GameObject.FindWithTag("WrenchPivot").transform;
        //beginPos = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (actOn == true)
        {
            boltRotOn();
        }
        else if (actOn == false)
        {
            boltRotOff();
        }
    }
    public void TrackingHand()
    {
        
    }
    public void boltRotOn()
    {
        actOn = true;
        float pvotValue;
        //float boltValue;
        //fbolt.transform.rotation = this.transform.rotation;
        //transform.LookAt(fboltRot);
        //boltValue = remap(pvot.transform.rotation.y, 0, 1f, 0, 360f);
        pvotValue = remap(fleft.transform.rotation.z, -0.3f, 0.7f, 0, 360f);
        pvot.transform.rotation = Quaternion.Euler(0, pvotValue, 0);
        fboltRot.rotation = Quaternion.Euler(-90, 0, pvotValue);

        //pvot.transform.rotation = fleft.transform.rotation;
        //pvot.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0, fleft.transform.rotation.z));

    }

    private float remap(float aValue, float from1, float to1, float from2, float to2)
    {
        float normal = Mathf.InverseLerp(from1, to1, aValue);
        float bValue = Mathf.Lerp(from2, to2, normal);
        return bValue;
    }
    public void boltRotOff()
    {
        actOn = false;
        /*
        Vector3 a;
        a.x = beginPos.position.x;
        a.y = beginPos.position.y;
        a.z = beginPos.position.z;
        //changePos 
        this.transform.position = a;
        */
    }
}
