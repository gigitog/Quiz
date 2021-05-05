using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

internal class TextQuestionsReader
{
    public static List<List<Question>> LoadData((string, string, string) files, (int, int, int) numsQs)
    {
        // return list of 3 lists (difficutlies)
        var qbank = new List<List<Question>>();
        qbank.Add(LoadQuestionsFromTxt(files.Item1, numsQs.Item1));
        qbank.Add(qbank[0]);
        qbank.Add(qbank[0]);
        return qbank;
    }


    private static List<Question> LoadQuestionsFromTxt(string qFile, int numQ)
    {
        var tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, qFile + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return default;
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return default;
        }

        //Load saved Json
        byte[] txtByte = null;
        try
        {
            txtByte = File.ReadAllBytes(tempPath);
            Debug.Log("L D ! from: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        //Convert to txt string
        var txtData = Encoding.UTF8.GetString(txtByte);

        //Convert to Object
        return GetQuestionsByDiff(txtData, numQ);
    }


    // block of questions -> List<q>
    private static List<Question> GetQuestionsByDiff(string blockOfQuestions, int numQs)
    {
        var questions = new List<Question>();
        // get lines
        var lines = new List<string>();
        var regexLine = new Regex(@".*[^\n]");
        var l = regexLine.Matches(blockOfQuestions);
        foreach (Match m in l) lines.Add(m.Value);


        var ls = lines.ToArray();
        for (var i = 0; i < numQs; i++)
        {
            var seg = new ArraySegment<string>(ls, i * 5, 5);

            var q = GetQuestionFromString(seg.ToArray());
            questions.Add(q);
        }

        return questions;
    }

    // txt question -> question 
    /* 23. Привет я кто ты?
     * 1) ответ привет
     * 2) ответ второй
     * 3) ответ неправильный
     * 4) * правильный ответ
     */
    private static Question GetQuestionFromString(string[] lines)
    {
        var patternNumber = @"\d+\. ";
        var answers = new string[4];
        var question = new Question();
        // set question's id, text and answers 
        {
            var regexId = new Regex(@"\d+");
            int.TryParse(regexId.Matches(lines[0])[0].Value, out question.id);

            question.question = Regex.Replace(lines[0], patternNumber, string.Empty);

            for (var i = 0; i < 4; i++)
                answers[i] = lines[i + 1];
            //answers[3] = lines[4].Substring(2);

            question.answers = answers;
        }

        return question;
    }
}