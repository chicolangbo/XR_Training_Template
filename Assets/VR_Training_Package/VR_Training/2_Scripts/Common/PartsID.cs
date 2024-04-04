using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsID : MonoBehaviour
{
    public EnumDefinition.PartsType partType;
    public int id;
    public string partName;
    public Highlighter highlighter;
    public GameObject ghostObject;
    public Collider slotCollider;
    public Collider myCollider;
    public Animator animator; //다음동작 에니메이터 
    public BoltInfo boltInfo;
         

    private void Start()
    {
        if (TryGetComponent(out Highlighter _highlighter))
            highlighter = _highlighter;

        if (TryGetComponent(out Collider _myCollider))
            myCollider = _myCollider;

        if (TryGetComponent(out BoltInfo _boltInfo))
            boltInfo = _boltInfo;


        if (partType.ToString().Contains("SLOT") && slotCollider == null)
        {
            GetSlotCollider();
            GetGhostObject();
        }
    }

    public void SetPartId(int _id)
    {
        id = _id;
    }

    public void MyColliderEnable(bool value)
    {
        if (myCollider != null)
        {
            myCollider.enabled = value;
        }
    }

    void GetGhostObject()
    {
        var childObjects = transform.GetComponentsInChildren<Transform>();
        foreach(var element in childObjects)
        {
            if (element.gameObject.name.Contains("Ghost") || element.gameObject.name.Contains("ghost") || element.gameObject.name.Contains("GHOST"))
            {
                ghostObject = element.gameObject;
                return;
            }
        }
        PrintWarning("Ghost");
    }
    void GetSlotCollider()
    {
        if (TryGetComponent<Collider>(out Collider col))
        {
            slotCollider = col;
        }
        else
        {
            PrintError("Collider");
        }
    }
        
    public Highlighter GetHighligther()
    {
        if (highlighter != null)
            return highlighter;
        else
        {
            PrintError("Highlighter");
            return null;
        }
    }
        
    public void GhostObjectOn()
    {
        if(ghostObject != null)
            ghostObject.SetActive(true);
    }

    public void GhostObjectOff()
    {
        if (ghostObject != null)
            ghostObject.SetActive(false);
    }

    public void SlotColliderDisable()
    {
        if (slotCollider != null)
            slotCollider.enabled = false;
    }

    public void SlotColliderEnable()
    {
        if (slotCollider != null)
            slotCollider.enabled = true;
    }

    void PrintError( string type)
    {
        Debug.LogError($"{partType} _ {id} _ {partName} _ {gameObject.name}_ 오브젝트는 {type} 가 없습니다. ");
    }
    void PrintWarning(string type)
    {
        Debug.LogWarning($"{partType} _ {id} _ {partName} _ {gameObject.name}_ 오브젝트는 {type} 가 없습니다. ");
    }

}
