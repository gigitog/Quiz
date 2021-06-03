#region

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

#endregion

[Serializable]
public class QuestionBank : MonoBehaviour
{
    private const byte PoolSize = 15;
    private const byte DifficultyLevels = 3;
    private const byte CorrectAnswerId = 3;
    [SerializeField] private TextAsset textQuestions;

    [SerializeField] private int ezNUm = 100;
    [SerializeField] private int medNum = 100;
    [SerializeField] private int hardNum = 100;
    public static QuestionBank Instance { get; private set; }
    public QuestionWebGetter qwg;
    private List<List<Question>> Questions { get; set; }

    private void Awake()
    {
        SetSingleton();
        qwg = QuestionWebGetter.Instance;
        LoadLocalQuestionBank();
    }
    
    public QuestionPool GetQuestionPoolFromLocal(Player player = null)
    {
        List<int> usedIds = SetUsedIds(player);
        var pool = new QuestionPool()
        {
            questions = new Question[PoolSize],
            answers = new string[PoolSize],
            tipQuestions = new Question[DifficultyLevels],
            tipAnswers = new string[DifficultyLevels]
        };

        SetQuestions(pool, usedIds);
        SetTipQuestions(pool, usedIds);

        // string ss = "";
        // foreach (var q in qs) ss += $"qid: {q.id} \nq: {q.question} \n---\n";
        // Debug.LogWarning(ss);
        return pool;
    }

    private void SetTipQuestions(QuestionPool pool, List<int> usedIds)
    {
        for (int i = 0; i < DifficultyLevels; i++)
        {
            pool.SetTipQuestion(GetRandomQuestion(i, usedIds), i);
        }
    }

    private void SetQuestions(QuestionPool pool, List<int> usedIds)
    {
        for (byte i = 0; i < PoolSize; i++)
        {
            int difficulty = i >= 10 ? 2 : i >= 5 ? 1 : 0;
            pool.SetQuestion(GetRandomQuestion(difficulty, usedIds), i);
        }
    }

    private static List<int> SetUsedIds(Player player)
    {
        List<int> usedIds = player.last20Q.ToList();
        return usedIds;
    }

    private Question GetRandomQuestion(int difficulty, List<int> usedIds)
    {
        int size = difficulty == 0 ? ezNUm : difficulty == 1 ? medNum : hardNum;

        var id = GetRandomID(usedIds, size);

        var randomQuestion = SetRandomQuestionObject(difficulty, id);

        return randomQuestion;
    }

    private Question SetRandomQuestionObject(int difficulty, int id)
    {
        var randomQuestion = new Question
        {
            question = Questions[difficulty][id].question,
            id = Questions[difficulty][id].id,
            answers = new string[4]
        };

        for (int j = 0; j < 4; j++)
            randomQuestion.answers[j] = Questions[difficulty][id].answers[j];
        return randomQuestion;
    }

    // TODO: remake randomizer id
    private static int GetRandomID(List<int> usedIds, int size)
    {
        int id = Random.Range(0, size);

        for (int i = 0; i < PoolSize; i++)
        {
            id = Random.Range(0, size);
            if (usedIds.Contains(id))
                id = Random.Range(0, size);
            usedIds.Add(id);
        }

        return id;
    }
    
    private void SetSingleton()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void LoadLocalQuestionBank()
    {
        Questions = DataGetter.LoadQuestionBank(textQuestions.text);
    }
}