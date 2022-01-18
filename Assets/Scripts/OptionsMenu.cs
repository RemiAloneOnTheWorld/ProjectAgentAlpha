using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionList;
    public GameObject VolumeText;
    public GameObject SensitivityText;
    public GameObject SensitivityP2Text;
    Resolution[] resolutions;


    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionList.ClearOptions();

        List<string> resolutionsString = new List<string>();

        int currentResolution = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolution = resolutions[i].width + " x " + resolutions[i].height;
            resolutionsString.Add(resolution);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        resolutionList.AddOptions(resolutionsString);
        resolutionList.value = currentResolution;
        resolutionList.RefreshShownValue();

        float s1 = PlayerPrefs.GetFloat("P1Sensitivity");
        float s2 = PlayerPrefs.GetFloat("P2Sensitivity");

        if (s1 != 0)
        {
            TextMeshProUGUI sText = SensitivityText.GetComponent<TextMeshProUGUI>();
            sText.text = s1.ToString();
        }

        if (s2 != 0)
        {
            TextMeshProUGUI s2Text = SensitivityP2Text.GetComponent<TextMeshProUGUI>();
            s2Text.text = s2.ToString();
        }

    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        float percentage = ((volume + 80) / 80) * 100;
        int pInt = (int)percentage;
        TextMeshProUGUI vText = VolumeText.GetComponent<TextMeshProUGUI>();
        vText.text = pInt.ToString() + "%";
    }

    public void SetResolution(int resolutionI)
    {
        Resolution resolution = resolutions[resolutionI];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("P1Sensitivity", sensitivity);
        TextMeshProUGUI sText = SensitivityText.GetComponent<TextMeshProUGUI>();
        sText.text = sensitivity.ToString();
    }

    public void SetP2Sensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("P2Sensitivity", sensitivity);
        TextMeshProUGUI s2Text = SensitivityP2Text.GetComponent<TextMeshProUGUI>();
        s2Text.text = sensitivity.ToString();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}