using UnityEngine;
using UnityEngine.UI;

public class FullscreenButtonController : MonoBehaviour
{
    public Image buttonImage;
    public Sprite toFull;
    public Sprite toNormal;
    public RectTransform topBar;
    public RectTransform bottomBar;

    private bool isFullscreen = false; // Track fullscreen state

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen; // Toggle fullscreen state

        // Change the button image based on the state
        buttonImage.sprite = isFullscreen ? toNormal : toFull;

        // Animate top and bottom bars tucking in
        StartCoroutine(TuckBars(isFullscreen));
    }

    private System.Collections.IEnumerator TuckBars(bool tuckIn)
    {
        // Set the target positions for the top and bottom bars
        Vector3 targetTopPosition = tuckIn ? new Vector3(topBar.anchoredPosition.x, topBar.sizeDelta.y, 0) : new Vector3(topBar.anchoredPosition.x, 0, 0);
        Vector3 targetBottomPosition = tuckIn ? new Vector3(bottomBar.anchoredPosition.x, -bottomBar.sizeDelta.y, 0) : new Vector3(bottomBar.anchoredPosition.x, 0, 0);

        float duration = 0.5f; // Duration of the tuck animation
        float elapsed = 0f;

        // Store initial positions
        Vector3 initialTopPosition = topBar.anchoredPosition;
        Vector3 initialBottomPosition = bottomBar.anchoredPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Smooth transition using Lerp
            topBar.anchoredPosition = Vector3.Lerp(initialTopPosition, targetTopPosition, t);
            bottomBar.anchoredPosition = Vector3.Lerp(initialBottomPosition, targetBottomPosition, t);

            yield return null; // Wait for the next frame
        }

        // Ensure final position is set
        topBar.anchoredPosition = targetTopPosition;
        bottomBar.anchoredPosition = targetBottomPosition;
    }
}