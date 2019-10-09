using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private GameObject m_GameOverPanel;
    [SerializeField]
    private LevelLogic m_levelLogic;
    [SerializeField]
    private int myLevel;
    [SerializeField]
    private GameObject m_winPanel;
    [SerializeField]
    private GameObject m_pausePanel;

    public void SetHealthbar(float life)
    {
        healthBar.fillAmount = life;
    }

    public void GameOver()
    {
        m_GameOverPanel.SetActive(true);
    }

    public void RestartButton()
    {
        m_levelLogic.LoadLevel(myLevel);
    }

    public void BackToMenuButton()
    {
        m_levelLogic.LoadLevel(0);
    }

    public void ShowWinPanel()
    {
        m_winPanel.SetActive(true);
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            m_pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            m_pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
