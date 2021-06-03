#region

using System;

#endregion

[Serializable]
public struct Question
{
    public int id;
    public string question;
    public string[] answers;
}
