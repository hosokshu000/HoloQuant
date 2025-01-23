using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    public Image fadePanel; // Assign this in the Inspector
    public Image splashImage; // Assign this in the Inspector
    public float fadeDuration = 1f;
    public string nextSceneName = "Main Menu"; // Name of the home scene

    void Start()
    {
        StartCoroutine(PlaySplashScreen());
    }

    IEnumerator PlaySplashScreen()
    {
        // Fade-in effect
        yield return StartCoroutine(Fade(fadePanel, 1, 0));

        // Wait for a few seconds (or as long as needed)
        yield return new WaitForSeconds(2f);

        // Fade-out effect
        yield return StartCoroutine(Fade(fadePanel, 0, 1));

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
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
