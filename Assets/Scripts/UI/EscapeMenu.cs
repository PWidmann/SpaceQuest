using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject escapeMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menuBackGround;

    private PlayerController playerController;

    private bool escapeMenuActive = false;

    private bool isInSettingsMenu = false;

    private void Start()
    {
        escapeMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        menuBackGround.SetActive(false);
    }

    private void Update()
    {
        SearchForPlayerController();
        ActivateEscapeMenu();
    }

    public void ContinueButton()
    {
        escapeMenuPanel.SetActive(false);
        playerController.SetPlayerControlTrue();
        menuBackGround.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        escapeMenuActive = false;
        Time.timeScale = 1;
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(true); 
        escapeMenuPanel.SetActive(false);
        isInSettingsMenu = true;
    }

    public void BackToMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ApplyButton()
    { 
    
    }

    public void BackButton()
    {
        settingsPanel.SetActive(false);
        escapeMenuPanel.SetActive(true);
        isInSettingsMenu = false;
    }

    private void SearchForPlayerController()
    {
        if (!playerController)
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
    }

    private void ActivateEscapeMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (escapeMenuActive && !isInSettingsMenu)
            {
                // Close escape menu
                ContinueButton();
            }
            else if (escapeMenuActive && isInSettingsMenu)
            {
                // Back out of settings
                BackButton();
            }
            else
            {
                // Open escape menu
                escapeMenuActive = true;
                escapeMenuPanel.SetActive(true);
                settingsPanel.SetActive(false);
                playerController.SetPlayerControlFalse();
                menuBackGround.SetActive(true);
                Debug.Log("Escape into menu");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Time.timeScale = 0;
            }
        }
    }
}
