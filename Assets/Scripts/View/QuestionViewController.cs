
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionViewController : MonoBehaviour
{
    private const int PoolSize = 15;

    [HideInInspector] public PlayerSession ps;

    [Header("UI")] public Text questionNum;

    public Text questionMoney;
    public GameObject questionText;
    public List<TextMeshProUGUI> answers;
    public List<ButtonManagerBasic> ansButtons;
    public List<Button> hintButtons;
    public GameObject disablerButtons;

    [Header("PopUp")]
    // public GameObject popUpPanel;
    public GameObject warningConfirmPanel;
    public GameObject exitConfirmPanel;
    public Text exitPrizeText;
    public GameObject endGamePanel;
    public Text endGamePrizeText;
    public GameObject prizePanel;
    public Text fireProfit;
    public Text nextPrizeText;
    public Text nextQuestion;
    public GameObject hintPanel;
    public Text hintLetter;
    private readonly Color blue = new Color(158 / 255f, 225 / 255f, 253 / 255f);
    private readonly Color green = new Color(137 / 255f, 239 / 255f, 120 / 255f);
    private readonly Color orange = new Color(253 / 255f, 209 / 255f, 158 / 255f);
    private readonly Color red = new Color(1f, 119 / 255f, 119 / 255f);

    private AudioManager audioM;
    private readonly System.Random rnd = new System.Random();
    private void Start()
    {
        audioM = AudioManager.Instance;
        ps = gameObject.GetComponent<PlayerSession>();
        ps.OnNextQuestionData += Ps_OnNextQuestionData;
        ps.OnAnswer += Ps_OnAnswer;
        ps.OnExit += Ps_OnExit;
        ps.OnWarning += Ps_OnWarning;
        ps.OnHint += Ps_OnHint;
    }

    #region HintHandler

    private void Ps_OnHint(object sender, HintArgs e)
    {
        DisableHintButton((int)e.hint);
        switch (e.hint)
        {
            case MyHint.H5050:
                Hint50(e.correctChoice);
                break;
            case MyHint.HStats:
                ReleaseHintPanel(e.hint, e.correctChoice);
                break;
            case MyHint.HComp:
                ReleaseHintPanel(e.hint, e.correctChoice);
                break;
            case MyHint.HSwitch:
                // TODO: Make implementation of HSwitch
                break;
            case MyHint.HClose:
                CloseHintPanel();
                break;
            default:
                Debug.LogError("IncorrectHint");
                break;
        }
    }

    private void DisableHintButton(int id)
    {
        hintButtons[id].GetComponent<Image>().color = Color.gray;
        hintButtons[id].GetComponent<Button>().interactable = false;
    }

    private void Hint50(int correctChoice)
    {
        (int, int) tuple; // tuple of 2 incorrect answers
        List<int> tempAnswers = new List<int>() {0, 1, 2, 3};
        tempAnswers.Remove(correctChoice);
        int randId = rnd.Next(3);
        tuple.Item1 = tempAnswers[randId];
        tempAnswers.Remove(randId);
        randId = rnd.Next(2);
        tuple.Item2 = tempAnswers[randId];
        // remove 2 incorrect answers;
        //animate
        ansButtons[tuple.Item1].GetComponent<CanvasGroup>().alpha = 0;
        ansButtons[tuple.Item1].GetComponent<CanvasGroup>().interactable = false;
        ansButtons[tuple.Item2].GetComponent<CanvasGroup>().alpha = 0;
        ansButtons[tuple.Item2].GetComponent<CanvasGroup>().interactable = false;
        
    }

    private void ReleaseHintPanel(MyHint hint, int correctChoice)
    {
        // TODO: Create proper hints
        //animate 
        hintPanel.SetActive(true);
        if (hint == MyHint.HStats)
        {
            hintLetter.text = GetLetter(correctChoice);
        }
        else if (hint == MyHint.HComp)
        {
            hintLetter.text = GetLetter(correctChoice);
        }
    }

    private string GetLetter(int i)
    {
        var s = new[] {"A", "B", "C", "D"};
        return s[i];
    }
    
    private void CloseHintPanel()
    {
        //animate
        hintPanel.SetActive(false);
    }

    #endregion

    private void Ps_OnWarning(object sender, WarningArgs e)
    {
        if (e.action == MyAction.Open)
            warningConfirmPanel.SetActive(true);
        else if (e.action == MyAction.Confirm)
            warningConfirmPanel.SetActive(false);
        else if (e.action == MyAction.Close) warningConfirmPanel.SetActive(false);
    }

    private void Ps_OnExit(object sender, ExitArgs e)
    {
        if (e.action == MyAction.Open)
        {
            // Show popup to confirm
            exitConfirmPanel.SetActive(true);
            exitPrizeText.text = "$ " + (e.qNum > 0 ? GetQuestionPrize(e.qNum - 1) : 0);
        }
        else if (e.action == MyAction.Confirm)
        {

            exitConfirmPanel.SetActive(false);

            ShowGameEnd(e.qNum - 1, e.qNum > 0 ? GetQuestionPrize(e.qNum - 1) : 0);
        }
        else if (e.action == MyAction.Close)
        {
            exitConfirmPanel.SetActive(false);
        }
    }

    private void Ps_OnNextQuestionData(object sender, NewQuestionArgs e)
    {
        prizePanel.SetActive(false);
        disablerButtons.SetActive(false);
        SetQuestionData(e.q, e.qNum);
    }

    #region AnswerHandler

    private void Ps_OnAnswer(object sender, AnswerArgs e)
    {
        disablerButtons.SetActive(true);


        // make button another color
        ansButtons[e.choice].GetComponent<Image>().color = orange;

        WaitForResult(e.qNum, e.choice, e.correct, e.correctChoice);
    }

    private void WaitForResult(int qNum, int choice, bool correct, int correctChoice)
    {
        StartCoroutine(Co_WaitForResult(qNum, choice, correct, correctChoice));
    }

    private IEnumerator Co_WaitForResult(int qNum, int choice, bool correct, int correctChoice)
    {
        var secondsWaitForResult = new WaitForSeconds(qNum / 5f + 1f);
        var secondsWaitAfterResult = new WaitForSeconds(1.25f);
        
        yield return secondsWaitForResult;
        
        ShowResult(choice, correct, correctChoice);

        yield return secondsWaitAfterResult;
       
        ShowQuestionEnd(qNum, correct);
    }

    private void ShowResult(int choice, bool correct, int correctChoice)
    {
        //sound
        if (correct) audioM.Correct();
        else audioM.Incorrect();
        //show result
        ansButtons[choice].GetComponent<Image>().color = correct ? green : red;
        if (!correct) ansButtons[correctChoice].GetComponent<Image>().color = green;
    }

    private void ShowQuestionEnd(int qNum, bool correct)
    {
        if (!correct)
            ShowGameEnd(qNum, qNum > 0 ? Player.GetLosePrize(qNum - 1) : 0);
        else if (qNum == PoolSize - 1)
            ShowGameEnd(qNum, GetQuestionPrize(qNum));
        else
            ShowPrizePanel(qNum);
    }

    #endregion

    private void ShowGameEnd(int qNum, long prize)
    {
        Debug.Log(qNum == PoolSize - 1 ? "WIN" : "LOSE/EXIT");

        endGamePrizeText.text = "$ " + prize;
        RevealEndPanel();
    }

    private void RevealEndPanel()
    {
        // animate endGamePanel
        endGamePanel.SetActive(true);
    }

    private void ShowPrizePanel(int qNum)
    {
        qNum++;
        prizePanel.SetActive(true);
        fireProfit.text = "$ " + Player.GetLosePrize(qNum);
        nextPrizeText.text = "$ " + GetQuestionPrize(qNum);
        nextQuestion.text = (qNum + 1) + "/15";

        // animation, show prize
    }

    private void SetQuestionData(Question q, int num)
    {
        questionNum.text = $"{num + 1}/15";
        questionMoney.text = "$" + GetQuestionPrize(num);
        questionText.GetComponent<TextMeshProUGUI>().text = q.question;

        for (var i = 0; i < 4; i++)
        {
            answers[i].text = q.answers[i]; 
            ansButtons[i].GetComponent<CanvasGroup>().alpha = 1;
            ansButtons[i].GetComponent<CanvasGroup>().interactable = true;
            ansButtons[i].GetComponent<Image>().color = blue;
        }
    }

    private long GetQuestionPrize(int num)
    {
        return Player.prizes[num];
    }
}