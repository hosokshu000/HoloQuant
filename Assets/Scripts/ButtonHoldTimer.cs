using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHoldTimer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float holdDuration = 1f; // Duration in seconds before timeout
    public UnityEvent onHoldTimeout; // Event triggered when hold times out
    public UnityEvent onNormalClick; // Event for normal clicks

    private bool isHolding = false;
    private float holdTimer = 0f;
    private bool hasTimedOut = false;

    private void Update()
    {
        if (isHolding && !hasTimedOut)
        {
            holdTimer += Time.deltaTime;
            
            if (holdTimer >= holdDuration)
            {
                hasTimedOut = true;
                onHoldTimeout.Invoke();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        holdTimer = 0f;
        hasTimedOut = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isHolding && !hasTimedOut && holdTimer < holdDuration)
        {
            onNormalClick.Invoke();
        }
        
        isHolding = false;
        holdTimer = 0f;
    }
}