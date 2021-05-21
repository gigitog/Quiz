using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public Player Player { get; private set; }

    [SerializeField] private TextAsset text;

    //public PlayerSession session;
    private void Awake()
    {
        //upload player data
        Player = new Player(TempGetter.LoadPlayerData());
        Application.targetFrameRate = 144;
        QuestionBank.Questions = TempGetter.LoadQuestionBank(text.text);
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("App Paused");
        if (pause && Player != null) SaveData();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        if (Player != null) SaveData();
    }

    private void SaveData() => TempGetter.SavePlayerData(Player.GetData());

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