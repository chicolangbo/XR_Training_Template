using System.Collections;
using System.Collections.Generic;

public enum NodeType
{
    Float,
    Add,
    Secnario,
    Quiz,
    TypeSelect,
    Mission,
    Pattern
    
}

public enum SecnarioType
{
    A,
    B,
    C,
    D,
    E
}

// ���� ���� , ���� ���� , �� ����
public enum ConnectType
{
    Start,
    Middle,
    End
}

public enum PlayType
{
    Playing,
    Complete
}

public enum QuzeAnswerType
{
    Correct,
    Incorrect
}

public enum QuizType
{
    MultipleChoice,
    ShortAnswer
}

public enum TypeSelectType
{
    Type_A,
    Type_B,
    Type_C
}