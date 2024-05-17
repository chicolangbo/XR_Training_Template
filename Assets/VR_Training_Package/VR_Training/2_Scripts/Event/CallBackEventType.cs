

public class CallBackEventType 
{
    public enum TYPES
    {
        /// <summary> 미션 클리어 됐을때 호출 </summary>
        OnMissionClear,
        /// <summary> Scenario_ColliderEvent.cs 를 통해 콜라이더 Enter 이벤트가 발생 했을때 호출 </summary>
        OnColliderEnter,
        /// <summary> Scenario_ColliderEvent.cs 를 통해 콜라이더 Stay 이벤트가 발생 했을때 호출 </summary>
        OnColliderStay,
        /// <summary> Scenario_ColliderEvent.cs 를 통해 콜라이더 Exit 이벤트가 발생 했을때 호출 </summary>
        OnColliderExit,
        /// <summary> 슬롯에 매치(결합) 되었을때 호출 </summary>
        OnSlotMatch,
        /// <summary> 소켓에 매치(결합) 되었을때 호출 ( 툴에 달려있는 소켓 포함 ) </summary>
        OnSocketMatch,
        /// <summary> 인벤토리 소켓에 매치(결합) 되었을때 호출  </summary>
        OnSocketMatchInventory,
        /// <summary> 인벤토리 소켓에 닿았을때 호출  </summary>
        OnSocketTriggerInventory,
        /// <summary> 소켓에서 분리 되었을때 호출 </summary>
        OnSocketSeparate,
        /// <summary> PARTS_SLOT 소켓에 HOVER 되었을때 호출 </summary>
        OnSlotSocketHover,
        /// <summary> 컨트롤러 GrabInteractable Select 되었을대 호출 </summary>
        OnGrabSelect,
        /// <summary> 컨트롤러 GrabInteractable Exit 되었을대 호출 </summary>
        OnGrabExit,
        /// <summary> Tool - 렌치 종류의 것들의 Progress 가 100%가 되었을때 호출 </summary>
        OnWrenchComplete,
        /// <summary> Tool - 리무버 종류의 것들의 Progress 가 100%가 되었을때 호출 </summary>
        OnRemoverComplete,
        /// <summary> Hinge Joint 로 구성된 툴을 그랩으로 잡았을때 호출 </summary>
        OnHingeToolGrabEnter,
        /// <summary> Hinge Joint 로 구성된 툴을 그랩을 놓았을때 호출 </summary>
        OnHingeToolGrabExit,
        /// <summary> 렌치 스위치 On/Off 되었을 때 호출 </summary>
        OnSwitchChange,
        /// <summary> 토크렌치 하단부 로테이션 완료 되었을때 호출 </summary>
        OnTorqueRotatComplete,
        /// <summary> 슬롯에 매치(결합) exit 되었을때 호출 </summary>
        OnSlotMatchExit,
        /// <summary> ui 버튼 hover 되었을때 호출 </summary>
        OnUIHover,
        /// <summary> ui 버튼 select 되었을때 호출 </summary>
        OnUISelect,
        /// <summary> 소켓에 매치되고 선택 되었을때 호출 </summary>
        OnSlotMatchSelect,
        ///<summary> 감점 처리 됐을때 호출 </summary>
        OnDeductionEvent,
        ///<summary> 과정 타이머 시작 됐을때 호출 </summary>
        OnStartCourseTimer,
        ///<summary> 과정 타이머 완료 됐을때 호출 </summary>
        OnCompleteCourseTimer,
        ///<summary> 과정과 과정 사이 타이머 시작 됐을때 호출 </summary>
        OnStartBreakTimer,
        ///<summary> 평가 전체 타이머 시작 됐을때 호출 </summary>
        OnStartTotalTimer,
        ///<summary> 실격 처리 됐을때 호출 </summary>
        OnDQ_Event,
        ///<summary> 평가 시작 혹은 종료 버튼 눌렀을때 호출 </summary>
        OnBtnCourseEvent,
        ///<summary> 평가시 공구와 슬롯이 잘못 매치 되었을때 감점 요소로 호출 </summary>
        OnToolMissMatchDeductionEvent,
        ///<summary> 언어 영문 변환시 호출 </summary>
        OnChangeLanguage_EN,
        ///<summary> 언어 한글 변환시 호출 </summary>
        OnChangeLanguage_KR





    }
}
