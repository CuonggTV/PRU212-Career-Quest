using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public TMP_Dropdown graphicDropdown;
    public Slider masterSlider, musicSlider, sfxSlider;
    public AudioMixer mainAudioMixer;
    
    public void ChangeGraphicDropdown()
    {
        QualitySettings.SetQualityLevel(graphicDropdown.value);
    }
    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVolume", masterSlider.value);
    }
    public void ChangeMusicVolume()
    {
        mainAudioMixer.SetFloat("MusicVolume", musicSlider.value);
    }
    public void ChangeSFXVolume()
    {
        mainAudioMixer.SetFloat("SFXVolume", sfxSlider.value);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
