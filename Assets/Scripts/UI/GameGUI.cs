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
