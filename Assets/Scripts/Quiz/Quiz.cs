using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [System.Serializable]
    struct Question
    {
        [SerializeField]
        public string text;
        [SerializeField]
        public List<string> answers;
        [SerializeField]
        public string solution;
    }

    [SerializeField]
    List<Question> Questions;

    [SerializeField]
    AudioClip correctSFX;
    
    [SerializeField]
    AudioClip incorrectSFX;

    Transform Answers;
    bool answerProcessing = false;
    int currentQuestion;
    int correct = 0;

    static string[] InsideOutQuiz =
    {
        "InsideOut - Quiz",
        "InsideOut - Kviz"
    };

    // Start is called before the first frame update
    void Start()
    {
        Answers = transform.Find("Canvas/Question/Answers");
        transform.Find("Canvas/Title").GetComponent<TMP_Text>().text = Localization.Loc.loc(InsideOutQuiz);
        for(int i = 0; i < Answers.childCount; i++)
        {
            int k = i;
            Answers.GetChild(i).GetComponent<Button>().onClick.AddListener(() => StartCoroutine(onAnswerClicked(k)));
        }

        currentQuestion = 0;
        setQuestion();
    }

    void setQuestion()
    {
        setQuestionTitle(Questions[currentQuestion].text + string.Format(" ({0}/{1})", currentQuestion+1, Questions.Count));
        setPossibleAnswers(Questions[currentQuestion].answers);
    }

    void setQuestionTitle(string title)
    {
        transform.Find("Canvas/Question/QuestTitle").GetComponent<TMP_Text>().text = title;
    }

    void setPossibleAnswers(List<string> answers)
    {
        if(answers.Count < 2 || answers.Count > 4) throw new System.Exception("Invalid amount of possible answers! Range [2,4]");
        for(int i = 0; i < 4; i++)
        {
            var obj = Answers.GetChild(i);
            if(i <= answers.Count)
            {
                obj.GetComponent<QuizButton>().setAnswer(answers[i]);
            }
            else
            {
                obj.gameObject.SetActive(false);
            }
        }
    }

    void showResult(int answer)
    {
        for(int i = 0; i < Questions[currentQuestion].answers.Count; i++)
        {
            var q = Answers.GetChild(i).GetComponent<QuizButton>();
            q.setBlockEvents(true);
            if(Questions[currentQuestion].answers[i] == Questions[currentQuestion].solution) q.mark(true);
            else if(i == answer) q.mark(false);
        }
    }

    static string[] QuizFinished =
    {
        "Quiz Finished!\nScore: ",
        "Kviz završen!\nRezultat: "
    };

    void showFinalResult()
    {
        transform.Find("Canvas/Question/QuestTitle").gameObject.SetActive(false);
        foreach (Transform tf in Answers) tf.gameObject.SetActive(false);

        var res = transform.Find("Canvas/Result");
        res.gameObject.SetActive(true);
        res.GetComponent<TMP_Text>().text = Localization.Loc.loc(QuizFinished) + correct;
    }

    IEnumerator onAnswerClicked(int answer)
    {
        if(!answerProcessing)
        {
            answerProcessing = true;

            if(Questions[currentQuestion].solution == Questions[currentQuestion].answers[answer])
            {
                correct++;
                AudioSource.PlayClipAtPoint(correctSFX, transform.position);
            }
            else
            {
                AudioSource.PlayClipAtPoint(incorrectSFX, transform.position);
            }

            showResult(answer);

            yield return new WaitForSeconds(3.0f);

            currentQuestion++;

            if(currentQuestion == Questions.Count) showFinalResult();
            else setQuestion();

            answerProcessing = false;
        } 
    }
}
