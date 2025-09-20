using UnityEngine;

public class PortraitWindowSizer : MonoBehaviour
{
    void Start()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        int maxH = Screen.currentResolution.height;
        int h = Mathf.Min(1920, maxH - 120);
        int w = Mathf.RoundToInt(h * 9f / 16f); 
        if (w < 540) { w = 540; h = Mathf.RoundToInt(540 * 16f / 9f); } 
        Screen.SetResolution(w, h, false);
    }
}
