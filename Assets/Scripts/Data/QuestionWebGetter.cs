using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

public class QuestionWebGetter : MonoBehaviour
{
    public static QuestionWebGetter Instance { get; private set; }
    private static Random rnd = new Random();
    private void Start()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    #region API Work

    public event EventHandler<QuestionAPIArgs> OnGetQuestion;
    
    public void GetRandomQuestionFromAPI(int qnum)
    {
        int difficulty = qnum >= 10 ? 3 : qnum >= 5 ? 2 : 1;
        string uri = "https://engine.lifeis.porn/api/millionaire.php?";
        uri += ($"qType={difficulty}&count=1");
        
        StartCoroutine(Co_GetRequest(uri, ProcessResultOfGetRequest));
    }
    
    public struct AnswerAPI
    {
        public int state;
        public Question[] data;
    }

    #endregion

    #region Server Work

    public void GetRandomQuestionFromServer(int qnum)
    {
        int difficulty = qnum >= 10 ? 3 : qnum >= 5 ? 2 : 1;
        int id = rnd.Next(50, 148);
        string uri = $"https://routine2.pnit.od.ua/question/get?id={id}";
        StartCoroutine(Co_GetRequest(uri, ProcessResultOfGetRequest));
    }
    

    #endregion

    #region Get Request

    private void ProcessResultOfGetRequest(bool isSuccess, string json)
    {
        if (isSuccess)
        {
            OnGetQuestion?.Invoke(this, new QuestionAPIArgs
            {
                connection = true,
                question = ParseJsonQuestion(json)
            } );
        }
        else
        {
            OnGetQuestion?.Invoke(this, new QuestionAPIArgs
            {
                connection = false,
                question = new Question()
            } );
        }
    }
    
    private IEnumerator Co_GetRequest(string uri, Action<bool, string> action)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                action(false, "");
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                action(true, webRequest.downloadHandler.text);

                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
    
    private Question ParseJsonQuestion(string json)
    {
        object resultValue = JsonConvert.DeserializeObject<AnswerAPI>(json);
        var returnVar = (AnswerAPI) Convert.ChangeType(resultValue, typeof(AnswerAPI));

        return returnVar.state == 0 ? returnVar.data[0] : new Question();
    }

    #endregion
    
}
