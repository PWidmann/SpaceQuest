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
    private FadeScreen fadeScreen;
    private float startTimer = 0;
    private bool startGame = false;

    #endregion

    #region Methods

    private void Awake()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        fadeScreen = GameObject.Find("FadeCanvas").GetComponent<FadeScreen>();
        
    }

    private void Start()
    {
        startTimer = 0;
        Time.timeScale = 1;
        fadeScreen.FadeIn();
    }

    private void Update()
    {
        if (startGame)
        {
            startTimer += Time.deltaTime;

            if (startTimer > 4f)
            {
                startGame = false;
                startTimer = 0;
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    public void StartButton()
    {
        fadeScreen.FadeOut();
        startGame = true;
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
