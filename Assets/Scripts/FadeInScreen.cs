using UnityEngine;
using UnityEngine.UI;

public class FadeInScreen : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 0.01f;

    private float alpha = 1f;

    void Update()
    {
        if (fadeImage != null)
        {
            alpha -= Time.deltaTime * fadeSpeed;

            fadeImage.color = new Color(0, 0, 0, alpha);

            if (alpha <= 0)
            {
                Destroy(fadeImage.gameObject);
            }
        }
    }
}