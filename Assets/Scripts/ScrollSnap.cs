// Credits to https://www.youtube.com/watch?v=3ruhLJsb0tk

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollSnap : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public RectTransform sampleListItem;

    public HorizontalLayoutGroup HLG;
    private bool isSnapped;
    private float snapSpeed;
    public float snapForce;

    void Start()
    {
        isSnapped = false;
    }

    void Update()
    {
        int currentItem = Mathf.RoundToInt(-contentPanel.localPosition.x / (sampleListItem.rect.width + HLG.spacing));

        if (scrollRect.velocity.magnitude < 300 && !isSnapped)
        {
            scrollRect.velocity = Vector2.zero;
            snapSpeed += snapForce * Time.deltaTime;
            contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(contentPanel.localPosition.x, -currentItem * (sampleListItem.rect.width + HLG.spacing), snapSpeed),
                contentPanel.localPosition.y,
                contentPanel.localPosition.z
            );
            if (contentPanel.localPosition.x == -currentItem * (sampleListItem.rect.width + HLG.spacing))
            {
                isSnapped = true;
                snapSpeed = 0;
            }
        }
        if (scrollRect.velocity.magnitude > 300 || contentPanel.localPosition.x != -currentItem * (sampleListItem.rect.width + HLG.spacing))
        {
            isSnapped = false;
        }
    }
}