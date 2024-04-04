using UnityEngine;

public class BoltRot : MonoBehaviour
{
    public bool onoff = false;
    public float addForce;
    public float sumForce;
    // Update is called once per frame
    Transform LeftHands;
    void Start()
    {
        LeftHands = GameObject.FindWithTag("LH").transform;
    }
    void Update()
    {
        if (onoff == true)
        {
            zRotOn();
        }
        else if (onoff == false)
        {
            zRotOff();
        }
    }
    public void zRotOn()
    {

        onoff = true;
        addForce = LeftHands.rotation.z * 100;

        if (addForce > 0)
        {
            this.transform.rotation = Quaternion.Euler(-90, 0, this.transform.rotation.z + addForce);
            sumForce += addForce;
        }
    }

    public void zRotOff()
    {
        onoff = false;
    }
}
