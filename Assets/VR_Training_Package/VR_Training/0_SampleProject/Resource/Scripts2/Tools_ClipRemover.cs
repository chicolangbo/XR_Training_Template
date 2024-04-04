using UnityEngine;

public class Tools_ClipRemover : ToolBase, ITools
{

    public Transform tr_clip;
    public Transform tr_tool;
    public Transform tr_controller; // vr controller
    public bool isPositionReady = false;
    public float watingTime = 0f;
    public EnumDefinition.Direction direction = EnumDefinition.Direction.Y;

    void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        if (eventStarted && !eventComplete)
            UseClipRemover();
    }

    #region Init Method
    void Init()
    {
        SetEvents();
    }

    void SetEvents()
    {
        evnStart = EventStart;
        evnComplete = EventComplete;
    }
    #endregion

    #region Main Method

    void UseClipRemover()
    {
        if (isPositionReady)
        {
            tr_tool.LookAt(GetToolLookAtPos(direction));

        }
    }

    #endregion

    #region Public Method

    public void SetPosition(bool value)
    {
        isPositionReady = value;
    }
    #endregion

    #region Event Method
    public void EventStart()
    {
        eventStarted = true;
        Debug.Log("ClipRemover Event Start");
    }

    public void EventComplete()
    {
        eventComplete = true;
        Debug.Log("ClipRemover Event Complete ");
    }
    #endregion

    Vector3 GetToolLookAtPos(EnumDefinition.Direction direction)
    {
        switch (direction)
        {
            case EnumDefinition.Direction.X: return new Vector3(tr_tool.position.x, tr_controller.position.y, tr_controller.position.z);
            case EnumDefinition.Direction.Y: return new Vector3(tr_controller.position.x, tr_tool.position.y, tr_controller.position.z);
            case EnumDefinition.Direction.Z: return new Vector3(tr_controller.position.x, tr_controller.position.y, tr_tool.position.z);
            default: return Vector3.zero;
        }
    }

}