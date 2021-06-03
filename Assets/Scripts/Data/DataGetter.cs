#region

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

public static class DataGetter
{
    public static PlayerData LoadPlayerData()
    {
        return DataSaver.LoadData<PlayerData>("player") ?? new PlayerData();
    }

    public static void SavePlayerData(PlayerData p)
    {
        DataSaver.SaveData(p, "player");
    }

    public static List<List<Question>> LoadQuestionBank(string text)
    {
        object resultValue = JsonConvert.DeserializeObject<List<Question>>(text);
        var q1 = (List<Question>) Convert.ChangeType(resultValue, typeof(List<Question>));
        q1 = QuestionsHandler.ReplaceStars(q1);
        var bank = new List<List<Question>> {q1, q1, q1};

        return bank;
    }
    

}