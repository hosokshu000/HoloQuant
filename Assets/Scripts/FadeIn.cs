using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public GameObject obj; // Reference to object to fade in
    public float fadeDuration; // Duration of the fade-in effect
    public float delay; // Delay before the object appears
    public bool isCursor = false; // Script behaves differently for ARCursor
    private Color col;
    private SpriteRenderer spriteRenderer; // SpriteRenderer component for 2D sprite
    private Image img; // Image component for buttons and UI elements

    void Start()
    {
        // Activate obj
       obj.SetActive(true);

        // Initialize the alpha to 0 (completely transparent)
        if (isCursor)
        {
            spriteRenderer = obj.GetComponent<SpriteRenderer>();
            col = spriteRenderer.color;
            col.a = 0f;
            spriteRenderer.color = col;
            StartFade(); // Load AR Cursor upon start
        }
        else
        {
            img = obj.GetComponent<Image>();

            if (img != null)
            {
                col = img.color;
                col.a = 0f;
                img.color = col;

                if (obj.GetComponent<Button>() == null)
                {
                    return;
                }

                // Set buttons to be uninteractable
                obj.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void StartFade(bool isBackButton =  false)
    {
        // Ensure the component is found
        if (spriteRenderer != null || img != null)
        {
            // Start the fade coroutine
            StartCoroutine(ActivateAndFade(isBackButton));
        }
        else
        {
            Debug.LogError("No SpriteRenderer/Image found on object.");
        }
    }

    IEnumerator ActivateAndFade(bool backButton)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Fade in obj over the duration
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the alpha value based on time
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            // Set the alpha value of the sprite
            col.a = alpha;

            if (isCursor)
            {
                spriteRenderer.color = col;
            }
            else if (img != null)
            {
                img.color = col;
            }

            yield return null;
        }

        // Ensure obj is fully visible after the fade
        col.a = 1f;

        if (isCursor)
        {
            spriteRenderer.color = col;
        }
        else if (img != null)
        {
            img.color = col;

            // Ensure buttons are interactable (except the back button)
            if (obj.GetComponent<Button>() != null && !backButton)
            {
                obj.GetComponent<Button>().interactable = true;
            }
        }
    }
}
