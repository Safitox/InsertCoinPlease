using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISoundControl : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Slider sliMusic;
    [SerializeField] Slider sliSound;

    private void Start()
    {
        sliMusic.value= masterMixer.GetFloat("MusicVolume", out float musicVol) ? musicVol : 0f;
        sliSound.value= masterMixer.GetFloat("SoundVolume", out float soundVol) ? soundVol : 0f;
    }



    public void ChangeMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicVolume", volume);
    }
    public void ChangeSoundVolume(float volume)
    {
        masterMixer.SetFloat("SoundVolume", volume);
    }
}
