[System.Serializable]
public class PatternData 
{
    public string ID;
    // 현재 과정 타이틀
    public string Course_Title;
    // 점프 인덱스 아이다
    public int Jump_ID;
    // 평가 항목 아이디
    public int Evaluation_ID;
    /// <summary> 정비구역 </summary>
    public string MNTN_AREA; 
    public string PATTERN_TYPE;
    public string PTRN_CLSFC_1;
    public string PTRN_CLSFC_2;
    public string PTRN_CLSFC_3;
    public string HIGHL;
    
    // 하이라이트 비활성화 여부
    public string HIGHL_OFF;
    // 나레이션 비활성화 여부
    public string NARR_OFF;
    public int    NARR_ID;
    /// <summary> 나레이션 텍스트 </summary>
    public string NARR;
    public string TITLE;
    /// <summary> 과정 </summary>
    public string CRS_SCRIPT;
    /// <summary> 주의사항 </summary>
    public string PRCTN;
    /// <summary> 부가설명 </summary>
    public string ADT_EXP_SCRIPT;
    public string SCRIPT;
}
