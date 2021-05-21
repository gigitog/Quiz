using System;

[Serializable]
public class Statistic
{
    public int correct;
    public int wrong;

    public Statistic()
    {
        correct = 0; 
        wrong = 0;
    }

    public void AddCorrect() => correct += 1;

    public void AddWrong() => wrong += 1;

    public float GetStatistic()
    {
        if (correct == 0) return 0;
        return correct / (correct + wrong + 0f);
    }
}