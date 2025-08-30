using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resolutionTxt;
    [SerializeField] private TextMeshProUGUI frameRateTxt;
    [SerializeField] private TextMeshProUGUI fullScreenTxt;

    private Resolution[] resolutions;
    private FullScreenMode[] screenModes = { FullScreenMode.Windowed, FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow };
    private int[] frameRates = { 30, 60, 120, -1 };
    private int currentResolutionNum;
    private int currentScreenMode;
    private int currentFrame;

    private void Start()
    {
        resolutions = Screen.resolutions;

        currentResolutionNum = resolutions.Length - 1;
        currentScreenMode = 0;
        currentFrame = 0;

        Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
        resolutionTxt.text = resolutions[resolutions.Length - 1].ToString().Split('@')[0];

        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        fullScreenTxt.text = Screen.fullScreenMode.ToString();

        Application.targetFrameRate = frameRates[currentFrame];
        frameRateTxt.text = Application.targetFrameRate + "FPS";
    }

    public void SetResolution(int num)
    {
        currentResolutionNum += num;

        if (currentResolutionNum > resolutions.Length - 1) currentResolutionNum = 0;
        else if (currentResolutionNum < 0) currentResolutionNum = resolutions.Length - 1;

        Resolution resolution = resolutions[currentResolutionNum];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionTxt.text = resolution.ToString().Split('@')[0];
    }

    public void SetFrameRate(int num)
    {
        currentFrame += num;

        if (currentFrame > frameRates.Length - 1) currentFrame = 0;
        else if (currentFrame < 0) currentFrame = frameRates.Length - 1;

        Application.targetFrameRate = frameRates[currentFrame];

        if (currentFrame == 3) frameRateTxt.text = "No Limit";
        else frameRateTxt.text = Application.targetFrameRate.ToString() + "FPS";
    }

    public void SetFullScreen(int num)
    {
        currentScreenMode += num;

        if (currentScreenMode > screenModes.Length - 1) currentScreenMode = 0;
        else if (currentScreenMode < 0) currentScreenMode = screenModes.Length - 1;

        Screen.fullScreenMode = screenModes[currentScreenMode];
        fullScreenTxt.text = Screen.fullScreenMode.ToString();
    }
}
