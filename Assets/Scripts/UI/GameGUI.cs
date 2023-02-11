using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameGUI : MonoBehaviour
{
    [SerializeField] private Slider lavaSlider;

    public void Start()
    {
        lavaSlider.gameObject.SetActive(false);
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


    
}
