using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBase : MonoBehaviour
{
    public delegate void Events();
    public Events evnStart;
    public Events evnComplete;
    public Events evnPause;
    public Events evnStop;

    public bool eventComplete = false;
    public bool eventStarted = false;

    public int index;

    
}
