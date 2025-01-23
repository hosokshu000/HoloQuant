// Credits to https://www.youtube.com/watch?v=8oTYabhj248

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingAnimation : MonoBehaviour
{
    public GameObject loadingText;
    void Start()
    {
        loadingText.SetActive(false);
    }

    public void Loading()
    {
        loadingText.SetActive(true);
        StartCoroutine(LoadAnimation());
    }

    IEnumerator LoadAnimation()
    {
        while (true)
        {
            loadingText.GetComponent<TextMeshProUGUI>().text = "Loading.";
            yield return new WaitForSeconds(0.4f);
            
            for (int i = 0; i < 3; i++)
            {
                loadingText.GetComponent<TextMeshProUGUI>().text += ".";
                yield return new WaitForSeconds(0.4f);
            }
        }
    }
}
