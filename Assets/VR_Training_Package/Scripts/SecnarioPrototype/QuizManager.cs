using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

public class QuizManager : MonoBehaviour
{

    public Text text_quiz;
    public List<Text> answerTextList = new List<Text>();
    public List<Button> answerBtnList = new List<Button>();
    public Text text_correct;
    
    public QuizSet quizData;
    int currentQuizID = 0;

    UnityAction<int> buttonAnction;

    QuizData curQuizData;

    void Start()
    {
        SetQuizDataUI(0);
        SetAction();
        SetBtnEvent();
    }
    
    void SetAction()
    {
        buttonAnction = (index) =>
        {
            if(index == curQuizData.correctIndex)
            {
                text_correct.text = "정답";
                Debug.Log("정답");
                currentQuizID++;
                if (currentQuizID >= quizData.nodes.Count)
                {
                    currentQuizID = 0;
                }

                foreach(var t in answerBtnList)
                {
                    t.GetComponent<Image>().color = Color.gray;
                }

                // NEXT SCNE SETTING
                Invoke("SetNextQuizUI", 1.5f);
            }
            else
            {
                text_correct.text = "오답";
                Debug.Log("오답");
            }
        };
    }


    void SetBtnEvent()
    {
        for (int i = 0; i < answerBtnList.Count; i++)
        {
            int index = i;
            answerBtnList[i].onClick.AddListener(()=>buttonAnction.Invoke(index));
        }
    }

    

    void SetQuizDataUI(int id)
    {
        curQuizData = GetQuizDataByID(id).quizData;
        text_quiz.text = curQuizData.question;
        text_correct.text = "결과";

        for (int i = 0; i < curQuizData.answerList.Count; i++)
        {
            answerTextList[i].text = curQuizData.answerList[i];
        }
    }

    void SetNextQuizUI()
    {
        curQuizData = GetQuizDataByID(currentQuizID).quizData;
        text_quiz.text = curQuizData.question;
        text_correct.text = "결과";

        for (int i = 0; i < curQuizData.answerList.Count; i++)
        {
            answerTextList[i].text = curQuizData.answerList[i];
        }

        foreach (var t in answerBtnList)
        {
            t.GetComponent<Image>().color =new Color32(255,224,0,255);
        }
    }
         

    Secnario_QuizNode GetQuizDataByID(int id)
    {
        return quizData.nodes.FirstOrDefault(f => f.index == id);
    }
    
    void NextQuiz()
    {

    }



}
