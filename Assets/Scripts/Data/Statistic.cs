#region

using System;

#endregion

[Serializable]
public class Statistic
{
    public Statistic()
    {
        Correct = 0;
        Wrong = 0;
    }

    public Statistic(int c, int w)
    {
        Correct = c;
        Wrong = w;
    }

    public int Correct { get; private set; }
    public int Wrong { get; private set; }
    public float Ratio => Correct == 0 ? 0f : Correct / (Correct + Wrong + 0f);

    public void AddCorrect()
    {
        Correct += 1;
    }

    public void AddWrong()
    {
        Wrong += 1;
    }
}