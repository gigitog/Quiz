#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

[Serializable]
public class Player : MonoBehaviour
{
    public Queue<int> last20Q;
    private long money;
    public string Id { get; private set; }
    public Statistic Stats { get; private set; }

    public long Money
    {
        get => money;
        private set => money = value > 0 ? value : money;
    }

    public static Player Instance { get; private set; }

    private void Awake()
    {
        SetSingleton();

        SetData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void SetSingleton()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void SetData()
    {
        var d = DataGetter.LoadPlayerData();
        Id = d.id;
        Money = d.money;
        Stats = new Statistic(d.correct, d.wrong);
        last20Q = d.last20Q ?? new Queue<int>();
    }

    public PlayerData GetData()
    {
        return new PlayerData
        {
            id = Id,
            money = Money,
            correct = Stats.Correct,
            wrong = Stats.Wrong,
            last20Q = last20Q
        };
    }

    public void AddAnsweredQuestion(int id, bool correct)
    {
        if (last20Q == null)
            last20Q = new Queue<int>();
        last20Q.Enqueue(id);
        if (last20Q.Count > 20)
            last20Q.Dequeue();

        if (correct)
            Stats.AddCorrect();
        else
            Stats.AddWrong();
    }

    public void Win()
    {
        Money += PrizesClass.Prizes[14];
        SaveData();
    }

    public void Lose(int qNum)
    {
        Money += PrizesClass.GetLosePrize(qNum);
        SaveData();
    }

    public void Exit(int qNum)
    {
        if (qNum > 0) Money += PrizesClass.Prizes[qNum - 1];
        SaveData();
    }

    public void SaveData()
    {
        DataGetter.SavePlayerData(GetData());
    }
}

public static class PrizesClass
{
    public static readonly int[] Prizes =
    {
        500,
        1000,
        2000,
        3000,
        5000,
        10000,
        15000,
        25000,
        50000,
        100000,
        200000,
        400000,
        800000,
        2000000,
        50000000
    };

    public static long GetLosePrize(int qNum)
    {
        long prize;
        if (qNum > 10) prize = Prizes[10];
        else if (qNum > 6) prize = Prizes[6];
        else if (qNum > 3) prize = Prizes[3];
        else prize = 0;
        return prize;
    }
}