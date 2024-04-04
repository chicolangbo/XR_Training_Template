
public class EnumDefinition 
{
    public enum LANGUAGE_TYPE
    {
        KR,
        EN
    }

    public enum Direction
    {
        X, Y, Z
    }

    public enum ControllerType
    {
        LeftController,
        RightController
    }

    public enum WrenchType
    {
        Ratchet,
        Torque,
        Hinge,
        Combination8mm,
        Combination17mm,
        Combination19mm,
        NosePlier,
        ClipRemover, 
        Combination
    }

    /// <summary> �� ���� Ÿ�� </summary>
    public enum Deduction_Type
    {
        NONE = 0,
        /// <summary> ���� ���� ( ������ �߸� ���� ������) </summary>
        TOOL = 3,

        /// <summary> ���� ���� ( ������ �߸��� ��Ʈ�� ü�� ������ ) </summary>
        USE_TOOL = 4,

        /// <summary> ���� �۾� �̿Ϸ� ���� ( ���� ����Ʈ �� �ٿ���� ���� �ʾ����� ) </summary>
        PRIORWORK = 5,
        /// <summary> ���� ���� ( ������ ������ ������ ���� ���� ���� �ʾ����� ) </summary>
        ORDER = 5,
        /// <summary> ���� Ÿ�̸� ���� ( ���� �ð� 70% �ʰ��� ) </summary>
        COURSE_TIMER = 2,
    }

    /// <summary> �ǰ� ó�� Ÿ�� </summary>
    public enum DQ_Type
    {
        /// <summary> ���� Ÿ�̸ӷ� �ʰ��� ���� �ǰ� </summary>
        TIMER_COURSE,
        /// <summary> ������ ���� ���� �ʰ��� ���� �ǰ� </summary>
        TIMER_BREAK,
        /// <summary> ��ü �ð� �ʰ��� ���� �ǰ� </summary>
        TIMER_TOTAL,
        /// <summary> ��ü ������ ���� �ǰ� ( 60�� �̸� ) </summary>
        TOTAL_SCORE,
        /// <summary> ������ �̼� ��ü �Ϸ� ���� �ʾ����� </summary>
        NOT_ALL_MISSION_CLEAR,
    }

    /// <summary> ���� �� ���� ��ư�� Ÿ�� </summary>
    public enum CourseBtnEventType
    {
        START, // ���� ����
        END    // ���� ����
    }

    public enum HoodState
    {
        CLOSE,
        OPEN
    }

    public enum LiftAnimState
    {
        NOT_COMPLETE,
        UP_COMPLETE,
        DOWN_COMPLETE
    }


    public enum TimerCalcType
    {
        START,
        END
    }
    


    public enum PlayModeType
    {
        NONE,
        /// <summary> ���丮�� </summary>
        TUTORIAL,
        /// <summary> �ǽ� </summary>
        TRAINING,
        /// <summary> �� </summary>
        EVALUATION
    }

    public enum CourseType
    {
        NONE,
        SUSPENSION_LOWER_ARM,
        SUSPENSION_STRUT_ASSEMBLY,
        SUSPENSION_INSPECTION,
        SUSPENSION_WHEEL_ALIGNMENT,
        STARTER_BATTERY,
        STARTER_DETACH_ATTACH,
        STARTER_DECOMPOSE_ASSEMBLY,
        //ģȯ���ڵ��� ������
        STRUCTURE_BLOCK,
        STRUCTURE_CONNECTION,
        STRUCTURE_BATTERY_OUT,
        STRUCTURE_BATTERY_IN,
        STRUCTURE_OPERATION_OUT,
        STRUCTURE_OPERATION_IN,
        //1ȸ���������Ÿ�����
        DISTANCE_PREPARE,
        DISTANCE_CONNECTION,
        DISTANCE_EXAM_AUTH,
        DISTANCE_EXAM_PRELIMINARY,
        DISTANCE_EXAM_COSTDOWN,
        DISTANCE_EXAM_DRIVE,

        //����������
        NOISE_CERTIFICATION_TEST,
        NOISE_CERTIFICATION_TEST_INFO,
        NOISE_EXAM_CERTIFICATION_TEST,

        //������
        THERMAL_RUNWAY_SET,
        THERMAL_RUNWAY_EXAM,
        THERMAL_RUNWAY_RESULT,
    }

    public enum MisiionDataType
    {
        TUTORIAL_SUSPENTION,
        TUTORIAL_STATER,
        EVALUTION_SUSPENTION,
        EVALUTION_STATER_BATTERY,
        EVALUTION_STATER_ELECT_MOTOR,
        TRAINING_SUSPENSION_LOWER_ARM,
        TRAINING_SUSPENSION_STRUT_ASSEMBLY,
        TRAINING_SUSPENSION_INSPECTION,
        TRAINING_SUSPENSION_WHEEL_ALIGNMENT,
        TRAINING_STARTER_BATTERY,
        TRAINING_STARTER_DETACH_ATTACH,
        TRAINING_STARTER_DECOMPOSE_ASSEMBLY,

        //ģȯ���ڵ��� ������
        TRAINING_STRUCTURE_BLOCK,
        TRAINING_STRUCTURE_CONNECTION,
        TRAINING_STRUCTURE_BATTERY_OUT,
        TRAINING_STRUCTURE_BATTERY_IN,
        TRAINING_STRUCTURE_OPERATION_OUT,
        TRAINING_STRUCTURE_OPERATION_IN,
        //1ȸ���������Ÿ�����
        TRAINING_DISTANCE_PREPARE,
        TRAINING_DISTANCE_CONNECTION,
        TRAINING_DISTANCE_EXAM_AUTH,
        TRAINING_DISTANCE_EXAM_PRELIMINARY,
        TRAINING_DISTANCE_EXAM_COSTDOWN,
        TRAINING_DISTANCE_EXAM_DRIVE,

