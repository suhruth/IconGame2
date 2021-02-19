using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Text ShowDeviceName;

    private void Start()
    {
        ShowDeviceName.text = isMobile().ToString();
    }

    [DllImport("__Internal")]
    private static extern bool IsMobile();

    public bool isMobile()
    {
#if UNITY_EDITOR //&& UNITY_WEBGL
             return IsMobile();
#endif
        return false;
    }
}
