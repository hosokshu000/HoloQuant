using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class GitHubLinkNavigator : MonoBehaviour
{
    public void GoToGitHub()
     {
         Application.OpenURL("https://github.com/hosokshu000/HoloQuant");
         XcodeLog("GitHub link opened");
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
