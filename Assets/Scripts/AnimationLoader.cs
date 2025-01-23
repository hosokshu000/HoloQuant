using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;
using UnityEngine.Video;
using UnityEngine.UI;

public class AnimationLoader : MonoBehaviour
{
    private GameObject actualObj;
    private GameObject shadowObj;
    private static string fullPath; // Full path to the lesson animation folder
    public static string lessonPath; // Lesson animation path relative to the streaming assets folder (Lesson name)
    public LessonNarrator narrator;
    public Slider slider;
    void Start()
    {
        fullPath = Path.Combine(Application.streamingAssetsPath, "Animations", lessonPath);
    }

    public void SetLesson(string lessonName)
    {
        lessonPath = lessonName;
        fullPath = Path.Combine(Application.streamingAssetsPath, "Animations", lessonPath);
        narrator.ChangeLesson(lessonName);
    }

    public void ChangeAnimation(int step)
    {
        actualObj = ARCursor.actualObj;
        shadowObj = ARCursor.shadowObj;

        // Changing animation for actualObj
        VideoPlayer actualVideo = actualObj.GetComponent<VideoPlayer>();

        string actualVideoPath = Path.Combine(fullPath, "Actual"); // Path to the animation files for actualObj
        string stepName = step.ToString() + ".webm"; // Animation file name to search for
        string clipPath = Path.Combine(actualVideoPath, stepName);

        if (File.Exists(clipPath))
        {
            if (step == 0)
            {
                StartCoroutine(Transition(actualObj, clipPath, true));
            }
            else
            {
                StartCoroutine(Transition(actualObj, clipPath));
            }
        }
        else
        {
            XcodeLog($"File {clipPath} not found!");
            return;
        }


        // Changing animation for shadowObj
        VideoPlayer shadowVideo = shadowObj.GetComponent<VideoPlayer>();

        string shadowVideoPath = Path.Combine(fullPath, "Shadow"); // Path to the animation files for shadowObj
        clipPath = Path.Combine(shadowVideoPath, stepName);

        if (File.Exists(clipPath))
        {
            if (step == 0)
            {
                StartCoroutine(Transition(shadowObj, clipPath, true));   
            }
            else
            {
                StartCoroutine(Transition(shadowObj, clipPath));
            }
        }
        else
        {
            XcodeLog($"File {clipPath} not found!");
            return;
        }
    }


    // Ensure smooth transition
    private IEnumerator Transition(GameObject obj, string newClipPath, bool initializing = false)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        VideoPlayer videoPlayer = obj.GetComponent<VideoPlayer>();

        if (renderer != null && videoPlayer != null)
        {
            if (initializing)
            {
                renderer.enabled = false;
            }

            // Store the current texture if transitioning from a previous step
            RenderTexture currentTexture = null;

            if (!initializing && videoPlayer.texture != null)
            {
                currentTexture = RenderTexture.GetTemporary(videoPlayer.texture.width, videoPlayer.texture.height, 0);
                Graphics.Blit(videoPlayer.texture, currentTexture); // Define currentTexture as the last frame of the previous step animation

                renderer.material.mainTexture = currentTexture; // Apply the last frame as a placeholder texture until the next animation loads
            }

            // Disable renderer to prevent white flash from appearing while video loads
            videoPlayer.Stop();
            
            videoPlayer.url = newClipPath; // Set video url to the path for the new step animation
            yield return null;

            // Prepare the next animation
            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }
            
            videoPlayer.Play();
            while (videoPlayer.frame < 1)
            {
                yield return null;
            }


            // Switch to the new video texture and reenable renderer
            renderer.material.mainTexture = videoPlayer.targetTexture;
            renderer.enabled = true;

            // Clean up temporary texture
            if (currentTexture != null)
            {
                RenderTexture.ReleaseTemporary(currentTexture);
            }
        }
        else
        {
            XcodeLog("Missing renderer or video player!");
        }
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
