// Credits to https://www.youtube.com/watch?v=DCndoQFN344

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public RectTransform contentPanelTransform;
    public HorizontalLayoutGroup HLG;

    public RectTransform[] ItemList;

    Vector2 OldVelocity;
    bool isUpdated;

    void Start()
    {
        isUpdated = false;
        OldVelocity = Vector2.zero;

        int ItemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / (ItemList[0].rect.width + HLG.spacing));

        for (int i = 0; i < ItemsToAdd; i++)
        {
            RectTransform RT = Instantiate(ItemList[i % ItemList.Length], contentPanelTransform);
            RT.SetAsLastSibling();
        }

        for (int i = 0; i < ItemsToAdd; i++)
        {
            int num = ItemList.Length - i - 1;
            while (num < 0)
            {
                num += ItemList.Length;
            }
            RectTransform RT = Instantiate(ItemList[num], contentPanelTransform);
            RT.SetAsFirstSibling();
        }

        contentPanelTransform.localPosition = new Vector3((0 - (ItemList[0].rect.width + HLG.spacing) * ItemsToAdd),
            contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);

    }

    void Update()
    {
        if (isUpdated)
        {
            isUpdated = false;
            scrollRect.velocity = OldVelocity;
        }

        if(contentPanelTransform.localPosition.x > 0)
        {
            Canvas.ForceUpdateCanvases();
            OldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition -= new Vector3(ItemList.Length * (ItemList[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }

        if(contentPanelTransform.localPosition.x < 0 - (ItemList.Length * (ItemList[0].rect.width + HLG.spacing)))
        {
            Canvas.ForceUpdateCanvases();
            OldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition += new Vector3(ItemList.Length * (ItemList[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }
    }
}
