using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEditor;
using TMPro;
using System.IO;

public class LessonNarrator : MonoBehaviour
{
    public GameObject tbObj; // Object with the text box
    public GameObject startButton; // Lesson start button
    public GameObject contArrow; // Continue arrow
    public static float narrationSpeed = 0.02f; // Narration speed in sec/character
    public static string lessonPath; // Path to narration text file relative to /StreamingAssets/NarrationText (Lesson name)
    public StreamReader reader; // Text file reader
    public int step = 1; // Store step of the lesson animation
    public static bool typingComplete = false; // Flag to signal narration completion for a step
    private bool cont = false; // Queue to continue narration
    private TextMeshProUGUI textBox; // Narration text box on the front end
    private List<string> narrationText = new List<string>(); // Load and store narration text for a step of the lesson
    private static string fullPath;
    private Coroutine currentCoroutine;

    void Start()
    {
        narrationSpeed = 0.02f;
        contArrow.SetActive(false);
        textBox = tbObj.GetComponent<TextMeshProUGUI>();
        fullPath = Path.Combine(Application.streamingAssetsPath, "NarrationText", lessonPath);
        textBox.text = ""; // Initially store an empty string
    }

    public void ChangeLesson(string newName)
    {
        lessonPath = newName + ".txt";
        fullPath = Path.Combine(Application.streamingAssetsPath, "NarrationText", lessonPath);
    }

    public void ChangeNarration(int step)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        textBox.text = "";
        narrationText.Clear();
        typingComplete = false;
        contArrow.SetActive(false);
        LoadAndInitializeList(step);
        currentCoroutine = StartCoroutine(TypeText());
    }

    void LoadAndInitializeList(int step) // Specify step, dynamically load and unload narration text for each step
    {        
        // Close any existing readers
        if (reader != null)
        {
            reader.Close();
        }
        reader = new StreamReader(fullPath);

        string line = reader.ReadLine();

        while (line != null) // Continue reading until the end of the file
        {
            if (line == step.ToString()) // Check if line of the file has the step number (each paragraph of text for a step should begin with its steps number)
            {
                line = reader.ReadLine();

                while(line != " ") // Continue to add to the list line by line until the reader hits a blank line with a single space
                {
                    narrationText.Add(line);
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        XcodeLog("Reached end of file!");
                        break;
                    }
                }
                return; // Break once the narrationText list is filled
            }
            line = reader.ReadLine(); // Move to read next line if line does not match the step number
        }
        XcodeLog($"Narration text for {step} not found!");
        reader.Close();
    }

    // Iterate and add more characters to the text in the textbox
    IEnumerator TypeText()
    {
        foreach (string s in narrationText.ToArray())
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\\') // Use '\' in the text file to denote a new narration box (the current text will be cleared for continuation once the continue arrow is pressed)
                {
                    cont = false;
                    contArrow.SetActive(true);
                    yield return new WaitUntil(() => cont);
                    contArrow.SetActive(false);
                    textBox.text = "";
                }
                else if (s[i] == '<') // Detecting rich text tags
                {
                    int tagEnd = s.LastIndexOf('>');
                    if (tagEnd != -1) // If a valid tag is found
                    {
                        string richTextTag = s.Substring(i, tagEnd - i + 1);
                        textBox.text += richTextTag; // Add the whole tag at once
                        i = tagEnd;
                        continue;
                    }
                }
                else
                {
                    textBox.text += s[i];
                }
                yield return new WaitForSeconds(narrationSpeed);
            }
        }

        typingComplete = true;
        currentCoroutine = null;
    }

    public void Continue()
    {
        cont = true;
    }

    [Conditional("UNITY_IOS")]
    private void XcodeLog(string message)
    {
        // This will appear in Xcode's console
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + "/XcodeLog.txt");
        System.IO.File.AppendAllText(Application.persistentDataPath + "/XcodeLog.txt", message + "\n");
        
        // This will still appear in Unity's console during development
        UnityEngine.Debug.Log(message);
    }
}
