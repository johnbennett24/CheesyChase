using UnityEngine;

public class TargetFrameRate : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }
}
