#region

using System.Collections.Generic;

#endregion

public class PlayerData
{
    public int correct;
    public string id;
    public Queue<int> last20Q;
    public long money;
    public int wrong;

    public PlayerData()
    {
        id = "";
        last20Q = new Queue<int>();
        correct = 0;
        wrong = 0;
        money = 0;
    }
}