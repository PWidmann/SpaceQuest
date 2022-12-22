using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameGUI : MonoBehaviour
{
    [SerializeField] private GameObject planetGeneratorPanel;
    [SerializeField] private GameObject spawnPlayerButton;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private GameObject escapeMenuObject;

    public void Start()
    {
        playerHUD.SetActive(false);
        escapeMenuObject.SetActive(false);
        planetGeneratorPanel.SetActive(true);

        // Disable bloom at start
        //bloom = GameObject.Find("Sky and Fog Global Volume").GetComponent<Bloom>();
        //bloom.intensity = new UnityEngine.Rendering.ClampedFloatParameter(0, 0, 1);

        //Debug.Log("DLSS capabilities detected: " + UnityEngine.Rendering.HighDefinition.HDDynamicResolutionPlatformCapabilities.DLSSDetected);
    }


    public void HideGeneratorPanel()
    {
        planetGeneratorPanel.SetActive(false);
    }

    public void ShowSpawnButton()
    {
        spawnPlayerButton.SetActive(true);
    }

    public void ShowPlayerHUD()
    {
        playerHUD.SetActive(true);
    }

    public void ActivateEscapeMenu()
    {
        escapeMenuObject.SetActive(true);
    }
}
