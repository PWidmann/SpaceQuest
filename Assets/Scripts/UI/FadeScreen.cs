using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    #region Members

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 0.2f;
    private float alpha = 1f;
    private bool fadeStarted = false;
    private bool fadingIn = false;
    private bool fadingOut = false;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        fadeImage.enabled = true;
        fadingIn = false;
        fadingOut = false;

        // Keep fade canvas alive #fadescreensmatter
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (fadeImage != null && fadeStarted)
        {
            if (fadingIn)
            {
                FadingIn();
            }

            if (fadingOut)
            {
                FadingOut();
            }
        }
    }

    #endregion

    #region Methods

    public void FadeIn()
    {
        fadeImage.enabled = true;
        fadingIn = true;
        fadingOut = false;
        fadeStarted = true;
    }

    public void FadeOut()
    {
        fadeImage.enabled = true;
        fadingOut = true;
        fadingIn = false;
        fadeStarted = true;
    }

    private void FadingIn()
    {
        alpha -= Time.deltaTime * fadeSpeed;

        fadeImage.color = new Color(0, 0, 0, alpha);

        if (alpha <= 0)
        {
            alpha = 0f;

            fadeImage.enabled = false;
            fadeStarted = false;
            fadingIn = false;
        }
    }

    private void FadingOut()
    {
        alpha += Time.deltaTime * fadeSpeed;

        fadeImage.color = new Color(0, 0, 0, alpha);

        if (alpha >= 1)
        {
            fadeStarted = false;
            fadingOut = false;
        }
    }

    #endregion
}