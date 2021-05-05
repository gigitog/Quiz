using System;

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
    public MyAction action;
    public int qNum;
}

public enum MyAction
{
    Open,
    Close,
    Confirm
}

public class WarningArgs : EventArgs
{
    public MyAction action;
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
    public MyHint hint;
    public int correctChoice;
}