using UnityEngine;

public class TargetFramerate
{
    void Start()
    {
        // Limits framerate to 60 and disables vSync
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = -1;
    }
}