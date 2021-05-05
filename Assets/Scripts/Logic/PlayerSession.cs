using System;
using UnityEngine;
using Random = System.Random;

public class PlayerSession : MonoBehaviour
{
    public TextAsset textq;
    public int currentQuestionNum;
    private string[] answers;
    private bool[] hints = {true, true, true, true};
    private const int Poolsize = 15;
    
    [HideInInspector] public QuestionViewController qvc;
    private AudioManager audioM;
    private Question currentQuestion;
    private Player p;
    private Question[] questions;
    
    public event EventHandler<AnswerArgs> OnAnswer;
    public event EventHandler<HintArgs> OnHint;
    public event EventHandler<ExitArgs> OnExit;
    public event EventHandler<WarningArgs> OnWarning;
    public event EventHandler<NewQuestionArgs> OnNextQuestionData;
    private void Awake()
    {
        Debug.Log("Started!");
    }

    private void Start()
    {
        qvc = GetComponent<QuestionViewController>();
        p = new Player(new TempGetter().LoadPlayerData());

        if (QuestionBank.Questions == null || QuestionBank.Questions.Count < 1)
            QuestionBank.Questions = new TempGetter(textq.text).LoadQuestionBank();

        questions = QuestionBank.GetQuestionPool(out answers, p);

        currentQuestionNum = -1;
        OnNextQuestion();
        audioM = AudioManager.Instance;
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("App Paused");
        if (pause) SaveData();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        SaveData();
    }

    /*
     * It's button func in PrizesPanel
     */
    public void OnNextQuestionClick()
    {
        audioM.Click();
        OnNextQuestion();
    }

    private void OnNextQuestion()
    {
        currentQuestionNum++;
        currentQuestion = questions[currentQuestionNum];

        QuestionsHandler.Shuffle(new Random(), currentQuestion.answers);

        OnNextQuestionData?.Invoke(this,
            new NewQuestionArgs {q = questions[currentQuestionNum], qNum = currentQuestionNum});
    }

    /*
     * It's button func on AnswerButton
     */
    public void OnAnswerClicked(int choice)
    {
        var last = currentQuestionNum == Poolsize - 1;
        var correct = currentQuestion.answers[choice] == answers[currentQuestionNum];

        audioM.Click();
        p.AddAnsweredQuestion(currentQuestion.id, correct);
        if (correct && last)
            PlayerWin();
        else if (!correct) PlayerLose();
        OnAnswer?.Invoke(this, new AnswerArgs
        {
            correct = correct,
            choice = choice,
            qNum = currentQuestionNum,
            correctChoice = GetCorrectChoice()
        });
    }
    public void OnHintClosed()
    {
        audioM.Click();
        
        OnHint?.Invoke(this, new HintArgs
        {
            hint = MyHint.HClose,
            correctChoice = GetCorrectChoice()
        } );
        
    }
    public void OnHintClicked(int hintID)
    {
        audioM.Click();
        Debug.Log(hintID);
        if (!hints[hintID]) return; 
        hints[hintID] = false;
        MyHint hint;
        switch (hintID)
        {
            case 0:
                hint = MyHint.H5050;
                break;
            case 1:
                hint = MyHint.HStats;
                break;
            case 2:
                hint = MyHint.HComp;
                break;
            default:
                hint = MyHint.HSwitch;
                break;
        }
        OnHint?.Invoke(this, new HintArgs
        {
            hint = hint,
            correctChoice = GetCorrectChoice()
        } );
    }

    /*
     * It's button func on WarningButton
     */
    public void OnWarningClicked()
    {
        audioM.Click();
        OnWarning?.Invoke(this, new WarningArgs {action = MyAction.Open});
    }

    /*
     * It's button func on WarningButton
     */
    public void OnWarningConfirmClicked()
    {
        audioM.Click();
        // SEND WARNING TO US
        Debug.LogWarning("Warning on the question!");
        OnWarning?.Invoke(this, new WarningArgs {action = MyAction.Confirm});
    }

    /*
     * It's button func on WarningButton
     */
    public void OnWarningBackClicked()
    {
        audioM.Click();
        OnWarning?.Invoke(this, new WarningArgs {action = MyAction.Close});
    }

    /*
     * It's button func on ExitButton
     */
    public void OnExitClicked()
    {
        audioM.Click();
        OnExit?.Invoke(this, new ExitArgs {action = MyAction.Open, qNum = currentQuestionNum});
    }

    /*
     * It's button func on ExitConfirmButton
     */
    public void OnExitConfirmedClicked()
    {
        audioM.Click();
        PlayerExit();
        OnExit?.Invoke(this, new ExitArgs {action = MyAction.Confirm, qNum = currentQuestionNum});
    }

    public void OnExitBackClicked()
    {
        audioM.Click();
        OnExit?.Invoke(this, new ExitArgs {action = MyAction.Close});
    }

    private void PlayerWin()
    {
        p.Win();
        SaveData();
    }

    private void PlayerLose()
    {
        p.Lose(currentQuestionNum);
        SaveData();
    }

    private void PlayerExit()
    {
        p.Exit(currentQuestionNum);
        SaveData();
    }

    public void SaveData()
    {
        if (p != null) new TempGetter().SavePlayerData(p.Data());
    }
    
    private int GetCorrectChoice()
    {
        for (var i = 0; i < 4; i++)
            if (currentQuestion.answers[i].Equals(answers[currentQuestionNum]))
                return i;
        return 0;
    }
}