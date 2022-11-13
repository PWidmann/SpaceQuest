using UnityEngine;
using UnityEngine.UI;

public class FadeInScreen : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 0.01f;

    private float alpha = 1f;

    public bool fadeStarted = false;

    public void FadeIn()
    {
        if (fadeStarted)
        {
            fadeImage.enabled = true;

            alpha -= Time.deltaTime * fadeSpeed;

            fadeImage.color = new Color(0, 0, 0, alpha);

            if (alpha <= 0)
            {
                Destroy(fadeImage.gameObject);
            }
        }
        
    }

    void Update()
    {
        if (fadeImage != null)
        {
            FadeIn();
        }
    }
}