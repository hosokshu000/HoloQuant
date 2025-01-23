using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuOptions : MonoBehaviour
{
    public GameObject title;
    public GameObject startButton;
    public GameObject optionButton;
    public GameObject logo;
    public GameObject optionsPanel;
    private List<GameObject> childObjects;

    void Start ()
    {
        childObjects = GetAllChildren(optionsPanel);

        ToggleOptionPanelStatus(false);
    }

    public void ToOptions()
    {
        title.GetComponent<Animator>().SetBool("FadeOut", true);
        startButton.GetComponent<Animator>().SetBool("FadeOut", true);
        optionButton.GetComponent<Animator>().SetBool("FadeOut", true);
        logo.GetComponent<Animator>().SetBool("FadeOut", true);

        WaitAndLoadOptions();
    }

    public void ReturnToHome()
    {
        ToggleOptionPanelStatus(false);

        title.GetComponent<Animator>().SetBool("FadeOut", false);
        startButton.GetComponent<Animator>().SetBool("FadeOut", false);
        optionButton.GetComponent<Animator>().SetBool("FadeOut", false);
        logo.GetComponent<Animator>().SetBool("FadeOut", false);
    }

    void ToggleOptionPanelStatus(bool status)
    {
        optionsPanel.SetActive(status);

        foreach (GameObject obj in childObjects)
        {
            obj.SetActive(status);
        }
    }

    List<GameObject> GetAllChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
            // Recursively add the child's children
            children.AddRange(GetAllChildren(child.gameObject));
        }

        return children;
    }

    void WaitAndLoadOptions()
    {
        StartCoroutine(WaitForAnimationCompletion());
    }

    IEnumerator WaitForAnimationCompletion()
    {
        yield return new WaitForSeconds(0.667f);
        ToggleOptionPanelStatus(true);
    }
}
