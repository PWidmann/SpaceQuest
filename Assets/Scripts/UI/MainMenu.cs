using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [Header("Settings Panel")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown windowDropdown;

    public void StartButton()
    {
        SceneManager.LoadScene("PlanetGeneratorScene");
    }

    public void SettingsButton()
    {
        if (settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
        else
        {
            settingsPanel.SetActive(true);
        }
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
}
