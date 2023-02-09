using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Members

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    private FadeInScreen fadeScreen;



    #endregion

    #region Methods

    private void Awake()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        fadeScreen = GetComponent<FadeInScreen>();
    }

    private void Start()
    {
        Time.timeScale = 1;
        fadeScreen.fadeStarted = true;
        fadeScreen.alpha = 1;
    }

    public void StartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SettingsButton()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    #endregion
}
