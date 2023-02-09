using UnityEngine;
using UnityEngine.UI;

public class FadeInScreen : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 0.01f;

    public float alpha = 1f;

    public bool fadeStarted = false;

    public void FadeIn()
    {
        fadeImage.enabled = true;

        alpha -= Time.deltaTime * fadeSpeed;

        fadeImage.color = new Color(0, 0, 0, alpha);

        if (alpha <= 0)
        {
            alpha = 0f;

            fadeImage.enabled = false;
            fadeStarted = false;
        }
    }

    void Update()
    {
        if (fadeImage != null && fadeStarted)
        {
            FadeIn();
        }
    }
}