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
    private const byte CorrectAnswerId = 3;
    [SerializeField] private TextAsset textQuestions;

    [SerializeField] private int ez = 100;
    [SerializeField] private int med = 100;
    [SerializeField] private int hard = 100;
    public static QuestionBank Instance { get; private set; }
    private List<List<Question>> Questions { get; set; }

    private void Awake()
    {
        SetSingleton();

        LoadQuestionBank();
    }

    private void SetSingleton()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void LoadQuestionBank()
    {
        Questions = DataGetter.LoadQuestionBank(textQuestions.text);
    }

    public Question[] GetQuestionPool(out string[] correctAnswers, Player player = null)
    {
        List<int> usedIds = SetUsedIds(player);
        var questionPool = new Question[PoolSize];

        correctAnswers = new string[PoolSize];

        for (byte i = 0; i < PoolSize; i++)
        {
            int difficulty = i >= 10 ? 2 : i >= 5 ? 1 : 0;
            questionPool[i] = GetRandomQuestion(difficulty, usedIds);
            correctAnswers[i] = questionPool[i].answers[CorrectAnswerId];
        }

        // string ss = "";
        // foreach (var q in qs) ss += $"qid: {q.id} \nq: {q.question} \n---\n";
        // Debug.LogWarning(ss);
        return questionPool;
    }

    private static List<int> SetUsedIds(Player player)
    {
        List<int> usedIds = player.last20Q.ToList();
        return usedIds;
    }

    private Question GetRandomQuestion(int difficulty, List<int> usedIds)
    {
        int size = difficulty == 0 ? ez : difficulty == 1 ? med : hard;

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
}