using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Diagnostics;

public class SceneLoader : MonoBehaviour
{
    public GameObject title;
    public GameObject startButton;
    public GameObject optionButton;

    public void LoadSceneAsync(string sceneName)
    {
        CleanupUnusedAssets();
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Loading screen transition for main menu to selection screen
        if (SceneManager.GetActiveScene().name == "Main Menu" && sceneName == "Selection")
        {
            title.GetComponent<Animator>().SetBool("FadeOut", true);
            startButton.GetComponent<Animator>().SetBool("FadeOut", true);
            optionButton.GetComponent<Animator>().SetBool("FadeOut", true);
            
            // Wait until the FadeOut animation completes
            yield return new WaitForSeconds(0.5f);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void CleanupUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }
}