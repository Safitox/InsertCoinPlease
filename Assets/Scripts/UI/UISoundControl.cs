using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISoundControl : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Slider sliMaster;
    [SerializeField] Slider sliMusic;
    [SerializeField] Slider sliSound;
    [SerializeField] Slider sliVoice;

    private void Start()
    {
        sliMaster.value= masterMixer.GetFloat("MasterVolume", out float masterVol) ? Mathf.Pow(10f, masterVol / 20f) : 0f;
        sliMusic.value= masterMixer.GetFloat("MusicVolume", out float musicVol) ? Mathf.Pow(10f, musicVol / 20f) : 0f;
        sliSound.value= masterMixer.GetFloat("SoundVolume", out float soundVol) ? Mathf.Pow(10f, soundVol / 20f) : 0f;
        sliVoice.value= masterMixer.GetFloat("VoiceVolume", out float voiceVol) ? Mathf.Pow(10f, voiceVol / 20f) : 0f;

    }



    public void ChangeMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void ChangeMusicVolume(float volume)
    {
        //masterMixer.SetFloat("MusicVolume", volume);
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
    public void ChangeSoundVolume(float volume)
    {
        masterMixer.SetFloat("SoundVolume", Mathf.Log10(volume) * 20);
    }
    public void ChangeVoiceVolume(float volume)
    {
        masterMixer.SetFloat("VoiceVolume", Mathf.Log10(volume) * 20);
    }
}
