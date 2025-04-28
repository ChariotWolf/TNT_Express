using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSizeAdjuster : MonoBehaviour
{
    void Start()
    {
        int screenWidth = Display.main.systemWidth;
        int screenHeight = Display.main.systemHeight;

        float targetAspect = 9f / 16f;


        int maxAllowedHeight = screenHeight - 100;


        int newHeight = Mathf.Min(1920, maxAllowedHeight);
        int newWidth = Mathf.RoundToInt(newHeight * targetAspect);

        Screen.SetResolution(newWidth, newHeight, false);
    }
}
