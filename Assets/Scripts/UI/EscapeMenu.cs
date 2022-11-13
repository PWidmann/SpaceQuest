using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject escapeMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menuBackGround;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown windowDropdown;
    [SerializeField] private Slider mouseSensitivitySlider;

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
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ApplyButton()
    {
        // Screen settings
        string dropDownValue = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] resolution = dropDownValue.Split('x');
        bool fullScreen = windowDropdown.value == 0 ? false : true;

        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), fullScreen);
    }

    public void BackButton()
    {
        settingsPanel.SetActive(false);
        escapeMenuPanel.SetActive(true);
        isInSettingsMenu = false;
    }

    public void SetMouseSensitivity()
    {
        GameManager.MouseSensitivity = mouseSensitivitySlider.value;
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
