using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SplashScreen : MonoBehaviour
{
    #region Members

    [SerializeField] private Image MHPlogoImage;
    [SerializeField] private float fadeSpeed = 0.5f;
    private float opacity = 0f;
    private float startTimer = 0;
    private bool fadeIn = false;

    #endregion

    #region Unity Methods

    void Start()
    {
        // Make logo transparent on start
        opacity = 0;
        startTimer = 0;
        MHPlogoImage.color = new Color(255, 255, 255, opacity);
    }

    void Update()
    {
        FadeInOut();
    }

    #endregion

    #region Methods

    private void FadeInOut()
    {
        startTimer += Time.deltaTime;
        if (startTimer > 1f && opacity == 0f)
        {
            fadeIn = true;
        }

        if (fadeIn)
        {
            if (opacity < 1f)
            {
                opacity += Time.deltaTime * fadeSpeed;
                MHPlogoImage.color = new Color(255, 255, 255, opacity);
            }
            else
            {
                fadeIn = false;
            }
        }
        else
        {
            // Fade out after 5 seconds
            if (startTimer > 5f)
            {
                opacity -= Time.deltaTime * fadeSpeed;
                MHPlogoImage.color = new Color(255, 255, 255, opacity);

                if (opacity <= 0)
                {
                    SceneManager.LoadScene("PlanetScene");
                }
            }
        }
    }

    #endregion
}
