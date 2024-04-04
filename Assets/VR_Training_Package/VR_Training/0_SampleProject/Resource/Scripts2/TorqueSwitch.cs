using UnityEngine;

public class TorqueSwitch : MonoBehaviour
{
    public bool isTorqueValue = false;
    Quaternion originalRot, addRot, mulRot, newRotation;


    public void OnOffTorqueSwitch()
    {
        isTorqueValue = isTorqueValue == false ? true : false;

        if (isTorqueValue)
        {
            originalRot = this.transform.rotation;
            addRot = Quaternion.Euler(new Vector3(0f, 60f, 0f));
            newRotation = originalRot * addRot;
            this.transform.rotation = newRotation;
        }
        else if (!isTorqueValue)
        {
            mulRot = Quaternion.Euler(new Vector3(0f, -60f, 0f));
            this.transform.rotation = this.transform.rotation * mulRot;
        }
    }
}
