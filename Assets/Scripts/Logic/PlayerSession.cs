using System;
using UnityEngine;
using Random = System.Random;

public class PlayerSession : MonoBehaviour
{
    private const int PoolSize = 15;
    public TextAsset textQ;
    public int currentQuestionNum;
    private string[] answers;
    private readonly bool[] hints = {true, true, true, true};
    
    private Question[] questions;
    private Question currentQuestion;
    private Player p;
    private AudioManager audioM;
    private readonly Random rnd = new Random();
    
    public event EventHandler<AnswerArgs> OnAnswer;
    public event EventHandler<HintArgs> OnHint;
    public event EventHandler<ExitArgs> OnExit;
    public event EventHandler<WarningArgs> OnWarning;
    public event EventHandler<NewQuestionArgs> OnNextQuestionData;
    private void Awake()
    {
        Debug.Log("Started!");
        // set player
        p = new Player(new TempGetter().LoadPlayerData());
        
        // additional load
        if (QuestionBank.Questions == null || QuestionBank.Questions.Count < 1)
            QuestionBank.Questions = new TempGetter(textQ.text).LoadQuestionBank();

        // get questions for session
        questions = QuestionBank.GetQuestionPool(out answers, p);
        
        currentQuestionNum = -1; // set it to -1, cause in OnNextQuestion it will +1
        audioM = AudioManager.Instance;
    }

    private void Start()
    {
        // get next question (first one)
        OnNextQuestion();
    }
    
    #region QuestionAnswer
    
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

        QuestionsHandler.Shuffle(rnd, currentQuestion.answers);

        OnNextQuestionData?.Invoke(this,
            new NewQuestionArgs {q = questions[currentQuestionNum], qNum = currentQuestionNum});
    }

    /*
     * It's button func on AnswerButton
     */
    public void OnAnswerClicked(int choice)
    {
        audioM.Click();
        
        var isLast = currentQuestionNum == PoolSize - 1;
        var isCorrect = currentQuestion.answers[choice] == answers[currentQuestionNum];

        p.AddAnsweredQuestion(currentQuestion.id, isCorrect);
        if (isCorrect && isLast)
            PlayerWin();
        else if (!isCorrect) PlayerLose();
        OnAnswer?.Invoke(this, new AnswerArgs
        {
            correct = isCorrect,
            choice = choice,
            qNum = currentQuestionNum,
            correctChoice = GetCorrectChoice()
        });
    }
    
    #endregion
    
    #region HintButtons
    
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

    #endregion
    
    #region WarningButton

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

    #endregion

    #region ExitButton

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
        audioM.Click(); // hello
        OnExit?.Invoke(this, new ExitArgs {action = MyAction.Close});
    }

    #endregion

    #region GameEndFunctions

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

    #endregion
    
    private int GetCorrectChoice()
    {
        for (var i = 0; i < 4; i++)
            if (currentQuestion.answers[i].Equals(answers[currentQuestionNum]))
                return i;
        return 0;
    }
    
    private void SaveData()
    {
        if (p != null) new TempGetter().SavePlayerData(p.GetData());
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

}