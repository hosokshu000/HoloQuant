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
}
