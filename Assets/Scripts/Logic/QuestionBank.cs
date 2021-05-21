using System.Collections.Generic;
using UnityEngine;

public static class QuestionBank
{
    private const byte PoolSize = 15;
    private const byte CorrectAnswId = 3;
    private const int Ez = 100;
    private const int Med = 100;
    private const int Hard = 100;
    public static List<List<Question>> Questions { get; set; }

    public static Question[] GetQuestionPool(out string[] correctAnswers, Player player = null)
    {
        List<int> usedIds = new List<int>();
        correctAnswers = new string[PoolSize];
        var qs = new Question[PoolSize];
        for (byte i = 0; i < PoolSize; i++)
        {
            int dif = i >= 11 ? 2 : i >= 6 ? 1 : 0;
            qs[i] = GetRandomQuestion(dif, qs, player, usedIds);
            correctAnswers[i] = qs[i].answers[CorrectAnswId];
        }

        // string ss = "";
        // foreach (var q in qs) ss += $"qid: {q.id} \nq: {q.question} \n---\n";
        // Debug.LogWarning(ss);
        return qs;
    }

    // подправить потом ids
    private static Question GetRandomQuestion(int difficulty, Question[] qs, Player player, List<int> usedIds)
    {
        int size = difficulty == 0 ? Ez : difficulty == 1 ? Med : Hard;
        int id = Random.Range(0, size);
        for (int i = 0; i < PoolSize; i++)
        {
            id = Random.Range(0, size);
            if (usedIds.Contains(id))
                id = Random.Range(0, size);
            usedIds.Add(id);
        }

        Question q = new Question
        {
            question = Questions[difficulty][id].question,
            id = Questions[difficulty][id].id,
            answers = new string[4]
        };
        for (int j = 0; j < 4; j++)
            q.answers[j] = Questions[difficulty][id].answers[j];
        
        return q;
    }
}