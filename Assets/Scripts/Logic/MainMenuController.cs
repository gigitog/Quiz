#region

using UnityEngine;

#endregion

public class MainMenuController : MonoBehaviour
{
    //public PlayerSession session;
    private void Awake()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
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