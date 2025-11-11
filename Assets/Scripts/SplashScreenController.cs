using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenController : MonoBehaviour
{
    public Image fadePanel;
    public Image splashImage;
    public float fadeDuration = 1f;
    public string nextSceneName = "Main Menu";

    void Start()
    {
        StartCoroutine(PlaySplashScreen());
    }

    IEnumerator PlaySplashScreen()
    {
        yield return StartCoroutine(Fade(fadePanel, 1, 0));

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(Fade(fadePanel, 0, 1));

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
