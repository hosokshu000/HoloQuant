using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Animator buttonAnimator;
    public Animator panelAnimator;

    public void OptionToggle()
    {        
        buttonAnimator.SetBool("ButtonPressed", true);
        panelAnimator.SetBool("ButtonPressed", true);

        bool OptionOpen = buttonAnimator.GetBool("OptionOpen");

        if (!(OptionOpen && SliderUtils.sliderTouched))
        {
            buttonAnimator.SetBool("OptionOpen", !OptionOpen);
            panelAnimator.SetBool("OptionOpen", !OptionOpen);
        }
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
