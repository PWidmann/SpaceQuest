using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameGUI : MonoBehaviour
{
    [SerializeField] private Slider lavaSlider;
    [SerializeField] private GameObject youDiedPanel;
    [SerializeField] private GameObject compass;
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private GameObject playerHealth;

    private void Awake()
    {
        lavaSlider.gameObject.SetActive(false);
        youDiedPanel.SetActive(false);
        compass.SetActive(false);
        interactPanel.SetActive(false);
        playerHealth.SetActive(false);
    }

    public void ShowLavaMeter(float value)
    {
        lavaSlider.gameObject.SetActive(true);
        lavaSlider.value = value;
    }

    public void HideLavaMeter()
    {
        lavaSlider.gameObject.SetActive(false);
    }

    public void SetDeathPanel(bool active)
    {
        youDiedPanel.SetActive(active);
    }

    public void SetPlayerHealthActive(bool active)
    {
        playerHealth.SetActive(active);
    }

    public void SetPlayerHealthBar(int value)
    {
        playerHealth.GetComponent<Slider>().value = value;
    }

    public void SetCompass(bool active)
    {
        compass.SetActive(active);
    }

    public void SetInteractPanel(bool active)
    {
        interactPanel.SetActive(active);
    }
}
