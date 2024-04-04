using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabRecognize : MonoBehaviour
{

    public Tools_RatchetWrench ratchet;
    public Tools_CombinationWrench combi;
    public Tools_ClipRemover remover;
    public Tools_HingeWrench hinge;
    XRController controller;

    //����
    public void StartGrab_Ratchet()
    {
        
        if (ratchet.eventStarted == false)
        {
            ratchet.evnStart.Invoke();
        }
        ratchet.SetRotate(true);
    }
    
    public void StartGrab_Combi()
    {
        
        if (combi.eventStarted == false)
        {
            combi.evnStart.Invoke();
        }
        combi.SetRotate(true);
    }

    //������
    public void StartGrab_Remover()
    {
        
        if (remover.eventStarted == false)
        {
            remover.evnStart.Invoke();
        }
        remover.SetPosition(true);

    }

    public void StartGrab_Hinge()
    {

        if (hinge.eventStarted == false)
        {
            hinge.evnStart.Invoke();
        }
        hinge.SetRotate(true);
    }

    /*
    public void StartGrab_Ratchet_T()
    {
        //����T ����
        if (ratchet_T.eventStarted == false)
        {
            ratchet_T.evnStart.Invoke();
        }
        ratchet_T.SetRotate(true);

    }
    public void StartGrab_Ratchet_RS()
    {
        //����T ����
        if (ratchet_RS.eventStarted == false)
        {
            ratchet_RS.evnStart.Invoke();
        }
        ratchet_RS.SetCondition(true);

    }

    public void StartGrab_Wheel()
    {
        // �� ����
        if (wheel.eventStarted == false)
        {
            wheel.evnStart.Invoke();

        }
        wheel.SetCondition(true);

    }
    */

    public void StopGrab_Ratchet()
    {
        //����
        ratchet.SetRotate(false);
    }    
    public void StopGrab_Combi()
    {
        //����
        combi.SetRotate(false);
    }
    
    public void StopGrab_Remover()
    {
        //������
        remover.SetPosition(false);

    }
    public void StopGrab_Hinge()
    {
        //����
        hinge.SetRotate(false);
    }
    /*
    public void StopGrab_Ratchet_T()
    {
        //����T
        ratchet_T.SetRotate(false);

    }
    public void StopGrab_Ratchet_RS()
    {
        //����RS
        ratchet_RS.SetCondition(false);
    }

    public void StopGrab_Wheel()
    {
        //��
        wheel.SetCondition(false);

    }

    public void Activate()
    {
        //XR_Events.ImpulseController(controller, 1, 1);
        Debug.Log("frame_startGrap");
    }

    public void DeActivate()
    {
        //XR_Events.ImpulseController(controller, 0, 0);
        Debug.Log("frame_endtGrap");
    }
    */
}
