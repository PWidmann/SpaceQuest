using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameGUI : MonoBehaviour
{
    [SerializeField] private GameObject planetGeneratorPanel;
    [SerializeField] private GameObject spawnPlayerButton;
    [SerializeField] private GameObject playerHUD;

    public void Start()
    {
        playerHUD.SetActive(false);
        planetGeneratorPanel.SetActive(true);
    }


    public void ShowPlayerHUD()
    {
        playerHUD.SetActive(true);
    }
}
