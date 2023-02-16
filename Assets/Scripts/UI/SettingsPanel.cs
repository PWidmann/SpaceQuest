using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsPanel : MonoBehaviour
{
    #region Members
    [Header("UI References")]
    [SerializeField] public GameObject settingsPanel;
    [SerializeField] public GameObject parentMenuPanel;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown windowDropdown;
    [SerializeField] private Dropdown graphicsQualityDropdown;
    [SerializeField] private Dropdown vSyncDropdown;
    [SerializeField] private Dropdown aaDropdown;
    [SerializeField] private Button applyButton;
    [SerializeField] private TMP_Text mouseSensitivityValueText;
    [SerializeField] private TMP_Text sfxValueText;
    [SerializeField] private TMP_Text musicValueText;
    #endregion

    #region Public Methods
    public void BackButton()
    {
        parentMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
    public void ApplyButton()
    {
        // Screen settings
        string dropDownValue = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] resolution = dropDownValue.Split('x');
        bool fullScreen = windowDropdown.value == 0 ? false : true;

        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), fullScreen);

        // Set graphics Quality
        QualitySettings.SetQualityLevel(graphicsQualityDropdown.value, false);

        QualitySettings.vSyncCount = vSyncDropdown.value;

        // Anti Aliasing
        // Int Value
        // Disabled = 0, 2x = 1, 4x = 2, 8x = 3
        QualitySettings.antiAliasing = aaDropdown.value;

        applyButton.interactable = false;
    }
    public void ActivateApplyButton()
    {
        applyButton.interactable = true;
    }
    public void MouseSensitivitySlider()
    {
        GameManager.MouseSensitivity = mouseSensitivitySlider.value;
        mouseSensitivityValueText.text = GameManager.MouseSensitivity.ToString();
    }
    public void SfxSlider()
    {
        GameManager.SfxVolume = effectsVolumeSlider.value;
        sfxValueText.text = (GameManager.SfxVolume) + " %";
    }
    public void MusicSlider()
    {
        GameManager.MusicVolume = musicVolumeSlider.value;
        musicValueText.text = (int)(GameManager.MusicVolume) + " %";
    }
    #endregion
}