        TRAINING_NOISE_CERTIFICATION_TEST,
        TRAINING_NOISE_CERTIFICATION_TEST_INFO,
        TRAINING_NOISE_EXAM_CERTIFICATION_TEST,

        TRAINING_THERMAL_RUNWAY_SET,
        TRAINING_THERMAL_RUNWAY_EXAM,
        TRAINING_THERMAL_RUNWAY_RESULT,
    }

    public enum EVALUATION_TYPE
    {
        NONE,
        SUSPENSION,
        STATER_BATTERY,
        STATER_ELECTRIC_MORTOR
    }
    
    public enum STATER_TYPE
    {
        NONE,
        BATTERY,
        ELECTRIC_MORTOR
    }

    public enum VibrationBtnType
    {
        Trigger,
        Grap
    }

    public enum ProjectType
    {
        /// <summary> ������ġ </summary>
        Suspension,
        /// <summary> �õ���ġ </summary>
        Starter
    }

    public enum PartsType
    {
        SOCKET,
        TOOL,
        INTERACTION,
        TOOLSTAND_SLOT,
        TOOLTABLE_SLOT,
        TOOLBOX_SLOT,
        TOOLBOX_AREA,
        EQUIP_AREA,
        PART_AREA,
        INTERACTION_BUTTON,
        GROUP_PARTS,
        TOOLBOX_GHOST,
        MOVING_INTERACTION,
        NONE,
        TOOL_GHOST,
        TOOLTABLE_GHOST,
        TOOLSTAND_GHOST,
        USING_TOOL,
        PARTS,
        INVENTORY,
        PARTS_SLOT,
        TOOL_SOCKET,
        PARTS_SLOT_GHOST_TABLE,
        PARTS_SLOT_GHOST_SHOCK, 
        SWITCH,
        PART_GHOST_AREA,
        OBJECT,
        LINE, 
        BOX,
        WHEEL_ALIGNMENT,
        WHEEL_ALIGNMENT_SLOT_GHOST,
        ICON,
        WARP
    }

    public enum PatternType
    {
        P_001,
        P_002,
        P_003,
        P_004,
        P_005,
        P_006,
        P_007,
        P_008,
        P_009,
        P_010,
        P_011,
        P_012,
        P_013,
        P_014,
        P_015,
        P_016,
        P_017,
        P_018,
        P_019,
        P_020,
        P_021,
        P_022,
        P_023,
        P_024,
        P_025,
        P_026,
        P_027,
        P_028,
        P_029,
        P_030,
        P_031,
        P_032,
        P_033,
        P_034,
        P_035,
        P_036,
        P_037,
        P_038,
        P_039,
        P_040,
        P_041,
        P_042,
        P_043,
        P_044,
        P_045,
        P_046,
        P_047,
        P_048,
        P_049,
        P_050,
        P_051,
        P_052,
        P_053,
        P_054,
        P_055,
        P_056,
        P_057,
        P_058,
        P_059,
        P_060,
        P_061,
        P_062,
        P_063,
        P_064,
        P_065,
        P_066,
        P_067,
        P_068,
        P_069,
        P_070,
        P_071,
        P_072,
        P_073,
        P_074,
        P_075,
        P_076,
        P_077,
        P_078,
        P_079,
        P_080,
        P_081,
        P_082,
        P_083,
        P_084,
        P_085,
        P_086,
        P_087,
        P_088,
        P_089,
        P_090,
        P_091,
        P_092,
        P_093,
        P_094,
        P_095,
        P_096,
        P_097,
        P_098,
        P_099,
        P_100,
        P_101,
        P_102,
        P_103,
        P_104,
        P_105,
        P_106,
        P_107,
        P_108,
        P_109,
        P_110,
    }

    public enum TagType
    {
        Player
    }
    
    public enum AngleDirType
    {
        x, y, z
    }
    public enum ToolDirType
    {
        forward,
        backward
    }

    /// <summary> Angle Tool�� ���� ���� ���� Ÿ�� ( x or y )  </summary>
    public enum BoltDirType
    {
        /// <summary> x </summary>
        Vertical,
        /// <summary> y </summary>
        Horizontal
    }

    /// <summary> BoltDirType�� Horizontal direction ������ �ش��� </summary>
    public enum ToolUpDownType // Horizontal side
    {
        Up,
        Down
    }
    /// <summary> BoltDirType�� Vertical direction ������ �ش��� </summary>
    public enum ToolLeftRightType // vertical side
    {
        Left,
        Right
    }




    /// <summary> �� ���ս� ���� ���α׷��� ǥ�� ����  </summary>
    public enum BoltProgressType
    {
        Enable,
        Disable
    }

    /// <summary>
    /// ui�������� �ش�ui���� 
    /// </summary>
    public enum UIClickType
    {
        None, 
        Maker,
        CarKind, 
        RunOut,
        Event,
        SideEvent
    }

    public enum SOUND_EFFECT
    {
        //����056����
        //RACHET, 
        //LIFT,
        //RENCH,
        //CLIP,
        //LOWERARM,
        //TORCH,
        //STRUT, 
        //BATTERY_CHARGE_ALARM,
        //NUM_STAT

        battery_charge,
        battery_charge_finish,
        car_lift,
        close_the_driver_door,
        drop_the_tool,
        hook_lever_operation,
        ignition,
        put_on_work_table,
        ratchet_wrench,
        spring_compressor_01,
        spring_compressor_02,
        spring_compressor_03,
        spring_noise,
        torque_wrench,
        vehicle_starter,
        wheel_alignment_head_fixed,
        ioniq6_start,
        chain,
        finish,
        air_pump,
    }


}
