#region

using System;
using UnityEngine;
using Random = System.Random;

#endregion

public class PlayerSession : MonoBehaviour
{
    private const int PoolSize = 15;
    
    private readonly Random rnd = new Random();
    private readonly bool[] sessionHints = {true, true, true, true};
    private AudioManager audioM;
    private QuestionWebGetter qwg;

    private Question currentQuestion;
    private int currentQuestionNum;

    private int lives = 2;
    private Player p;
    private QuestionBank questionBank;
    private QuestionPool pool;

    private void Awake()
    {
        SetSingletons();

        SetEventSubscribers();

        SetSessionQuestions();

        currentQuestionNum = -1; // set it to -1, cause in OnNextQuestion it will +1
    }
    
    private void Start()
    {
        // get next question (first one)
        OnNextQuestion();
    }

    public event EventHandler<AnswerArgs> OnAnswer;
    public event EventHandler<HintArgs> OnHint;
    public event EventHandler<ExitArgs> OnExit;
    public event EventHandler<WarningArgs> OnWarning;
    public event EventHandler<NewQuestionArgs> OnNewQuestionData;

    #region AwakeSetters
    
    private void SetEventSubscribers()
    {
        OnHint += (sender, args) =>
        {
            if (args.hint != MyHint.HSwitch) return;
            
            int difficulty = currentQuestionNum >= 10 ? 2 :
                currentQuestionNum >= 5 ? 1 : 0;
            pool.SetQuestion(pool.tipQuestions[difficulty], currentQuestionNum);
            
            SetNewQuestion();
        };
        qwg.OnGetQuestion += ReceiveQuestion;
    }

    private void SetSingletons()
    {
        SetPlayer();
        SetAudioManager();
        SetQuestionBank();
        SetQuestionWebGetter();
    }

    private void SetPlayer() => p = Player.Instance;

    private void SetAudioManager() => audioM = AudioManager.Instance;

    private void SetQuestionBank() => questionBank = QuestionBank.Instance;
    
    private void SetQuestionWebGetter() => qwg = QuestionWebGetter.Instance;

    private void SetSessionQuestions() => pool = questionBank.GetQuestionPoolFromLocal(p);

    #endregion
    
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
        SetNewQuestion();
    }

    private void SetNewQuestion() => qwg.GetRandomQuestionFromServer(currentQuestionNum);

    private void ReceiveQuestion(object sender, QuestionAPIArgs args)
    {
        if (args.connection)
            SetNewQuestionData(args.question);
        else 
            SetNewQuestionData(pool.questions[currentQuestionNum]);
    }

    private void SetNewQuestionData(Question q)
    {
        pool.SetQuestion(q, currentQuestionNum);
        currentQuestion = q;

        QuestionsHandler.Shuffle(rnd, currentQuestion.answers);

        OnNewQuestionData?.Invoke(this,
            new NewQuestionArgs {q = pool.questions[currentQuestionNum], qNum = currentQuestionNum});
    }
    
    
    /*
     * It's button func on AnswerButton
     */
    public void OnAnswerClicked(int choice)
    {
        audioM.Click();

        var isLast = currentQuestionNum == PoolSize - 1;
        var isCorrect = currentQuestion.answers[choice] == pool.answers[currentQuestionNum];

        p.AddAnsweredQuestion(currentQuestion.id, isCorrect);
        
        if (isCorrect && isLast)
            PlayerWin();
        else if (!isCorrect && lives == 0)
            PlayerLose();
        
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
        });
    }

    public void OnHintClicked(int hintID)
    {
        audioM.Click();

        if (!sessionHints[hintID]) return;
        sessionHints[hintID] = false;

        OnHint?.Invoke(this, new HintArgs
        {
            hint = (MyHint) hintID,
            correctChoice = GetCorrectChoice()
        });
    }

    #endregion

    #region WarningButton

    /*
     * It's button func on WarningButton
     */
    public void OnWarningClicked()
    {
        audioM.Click();
        OnWarning?.Invoke(this, new WarningArgs {action = WindowAction.Open});
    }

    /*
     * It's button func on WarningButton
     */
    public void OnWarningConfirmClicked()
    {
        audioM.Click();
        // SEND WARNING TO US
        Debug.LogWarning("Warning on the question!");
        OnWarning?.Invoke(this, new WarningArgs {action = WindowAction.Confirm});
    }

    /*
     * It's button func on WarningButton
     */
    public void OnWarningBackClicked()
    {
        audioM.Click();
        OnWarning?.Invoke(this, new WarningArgs {action = WindowAction.Close});
    }

    #endregion

    #region ExitButton

    /*
     * It's button func on ExitButton
     */
    public void OnExitClicked()
    {
        audioM.Click();
        OnExit?.Invoke(this, new ExitArgs {action = WindowAction.Open, qNum = currentQuestionNum});
    }

    /*
     * It's button func on ExitConfirmButton
     */
    public void OnExitConfirmedClicked()
    {
        audioM.Click();
        PlayerExit();
        OnExit?.Invoke(this, new ExitArgs {action = WindowAction.Confirm, qNum = currentQuestionNum});
    }

    public void OnExitBackClicked()
    {
        audioM.Click(); // hello
        OnExit?.Invoke(this, new ExitArgs {action = WindowAction.Close});
    }

    #endregion

    #region GameEndFunctions

    private void PlayerWin()
    {
        p.Win();
    }

    private void PlayerLose()
    {
        p.Lose(currentQuestionNum);
    }

    private void PlayerExit()
    {
        p.Exit(currentQuestionNum);
    }

    #endregion

    #region Utils

    private int GetCorrectChoice()
    {
        for (var i = 0; i < 4; i++)
            if (currentQuestion.answers[i].Equals(pool.answers[currentQuestionNum]))
                return i;
        Debug.LogError("NO CORRECT ANSWER");
        return 0;
    }

    #endregion
}