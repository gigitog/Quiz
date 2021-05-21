using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public Player p;

    public TextAsset text;

    //public PlayerSession session;
    private void Awake()
    {
        //upload player data
        p = new Player(new TempGetter().LoadPlayerData());
        Application.targetFrameRate = 144;
        QuestionBank.Questions = new TempGetter(text.text).LoadQuestionBank();
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("App Paused");
        if (pause && p != null) SaveData();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        if (p != null) SaveData();
    }

    public void SaveData()
    {
        new TempGetter().SavePlayerData(p.GetData());
    }

    public void SwitchSound(bool isOn)
    {
        AudioManager.Instance.Click();

        AudioManager.Instance.SwitchSound(isOn);
    }

    public void SwitchMusic(bool isOn)
    {
        AudioManager.Instance.Click();

        AudioManager.Instance.SwitchMusic(isOn);
    }
}