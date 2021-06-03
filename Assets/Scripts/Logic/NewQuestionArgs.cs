#region

using System;

#endregion

public class NewQuestionArgs : EventArgs
{
    public Question q;
    public int qNum;
}

public class AnswerArgs : EventArgs
{
    public int choice; // какой ответ подсвечивать
    public bool correct; // либо правильный ответ, либо нет
    public int correctChoice;
    public int qNum; // передача номера, чтобы тянуть время с проверкой ответа
}

public class ExitArgs : EventArgs
{
    public WindowAction action;
    public int qNum;
}

public enum WindowAction
{
    Open,
    Close,
    Confirm
}

public class WarningArgs : EventArgs
{
    public WindowAction action;
}

public class QuestionAPIArgs : EventArgs
{
    public bool connection;
    public Question question;
}

public enum MyHint
{
    H5050 = 0,
    HStats = 1,
    HComp = 2,
    HSwitch = 3,
    HClose = 4
}

public class HintArgs : EventArgs
{
    public int correctChoice;
    public MyHint hint;
}

// TODO: do smth with this
public enum GameState
{
    Answering,
    Waiting,
    Resulting,
    Ending
}

public enum QuestionDifficulty
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}