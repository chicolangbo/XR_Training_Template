using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftGrabRecognize : MonoBehaviour
{
    public Tools_RatchetWrench torque;

    public void StartGrab_Ratchet()
    {
        // ∂Û√¬ Ω√¿€
        if (torque.eventStarted == false)
        {
            torque.evnStart.Invoke();
        }
        torque.SetRotate(true);
    }
     
    public void StopGrab_Ratchet()
    {
        //∂Û√¬ ¡æ∑·
        torque.SetRotate(false);
    }

}
