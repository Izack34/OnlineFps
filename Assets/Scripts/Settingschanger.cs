using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settingschanger : MonoBehaviour
{
    GameObject settings;
    public string New_name;
    public static string UserName;
    static float volume_s;

    public AudioMixer Audiomixer;
    public Slider Volumeslider;
    public Dropdown resolutionList;
    Resolution[] resolutions;

    List<string> options = new List<string>();
    void Start()
    {
        //settings = GameObject.Find("Settings");
        Screen.fullScreen = true;
        resolutions = Screen.resolutions;
        Volumeslider.value = volume_s;

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + " Hz";
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionList.AddOptions(options);
        resolutionList.value = currentResolutionIndex;
        resolutionList.RefreshShownValue();
    }

    public void SetUserName(string name){
        UserName = name;
        //settings.GetComponent<UserSettings>().User_name = New_name;
    }

    public void SetVolume(float volume){
        volume_s = volume;
        Audiomixer.SetFloat("Mastervolume", volume);
    }

    public void SetFullscreen(bool isFullscreen){
        Debug.Log("changed");
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int ResIndex){
        Resolution resolution = resolutions[ResIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
