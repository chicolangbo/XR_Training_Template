using UnityEngine;


public abstract class UsingToolBase : MonoBehaviour
{
    public bool isGrip = false;

    public abstract void GrabOn();
    public abstract void GrabOff();

}
