using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

public class NewCursor : MonoBehaviour
{
    public GameObject cursorChildObject; // Reference for the cursor sprite
    public GameObject actualPrefab; // Reference for the actual object prefab
    public GameObject shadowPrefab; // Reference for the shadow object prefab
    public ARRaycastManager raycastManager;
    public float fadeT; // Fade duration for old objects being removed
    public bool useCursor = true; // Toggle cursor

    public static GameObject actualObj; // Store the animated object instance
    public static GameObject shadowObj; // Store the shadow object instance

    void Start()
    {
        cursorChildObject.SetActive(useCursor);
    }

    void Update()
    {
        if (useCursor)
        {
            UpdateCursor();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Ignore if touch is over any UI buttons
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }

            Vector3 positionOffset = new Vector3(0, 0.02f, 0); // Default offset for position
            Vector3 rotationOffset = new Vector3(90, 0, 0); // Default offset for rotation

            Quaternion adjustedRotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationOffset);

            if (useCursor)
            {
                // Destroy previous objects
                if (actualObj != null)
                {
                    StartCoroutine(FadeAndDestroy(actualObj, fadeT));
                    StartCoroutine(FadeAndDestroy(shadowObj, fadeT));
                }

                // Instantiate prefab at cursor position
                actualObj = Instantiate(actualPrefab, transform.position + positionOffset, adjustedRotation);
                shadowObj = Instantiate(shadowPrefab, transform.position, adjustedRotation);
            }
        }
    }

    void UpdateCursor()
    {
        Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
    }

    IEnumerator FadeAndDestroy(GameObject obj, float fadeDuration)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color initialColor = spriteRenderer.color;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

                spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

                yield return null;
            }

            // Ensure fully transparent
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        }

        // Destroy the object after fading
        Destroy(obj);
    }
}