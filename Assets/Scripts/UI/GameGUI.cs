using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGUI : MonoBehaviour
{
    [SerializeField] private GameObject planetGeneratorPanel;
    [SerializeField] private GameObject spawnPlayerButton;

    public void HideGeneratorPanel()
    {
        planetGeneratorPanel.SetActive(false);
    }

    public void ShowSpawnButton()
    {
        spawnPlayerButton.SetActive(true);
    }
}
