using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider sfxSlider;
    
    private string MUSIC_VOL = "ExposedMusicVolume";
    private string SFX_VOL   = "ExposedSFXVolume";

    void Start()
    {
        LoadVolumeSettings();
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(MUSIC_VOL, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(MUSIC_VOL, value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(SFX_VOL, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(SFX_VOL, value);
    }

    void LoadVolumeSettings()
    {
        float music  = PlayerPrefs.GetFloat(MUSIC_VOL, 1f);
        float sfx    = PlayerPrefs.GetFloat(SFX_VOL, 1f);

        SetMusicVolume(music);
        SetSFXVolume(sfx);

        if (musicSlider != null) musicSlider.value = music;
        if (sfxSlider != null) sfxSlider.value = sfx;

        //audioMixer.SetFloat(MUSIC_VOL, Mathf.Log10(music) * 20);
        //audioMixer.SetFloat(SFX_VOL, Mathf.Log10(sfx) * 20);
    }
}
