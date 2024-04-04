using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltInfo : MonoBehaviour
{
    public EnumDefinition.BoltDirType dirType;
    public EnumDefinition.ToolUpDownType toolUpDonwType;
    public EnumDefinition.ToolLeftRightType toolLeftRightType;
    public EnumDefinition.BoltProgressType progressType;
    public float combinationAngle;
    public float toolZ_Angle;
    public bool angleLimit;
    public float min_angle;
    public float max_angle;
    public bool reverse_tool;
         

    void Start()
    {
        
    }


}
