using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPool
{
    public Question[] questions;
    public string[] answers;
    public Question[] tipQuestions;
    public string[] tipAnswers;

    private const byte CorrectAnswerId = 3;
    public void SetQuestion(Question q, int id)
    {
        questions[id] = q;
        answers[id] = q.answers[CorrectAnswerId];
    }

    public void SetTipQuestion(Question q, int id)
    {
        tipQuestions[id] = q;
        tipAnswers[id] = tipQuestions[id].answers[CorrectAnswerId];
    }

}
