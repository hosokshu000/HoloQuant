using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Transitioner : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 3f;
    void Start()
    {
        StartCoroutine(PlaySplashScreen());
    }

    IEnumerator PlaySplashScreen()
    {
        // Fade-in effect
        yield return StartCoroutine(Fade(fadePanel, 1, 0));
        
        // Set Panel Inactive
        fadePanel.gameObject.SetActive(false);
    }

    IEnumerator Fade(Image image, float startAlpha, float endAlpha)
    {
        Color color = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }

        // Ensure the final alpha is set
        color.a = endAlpha;
        image.color = color;
    }
}
