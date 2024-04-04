using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayRecognize : MonoBehaviour
{
    public Engines_AEM tools;
    public void StartGrab()
    {
        //AEM
        if (tools.eventStarted == false)
        {
            tools.evnStart.Invoke();
        }
        tools.CompleteGage(true);
    }

    public void StopGrab()
    {
        //AEM
        tools.CompleteGage(false);
    }

    public void Activate()
    {
        //XR_Events.ImpulseController(controller, 1, 1);
        Debug.Log("frame_startRay");
    }

    public void DeActivate()
    {
        //XR_Events.ImpulseController(controller, 0, 0);
        Debug.Log("frame_endRay");
    }

}
