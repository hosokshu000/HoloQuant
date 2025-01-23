using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.Video;
using System.Diagnostics;

public class SliderUtils : MonoBehaviour
{
    public Slider slider;
    public static bool sliderTouched = false;
    public TextMeshProUGUI label; // Textbox label for the animation speed
    private GameObject actualObj;
    private GameObject shadowObj;
    private VideoPlayer actualPlayer;
    private VideoPlayer shadowPlayer;
    private float[] snapValues = {0.5f, 0.75f, 1.0f, 1.25f, 1.5f, 1.75f, 2.0f};
    
    void Start()
    {
        Time.timeScale = 1.0f;
        slider.value = 1.0f; // Set initial value to 1
        label.text = slider.value.ToString();
    }

    public void SnapSlider()
    {
        float currentValue = slider.value;
        float closestSnapValue = 1.0f; // Store the closest value to currentValue in snapValues
        float diff = float.MaxValue; // Store difference between currentValue and snapValues elements

        foreach (float value in snapValues)
        {
            if (Math.Abs(currentValue - value) <= diff)
            {
                diff = Math.Abs(currentValue - value);
                closestSnapValue = value;
            }
        }

        slider.value = closestSnapValue;
        label.text = slider.value.ToString();
    }

    public void SetAnimationSpeed()
    {
        if (ARCursor.actualObj != null && ARCursor.shadowObj != null)
        {
            actualObj = ARCursor.actualObj;
            shadowObj = ARCursor.shadowObj;

            if (actualObj.GetComponent<VideoPlayer>() != null && shadowObj.GetComponent<VideoPlayer>() != null)
            {
                actualPlayer = actualObj.GetComponent<VideoPlayer>();
                shadowPlayer = shadowObj.GetComponent<VideoPlayer>();
            }
        }

        if (actualPlayer != null && shadowPlayer != null)
        {            
            actualPlayer.playbackSpeed = slider.value;
            shadowPlayer.playbackSpeed = slider.value;
        }
        else
        {
            XcodeLog("Player not found");
        }
    }

    public void SetNarrationSpeed()
    {
        LessonNarrator.narrationSpeed = 0.02f / (float)(slider.value);
    }

    public void SetScale()
    {
        if (ARCursor.actualObj != null && ARCursor.shadowObj != null)
        {
            actualObj = ARCursor.actualObj;
            shadowObj = ARCursor.shadowObj;
        }

        if (actualObj != null && shadowObj != null)
        {
            actualObj.transform.localScale = new Vector3(0.1f * slider.value, 0.1f * slider.value, 0.1f * slider.value);
            shadowObj.transform.localScale = new Vector3(0.1f * slider.value, 0.1f * slider.value, 0.1f * slider.value);
        }
    }

    public void CheckSliderTouch()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            sliderTouched = true;
        }
        else
        {
            sliderTouched = false;
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