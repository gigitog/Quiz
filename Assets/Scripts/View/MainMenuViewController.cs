using System.Globalization;
using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.UI;
class MainMenuViewController : MonoBehaviour
{
    public Text profit;
    public Player player;
    public GameObject prefsPanel;
    public GameObject statsPanel;
    public GameObject creditsPanel;
    public SwitchManager[] managers;
    private AudioManager am;
    public Text correctNum;
    public Text wrongNum;
    public Text ratio;

    private void Start()
    {
        am = AudioManager.Instance;
        player = GetComponent<MainMenuController>().Player;
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
        correctNum.text = player.Stats.correct.ToString();
        wrongNum.text = player.Stats.wrong.ToString();
        ratio.text = player.Stats.GetStatistic().ToString("F", CultureInfo.CurrentCulture);
    }

    public void CloseStatsPanel()
    {
        am.Click();
        statsPanel.SetActive(false);

    }
}