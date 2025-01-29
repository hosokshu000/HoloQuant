using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class URLOpener : MonoBehaviour
{
    public void OpenURL(string link)
     {
         Application.OpenURL(link);
     }

     public void OpenGmail()
     {
        #if UNITY_IOS
            try
            {
                // Open Gmail app
                Application.OpenURL("googlegmail:///co?to=holoquant.app@gmail.com");
            }
            catch
            {
                // If Gmail app is not installed, open App Store
                Application.OpenURL("https://apps.apple.com/app/gmail-email-by-google/id422689480");
            }
        #endif
     }
}
