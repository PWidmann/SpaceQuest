using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.U2D;

public enum GraphicsQuality
{ 
    Low, Medium, High, Custom
}

public class EscapeMenu : MonoBehaviour
{
    [Header("Interface Panels")]
    [SerializeField] private GameObject escapeMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menuBackGround;

    [Header("Sound")]
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TMP_Text effectsValueText;
    [SerializeField] private TMP_Text musicValueText;
    [SerializeField] private TMP_Text mouseSensitivityValueText;

    [Header("Controls")]
    [SerializeField] private Slider mouseSensitivitySlider;

    [Header("Graphics")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown windowDropdown;
    [SerializeField] private Dropdown vsyncDropdown;
    [SerializeField] private Dropdown graphicsQualityDropdown;
    [SerializeField] private Dropdown textureQualityDropdown;
    [SerializeField] private Dropdown aaDropdown;
    [SerializeField] private Dropdown afDropdown;
    [SerializeField] private Dropdown pixelLightCountDropdown;
    [SerializeField] private Dropdown shadowDistanceDropdown;
    [SerializeField] private Dropdown shadowResolutionDropdown;
    
    [SerializeField] private Toggle dlssToggle;
    [SerializeField] private Dropdown dlssQualityDropdown;

    [Header("Buttons")]
    [SerializeField] private Button applyButton;

    [Header("Renderpipeline Quality Assets")]
    [SerializeField] private RenderPipelineAsset renderPA_low;
    [SerializeField] private RenderPipelineAsset renderPA_medium;
    [SerializeField] private RenderPipelineAsset renderPA_high;

    private PlayerController playerController;

    private bool escapeMenuActive = false;

    private bool isInSettingsMenu = false;

    private void Start()
    {
        escapeMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        menuBackGround.SetActive(false);
        applyButton.interactable = false;

        // TO DO
        // Detect current resolution settings and apply to the in game settings
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
        // Sound settings to soundmanager
        // TO DO: Soundmanager

        // Screen settings
        string dropDownValue = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] resolution = dropDownValue.Split('x');
        bool fullScreen = windowDropdown.value == 0 ? false : true;
        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), fullScreen);

        // Set graphics Quality
        SetGraphicsQuality(graphicsQualityDropdown.value);

        QualitySettings.vSyncCount = vsyncDropdown.value;

        applyButton.interactable = false;
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

    private void SetGraphicsQuality(int _index)
    {
        if (_index < 3) // Graphic presets
        {
            QualitySettings.SetQualityLevel(_index, false);
        }
        else // Custom Graphics
        {
            // Texture Quality - int
            // Full Res - 0, Half Res - 1, Quarter Res - 2, Eighth Res - 3
            QualitySettings.masterTextureLimit = textureQualityDropdown.value;
            
            // Pixel light count - int, up to 4
            QualitySettings.pixelLightCount = pixelLightCountDropdown.value;

            // Anti Aliasing
            // Int Value
            // Disabled = 0, 2x = 1, 4x = 2, 8x = 3
            QualitySettings.antiAliasing = aaDropdown.value;

            // Shadow Resolution
            // Enum
            switch (shadowResolutionDropdown.value)
            {
                case 0:
                    QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                    break;
                case 1:
                    QualitySettings.shadowResolution = ShadowResolution.High;
                    break;
                case 2:
                    QualitySettings.shadowResolution = ShadowResolution.Medium;
                    break;
                case 3:
                    QualitySettings.shadowResolution = ShadowResolution.Low;
                    break;
            }

            // Shadow draw distance
            switch (shadowDistanceDropdown.value)
            {
                case 0:
                    QualitySettings.shadowDistance = 400f;
                    break;
                case 1:
                    QualitySettings.shadowDistance = 200f;
                    break;
                case 2:
                    QualitySettings.shadowDistance = 100f;
                    break;
                case 3:
                    QualitySettings.shadowDistance = 50f;
                    break;
            }

            // Anisotropic Filtering
            switch (afDropdown.value)
            {
                case 0:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    break;
                case 1:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
                case 2:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
            }
        }  
    }

    public void EnableApplyButton()
    {
        applyButton.interactable = true;
    }


    public void SetInteractibility()
    {
        // Depending on the graphics quality dropdown value, advanced graphics settings are enabled/disabled
        if (graphicsQualityDropdown.value == 3)
        {
            textureQualityDropdown.interactable = true;
            pixelLightCountDropdown.interactable = true;
            aaDropdown.interactable = true;
            shadowResolutionDropdown.interactable = true;
            shadowDistanceDropdown.interactable = true;
            afDropdown.interactable = true;
        }
        else
        {
            textureQualityDropdown.interactable = false;
            pixelLightCountDropdown.interactable = false;
            aaDropdown.interactable = false;
            shadowResolutionDropdown.interactable = false;
            shadowDistanceDropdown.interactable = false;
            afDropdown.interactable = false;
        }
    }

    public void MouseSensitivitySlider()
    {
        GameManager.MouseSensitivity = mouseSensitivitySlider.value;
        mouseSensitivityValueText.text = GameManager.MouseSensitivity.ToString();
    }

    public void SfxSlider()
    {
        GameManager.SfxVolume = effectsVolumeSlider.value;
        effectsValueText.text = (int)(GameManager.SfxVolume * 100f) + " %";
    }

    public void MusicSlider()
    {
        GameManager.MusicVolume = musicVolumeSlider.value;
        musicValueText.text = (int)(GameManager.MusicVolume * 100f) + " %";
    }
}
