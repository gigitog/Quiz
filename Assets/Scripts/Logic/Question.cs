using System.Collections.Generic;

public struct Question
{
    public int id;
    public string question;
    public string[] answers;
}

public interface IQuestionLoader
{
    List<List<Question>> LoadQuestionBank();
}

public interface IPlayerSaver
{
    PlayerData LoadPlayerData();
    void SavePlayerData(PlayerData p);
}