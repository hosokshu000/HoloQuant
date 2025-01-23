using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Diagnostics;
using System;

public class StepChanger : MonoBehaviour
{
    public static int currentStep = 0; // Current animation step
    public int finalStep; // Step value at the final step of the animation
    public AnimationLoader loader;
    public LessonNarrator narrator;
    public Button nextButton;
    public Button backButton;
    public Animator endScreenAnimator;
    private GameObject actualObj;
    private GameObject shadowObj;
    private VideoPlayer actualPlayer;
    private VideoPlayer shadowPlayer;
    private bool startAtEndOfPrevStep = false; // Determine if the back button is pressed within the first 1 second of an animation step
    private bool backToStartOfStep = false; // Determine if the back button is pressed after the first 1 second of an animation step

    void Start()
    {
        currentStep = 0;
    }

    // Lesson navigation -- loads both the animation and narration for a lesson step
    public void ChangeStep(int diff) // Enter +1 for next button and -1 for back button
    {
        // If the final step is reached, display the end screen
        if (diff == 1 && currentStep == finalStep && LessonNarrator.typingComplete == true && actualPlayer.isPlaying == false)
        {
            endScreenAnimator.SetBool("LessonComplete", true);
            nextButton.interactable = false;
            return;
        }

        if (diff == -1)
        {
            // If the back button is pressed within 1 second of the current step, move back to the end of the previous step
            if (actualPlayer.time <= 1 && shadowPlayer.time <= 1)
            {
                startAtEndOfPrevStep = true;
            }
            else
            {
                backToStartOfStep = true;
            }
        }

        actualObj = ARCursor.actualObj;
        shadowObj = ARCursor.shadowObj;

        actualPlayer = actualObj.GetComponent<VideoPlayer>();
        shadowPlayer = shadowObj.GetComponent<VideoPlayer>();
        
        // Change step only if the video is not restarting the current step
        if (!backToStartOfStep)
        {
            currentStep += diff;
        }

        // Update interactibity of navigation buttons
        if (currentStep <= 1)
        {
            backButton.interactable = false;
            currentStep = 1;
        }
        else if (currentStep >= finalStep)
        {
            currentStep = finalStep;
        }
        else
        {
            nextButton.interactable = true;
            backButton.interactable = true;
        }

        // Load the animation for the new step
        loader.ChangeAnimation(currentStep);
        
        // If the flag is on, skip to the final frame of the previous step
        if (startAtEndOfPrevStep == true)
        {
            if (actualPlayer != null && shadowPlayer != null)
            {
                actualPlayer.frame = (long)actualPlayer.frameCount - 2;
                shadowPlayer.frame = (long)shadowPlayer.frameCount - 2;

                actualPlayer.Play();
                shadowPlayer.Play();
            }
        }

        // Wait for UI fade to complete before loading narration on the first step
        if (currentStep == 1)
        {
            StartCoroutine(Wait(1f));
        }
        else
        {
            narrator.ChangeNarration(currentStep);
        }

        startAtEndOfPrevStep = false;
        backToStartOfStep = false;
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        narrator.ChangeNarration(currentStep);
    }
}