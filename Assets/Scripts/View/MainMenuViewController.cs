#region

using System.Globalization;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class MainMenuViewController : MonoBehaviour
{
    [SerializeField] private Text profit;
    [SerializeField] private GameObject prefsPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private SwitchManager[] managers;
    [SerializeField] private Text correctNum;
    [SerializeField] private Text wrongNum;
    [SerializeField] private Text ratio;
    private AudioManager am;
    private Player player;

    private void Start()
    {
        am = AudioManager.Instance;
        player = Player.Instance;
        profit.text = "$ " + player.Money; // написать money prettifier
    }

    public void OpenPrefsPanel()
    {
        am.Click();
        // animation
        prefsPanel.SetActive(true);
    }

    public void ClosePrefsPanel()
    {
        am.Click();

        // animation
        prefsPanel.SetActive(false);
    }

    public void OpenStatsPanel()
    {
        am.Click();
        statsPanel.SetActive(true);
        correctNum.text = player.Stats.Correct.ToString();
        wrongNum.text = player.Stats.Wrong.ToString();
        ratio.text = player.Stats.Ratio.ToString("F", CultureInfo.CurrentCulture);
    }

    public void CloseStatsPanel()
    {
        am.Click();
        statsPanel.SetActive(false);
    }
}