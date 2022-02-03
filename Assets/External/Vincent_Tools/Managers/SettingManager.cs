using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    private static SettingManager _i;
    public static SettingManager i
    {
        get
        {
            if (_i == null)
            {
                _i = FindObjectOfType<SettingManager>();
            }
            return _i;
        }
    }

    //public bool canInit;
    public bool isCamera2D;
    Resolution[] allResolutions;
    List<Resolution> resolutionFor16_9 = new List<Resolution>();


    [SerializeField][Header("Resolution Drop down menu")]
    private Dropdown resolutionDisplay;


    [SerializeField][Header("Volume Slider")]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider soundEffectSlider;
 
    [SerializeField][Header("FullScreen toggle")]
    Toggle fullScreenToggle;
    private int currentResolutionIndex;

    float masterVolume;
    float musicVolume;
    float soundEffectVolume;

    [SerializeField][Header("Audio Mixer")]
    private AudioMixer masterMixer;

    [SerializeField][Header("Audio Mixer output group")]
    private AudioMixerGroup musicOutPutGroup;
    [SerializeField]
    private AudioMixerGroup soundEffectOutPutGroup;

    //This is for 2D game Camera
    //CameraRatio ratio;

    private void Awake()
    {
        //if (canInit)
        //{
        //    allResolutions = Screen.resolutions;
        //    resolutionDisplay.ClearOptions();

        //    List<string> optionsText = new List<string>();

        //    for (int i = 0; i < allResolutions.Length; i++)
        //    {
        //        string resolutions = allResolutions[i].width + "x" + allResolutions[i].height;
        //        optionsText.Add(resolutions);
        //        if (allResolutions[i].width == Screen.currentResolution.width && allResolutions[i].height == Screen.currentResolution.height)
        //        {
        //            currentResolutionIndex = i;
        //        }
        //    }
        //    musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        //    soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume");
        //    masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");

        //    fullScreenToggle.isOn = Screen.fullScreen;


        //    resolutionDisplay.AddOptions(optionsText);
        //    resolutionDisplay.value = currentResolutionIndex;
        //    resolutionDisplay.RefreshShownValue();
        //}

        allResolutions = Screen.resolutions;
        resolutionDisplay.ClearOptions();

        List<string> optionsText = new List<string>();

        for (int i = 0; i < allResolutions.Length; i++)
        {
            string resolutions = allResolutions[i].width + "x" + allResolutions[i].height;
            optionsText.Add(resolutions);
            if (allResolutions[i].width == Screen.currentResolution.width && allResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
 
        fullScreenToggle.isOn = Screen.fullScreen;


        resolutionDisplay.AddOptions(optionsText);
        resolutionDisplay.value = currentResolutionIndex;
        resolutionDisplay.RefreshShownValue();
    }

    private void Start()
    {
        //if (canInit)
        //{
        //    if (isCamera2D)
        //    {
        //        //ratio = FindObjectOfType<CameraRatio>();
        //        resolutionDisplay.onValueChanged.AddListener(delegate
        //        {
        //            //ratio.FitRatio();
        //            SetResolution(resolutionDisplay);

        //        });
        //    }
        //    else {
        //        resolutionDisplay.onValueChanged.AddListener(delegate
        //        {
        //            SetResolution(resolutionDisplay);
        //        });
        //    }

        //    masterMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MusicVolume"));
        //    masterMixer.SetFloat("MusicVolume",PlayerPrefs.GetFloat("MusicVolume"));
        //    masterMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SoundEffectVolume"));

        //    masterSlider.onValueChanged.AddListener(delegate { SetMasterVolume(masterSlider); });
        //    musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(musicSlider); });
        //    soundEffectSlider.onValueChanged.AddListener(delegate { SetSoundEffectVolume(soundEffectSlider); });

        //    fullScreenToggle.onValueChanged.AddListener(delegate
        //    {
        //        SetFullScreen(fullScreenToggle);

        //    });
        //}


        resolutionDisplay.onValueChanged.AddListener(delegate
        {
            SetResolution(resolutionDisplay);
        });

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume");
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");


        masterMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MusicVolume"));
        masterMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        masterMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SoundEffectVolume"));

        masterSlider.minValue = -60f;
        masterSlider.maxValue = 20;
        masterSlider.onValueChanged.AddListener(delegate {
            SetMasterVolume(masterSlider);
          
        });

        musicSlider.minValue = -60f;
        musicSlider.maxValue = 20;
        musicSlider.onValueChanged.AddListener(delegate {
            SetMusicVolume(musicSlider);
       
        });

        soundEffectSlider.minValue = -60f;
        soundEffectSlider.maxValue = 20;
        soundEffectSlider.onValueChanged.AddListener(delegate {
            SetSoundEffectVolume(soundEffectSlider);
            
        });

        fullScreenToggle.onValueChanged.AddListener(delegate
        {
            SetFullScreen(fullScreenToggle);

        });
    }


    public void SetFullScreen(Toggle isFullScreen)
    {
        Screen.fullScreen = isFullScreen.isOn;
    }



    public void SetResolution(Dropdown rsIndex)
    {
        Resolution rs = allResolutions[rsIndex.value];
        Screen.SetResolution(rs.width, rs.height, Screen.fullScreen);
    }

    public void SetMasterVolume(Slider slider)
    {
        masterVolume = slider.value;

        masterMixer.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
    }


    public void SetMusicVolume(Slider slider)
    {
        musicVolume = slider.value;
      
        masterMixer.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSoundEffectVolume(Slider slider)
    {
        soundEffectVolume = slider.value;
        
        masterMixer.SetFloat("SFXVolume", soundEffectVolume);
        PlayerPrefs.SetFloat("SoundEffectVolume", soundEffectVolume);
    }

    public AudioMixerGroup GetMusicAudioMixer()
    {
        return musicOutPutGroup;
    }

    public AudioMixerGroup GetSoundEffectAudioMixer()
    {
        return soundEffectOutPutGroup;
    }
}
