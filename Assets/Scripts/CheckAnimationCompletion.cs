using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CheckAnimationCompletion : MonoBehaviour
{
    private GameObject obj; // Either instantiated object (either actual or shadow)
    public Animator nextAnimator; // Next button animator component

    void Update()
    {
        obj = ARCursor.actualObj;

        if (obj != null)
        {
            VideoPlayer player = obj.GetComponent<VideoPlayer>();

            // Check if animation step is complete and activate next button's bobbing animation
            if (player.isPlaying == false && LessonNarrator.typingComplete == true)
            {
                nextAnimator.SetBool("StartBobbing", true);
            }
            else
            {
                nextAnimator.SetBool("StartBobbing", false);
            }
        }
    }
}
