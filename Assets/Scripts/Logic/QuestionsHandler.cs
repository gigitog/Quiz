#region

using System;
using System.Collections.Generic;

#endregion

public static class QuestionsHandler
{
    public static List<Question> ReplaceStars(List<Question> text)
    {
        for (var i = 0; i < text.Count; i++) text[i].answers[3] = text[i].answers[3].Replace("* ", "");

        return text;
    }

    public static void Shuffle<T>(Random rng, T[] array)
    {
        var n = array.Length;
        while (n > 1)
        {
            var k = rng.Next(n--);
            var temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}