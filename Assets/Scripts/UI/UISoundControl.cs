using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UISoundControl : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Slider sliMusic;
    [SerializeField] Slider sliSound;

    private void Start()
    {
        sliMusic.value= masterMixer.GetFloat("MusicVolume", out float musicVol) ? Mathf.Pow(10f, musicVol / 20f) : 0f;
        sliSound.value= masterMixer.GetFloat("SoundVolume", out float soundVol) ? Mathf.Pow(10f, soundVol / 20f) : 0f;

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
}
