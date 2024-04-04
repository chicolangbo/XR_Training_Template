using UnityEngine;
using UnityEngine.UI;
public class SceneControl : MonoBehaviour
{
    public Text text;
    private float countTime = 0f;
    private int num = 0;

    string[] txt = new string[5]; 
        
    /*= { "인벤티스에 오신것을 환영합니다.",
                     "아이디는 QWE 입니다. 입력해주세요.",
                     "비밀번호는 123입니다. 입력해주세요.",
                     "로그인 버튼을 누르면 차고로 이동합니다." };
    */
    void Start()
    {

    }
    void Update()
    {
        TransformText();
    }


    //단계별 진행을 위해 조건 값에 따른 텍스트 변환
    public void TransformText()
    {

        switch (num)
        {
            case 0:
                //시작 후 5초 간 핸드 사용 불가
                StepZero();
                break;
            case 1:
                //핸드 사용 가능 안내 UI 출력
                StepOne();
                break;
            case 2:
                //타어이어 옮긴 후
                StepFinal();
                break;

            default:
                Debug.Log("스위치문 작동 중 오류 발생");
                break;
        }
    }

    #region 단계별 스탭
    void StepZero()
    {
        if (countTime <= 5f)
        {
            countTime += Time.deltaTime;
            if (countTime >= 5f)
            {
                countTime = 0f;
                num = 1;
            }
        }
    }

    void StepOne()
    {
        text.text = txt[0];
        if (countTime <= 3f)
        {
            countTime += Time.deltaTime;
            if (countTime >= 3f)
            {
                countTime = 0f;
                num = 2;
            }
        }
    }

    void StepFinal()
    {
        text.text = txt[3];
    }
    #endregion
}
