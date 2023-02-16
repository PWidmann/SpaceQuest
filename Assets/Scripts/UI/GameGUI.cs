using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameGUI : MonoBehaviour
{
    #region Members
    [SerializeField] private Slider lavaSlider;
    [SerializeField] private GameObject youDiedPanel;
    [SerializeField] private GameObject compass;
    [SerializeField] private GameObject interactPanel;
    [SerializeField] private GameObject playerHealth;
    [SerializeField] private GameObject questTracker;
    [SerializeField] private Text questTrackerText;

    public GameObject Compass { get => compass; set => compass = value; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        lavaSlider.gameObject.SetActive(false);
        youDiedPanel.SetActive(false);
        Compass.SetActive(false);
        interactPanel.SetActive(false);
        playerHealth.SetActive(false);
        questTracker.SetActive(false);
    }
    #endregion

    #region Public Methods
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
        Compass.SetActive(active);
    }
    public void SetInteractPanel(bool active)
    {
        interactPanel.SetActive(active);
    }
    public void SetQuestTrackerActive(bool active)
    {
        questTracker.SetActive(active);
    }
    public void SetQuestTrackerText(string text)
    {
        questTrackerText.text = text;
    }
    #endregion
}
