using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    floatType,
    intType,
    boolType,
}

public class AniInfo : MonoBehaviour
{
    public int id; 
    public AnimationType aniType;
    public string aniName;
    public Animator ani; 
}
