

public class CallBackEventType 
{
    public enum TYPES
    {
        /// <summary> �̼� Ŭ���� ������ ȣ�� </summary>
        OnMissionClear,
        /// <summary> Scenario_ColliderEvent.cs �� ���� �ݶ��̴� Enter �̺�Ʈ�� �߻� ������ ȣ�� </summary>
        OnColliderEnter,
        /// <summary> Scenario_ColliderEvent.cs �� ���� �ݶ��̴� Stay �̺�Ʈ�� �߻� ������ ȣ�� </summary>
        OnColliderStay,
        /// <summary> Scenario_ColliderEvent.cs �� ���� �ݶ��̴� Exit �̺�Ʈ�� �߻� ������ ȣ�� </summary>
        OnColliderExit,
        /// <summary> ���Կ� ��ġ(����) �Ǿ����� ȣ�� </summary>
        OnSlotMatch,
        /// <summary> ���Ͽ� ��ġ(����) �Ǿ����� ȣ�� ( ���� �޷��ִ� ���� ���� ) </summary>
        OnSocketMatch,
        /// <summary> �κ��丮 ���Ͽ� ��ġ(����) �Ǿ����� ȣ��  </summary>
        OnSocketMatchInventory,
        /// <summary> �κ��丮 ���Ͽ� ������� ȣ��  </summary>
        OnSocketTriggerInventory,
        /// <summary> ���Ͽ��� �и� �Ǿ����� ȣ�� </summary>
        OnSocketSeparate,
        /// <summary> PARTS_SLOT ���Ͽ� HOVER �Ǿ����� ȣ�� </summary>
        OnSlotSocketHover,
        /// <summary> ��Ʈ�ѷ� GrabInteractable Select �Ǿ����� ȣ�� </summary>
        OnGrabSelect,
        /// <summary> ��Ʈ�ѷ� GrabInteractable Exit �Ǿ����� ȣ�� </summary>
        OnGrabExit,
        /// <summary> Tool - ��ġ ������ �͵��� Progress �� 100%�� �Ǿ����� ȣ�� </summary>
        OnWrenchComplete,
        /// <summary> Tool - ������ ������ �͵��� Progress �� 100%�� �Ǿ����� ȣ�� </summary>
        OnRemoverComplete,
        /// <summary> Hinge Joint �� ������ ���� �׷����� ������� ȣ�� </summary>
        OnHingeToolGrabEnter,
        /// <summary> Hinge Joint �� ������ ���� �׷��� �������� ȣ�� </summary>
        OnHingeToolGrabExit,
        /// <summary> ��ġ ����ġ On/Off �Ǿ��� �� ȣ�� </summary>
        OnSwitchChange,
        /// <summary> ��ũ��ġ �ϴܺ� �����̼� �Ϸ� �Ǿ����� ȣ�� </summary>
        OnTorqueRotatComplete,
        /// <summary> ���Կ� ��ġ(����) exit �Ǿ����� ȣ�� </summary>
        OnSlotMatchExit,
        /// <summary> ui ��ư hover �Ǿ����� ȣ�� </summary>
        OnUIHover,
        /// <summary> ui ��ư select �Ǿ����� ȣ�� </summary>
        OnUISelect,
        /// <summary> ���Ͽ� ��ġ�ǰ� ���� �Ǿ����� ȣ�� </summary>
        OnSlotMatchSelect,
        ///<summary> ���� ó�� ������ ȣ�� </summary>
        OnDeductionEvent,
        ///<summary> ���� Ÿ�̸� ���� ������ ȣ�� </summary>
        OnStartCourseTimer,
        ///<summary> ���� Ÿ�̸� �Ϸ� ������ ȣ�� </summary>
        OnCompleteCourseTimer,
        ///<summary> ������ ���� ���� Ÿ�̸� ���� ������ ȣ�� </summary>
        OnStartBreakTimer,
        ///<summary> �� ��ü Ÿ�̸� ���� ������ ȣ�� </summary>
        OnStartTotalTimer,
        ///<summary> �ǰ� ó�� ������ ȣ�� </summary>
        OnDQ_Event,
        ///<summary> �� ���� Ȥ�� ���� ��ư �������� ȣ�� </summary>
        OnBtnCourseEvent,
        ///<summary> �򰡽� ������ ������ �߸� ��ġ �Ǿ����� ���� ��ҷ� ȣ�� </summary>
        OnToolMissMatchDeductionEvent,
        ///<summary> ��� ���� ��ȯ�� ȣ�� </summary>
        OnChangeLanguage_EN,
        ///<summary> ��� �ѱ� ��ȯ�� ȣ�� </summary>
        OnChangeLanguage_KR





    }
}
