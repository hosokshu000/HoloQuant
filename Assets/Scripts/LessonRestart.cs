using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LessonRestart : MonoBehaviour
{
    public StepChanger stepChanger;
    public Animator endScreenAnimator;
    public LoadingAnimation loader;

    public void RestartLesson()
    {
        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        endScreenAnimator.SetBool("LessonComplete", false);
        endScreenAnimator.SetBool("RestartLesson", true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("AR Scene");  // Reload lesson

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        endScreenAnimator.SetBool("RestartLesson", false);
    }
}
