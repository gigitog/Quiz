using System;
using System.Collections.Generic;

[Serializable]
public class Player
{
    public static int[] prizes =
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

    private long _money;
    public Queue<int> last20Q;
    public Statistic Stats { get; private set; }
    public long Money
    {
        get => _money;
        set => _money = value > 0 ? value : _money;
    }
    public Player()
    {
        _money = 0;
        Stats = new Statistic();
        last20Q = new Queue<int>();
    }

    public Player(PlayerData d)
    {
        _money = d.money;
        Stats = d.stats ?? new Statistic();
        last20Q = d.last20Q ?? new Queue<int>();
    }
    public PlayerData GetData()
    {
        return new PlayerData
        {
            id = "",
            money = _money,
            stats = Stats,
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

    public void Win() => Money += prizes[14];

    public void Lose(int qNum) => Money += GetLosePrize(qNum);

    public static long GetLosePrize(int qNum)
    {
        long prize;
        if (qNum > 10) prize = prizes[10];
        else if (qNum > 6) prize = prizes[6];
        else if (qNum > 3) prize = prizes[3];
        else prize = 0;
        return prize;
    }

    public void Exit(int qNum)
    {
        if (qNum > 0) Money += prizes[qNum - 1];
    }
}