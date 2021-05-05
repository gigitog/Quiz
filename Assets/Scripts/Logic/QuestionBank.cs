using System.Collections.Generic;
using UnityEngine;

public static class QuestionBank
{
    public const byte POOLSIZE = 15;
    public const byte correctAnswId = 3;
    public const int ez = 100;
    public const int med = 100;
    public const int hard = 100;
    public static List<List<Question>> Questions { get; set; }

    public static Question[] GetQuestionPool(out string[] correctAnswers, Player player = null)
    {
        var usedIds = new List<int>();
        correctAnswers = new string[POOLSIZE];
        var qs = new Question[POOLSIZE];
        for (byte i = 0; i < POOLSIZE; i++)
        {
            var dif = i >= 11 ? 2 : i >= 6 ? 1 : 0;
            qs[i] = GetRandomQuestion(dif, qs, player, usedIds);
            correctAnswers[i] = qs[i].answers[correctAnswId];
        }

        var ss = "";
        foreach (var q in qs) ss += $"qid: {q.id} \nq: {q.question} \n---\n";
        Debug.LogWarning(ss);
        return qs;
    }

    // подправить потом ids
    private static Question GetRandomQuestion(int difficulty, Question[] qs, Player player, List<int> usedIds)
    {
        var size = difficulty == 0 ? ez : difficulty == 1 ? med : hard;
        var id = Random.Range(0, size);
        for (var i = 0; i < POOLSIZE; i++)
        {
            id = Random.Range(0, size);
            if (usedIds.Contains(id))
                id = Random.Range(0, size);
            usedIds.Add(id);
        }

        return Questions[difficulty][id];
    }
}