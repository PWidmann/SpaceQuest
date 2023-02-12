using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.U2D;

public class EscapeMenu : MonoBehaviour
{

    [SerializeField] private GameObject menuBackground;
    [SerializeField] private GameObject escapeMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    private PlayerController playerController;
    private bool escapeMenuActive = false;
    private bool isInSettingsMenu = false;


    private void Start()
    {
        
    }

    private void Update()
    {
        // Get ref for player controller if not initialized
        SearchForPlayerController();

        if (playerController)
        {
            SetMenuActive();
        }
    }

    private void SetMenuActive()
    {
        // Enable/disable menu with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (!escapeMenuActive && !playerController.Death)
            {
                escapeMenuActive = true;
            }
            else
            {
                if (isInSettingsMenu)
                {
                    settingsPanel.SetActive(false);
                    escapeMenuPanel.SetActive(true);
                    isInSettingsMenu = false;
                }
                else
                {
                    escapeMenuActive = false;
                }
            }
        }


        // Set menu panels accordingly to menu state
        if (playerController && escapeMenuActive)
        {
            escapeMenuPanel.SetActive(true);
            Time.timeScale = 0;
            playerController.SetPlayerIsInControl(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            menuBackground.SetActive(true);
        }

        if(playerController && !escapeMenuActive)
        {
            escapeMenuPanel.SetActive(false);
            Time.timeScale = 1;
            playerController.SetPlayerIsInControl(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            menuBackground.SetActive(false);
        }
    }

    public void ContinueButton()
    {
        playerController.SetPlayerIsInControl(true);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        escapeMenuActive = false;
        Time.timeScale = 1;
    }

    public void SettingsButton()
    {
        escapeMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        isInSettingsMenu = true;
    }

    public void BackToMenuButton()
    {
        Time.timeScale = 1;
        GameObject fadeScreen = GameObject.Find("FadeCanvas");
        Destroy(fadeScreen);
        SceneManager.LoadScene("MainMenu");

    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void SearchForPlayerController()
    {
        if (!playerController)
        {
            try
            {
                playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            }
            catch
            { 
            
            }
        }
    }
}
