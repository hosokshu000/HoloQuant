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
            Animator titleAnimator = title.GetComponent<Animator>();
            while (titleAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeOut") && 
                titleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }
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

    [Conditional("UNITY_IOS")]
    private void XcodeLog(string message)
    {
        // This will appear in Xcode's console
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + "/XcodeLog.txt");
        System.IO.File.AppendAllText(Application.persistentDataPath + "/XcodeLog.txt", message + "\n");
        
        // This will still appear in Unity's console during development
        UnityEngine.Debug.Log(message);
    }
}