using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class TempGetter : IQuestionLoader, IPlayerSaver
{
    public string text;

    public TempGetter(string text)
    {
        this.text = text;
    }

    public TempGetter()
    {
        text = "";
    }

    public PlayerData LoadPlayerData()
    {
        return DataSaver.LoadData<PlayerData>("player") ?? new PlayerData();
    }

    public void SavePlayerData(PlayerData p)
    {
        DataSaver.SaveData(p, "player");
    }

    public List<List<Question>> LoadQuestionBank()
    {
        object resultValue = JsonConvert.DeserializeObject<List<Question>>(text);
        var q1 = (List<Question>) Convert.ChangeType(resultValue, typeof(List<Question>));
        q1 = QuestionsHandler.ReplaceStars(q1);
        var bank = new List<List<Question>>();
        bank.Add(q1);
        bank.Add(q1);
        bank.Add(q1);

        return bank;
    }
}