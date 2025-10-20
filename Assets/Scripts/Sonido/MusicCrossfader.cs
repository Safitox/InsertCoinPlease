using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicCrossfader : MonoBehaviour
{

    public float defaultFade = 2f;

    public AudioSource active, idle;

    void Awake()
    {
        // Inicialización
        idle.volume = 0f;
    }

    private void Start()
    {
        active = GameObject.Find("Music").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            CrossfadeTo(defaultFade);
        }
    }

    public void PlayImmediate(float volume = 1f)
    {
        active.volume = volume;
        CrossfadeTo(3f);
    }

    public void CrossfadeTo(float fadeSeconds = -1f)
    {
        if (fadeSeconds <= 0f) fadeSeconds = defaultFade;


        double startDsp = AudioSettings.dspTime + 0.05; // pequeño offset
        idle.PlayScheduled(startDsp);

        StopAllCoroutines();
        StartCoroutine(FadeRoutine(fadeSeconds, startDsp));
    }

    IEnumerator FadeRoutine(float fadeSeconds, double startDsp)
    {
        float t = 0f;
        float startVolActive = active.volume;

        while (AudioSettings.dspTime < startDsp) yield return null;

        while (t < fadeSeconds)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeSeconds);
            active.volume = Mathf.Lerp(startVolActive, 0f, k);
            idle.volume = Mathf.Lerp(0f, 1f, k);
            yield return null;
        }

        // Intercambio de roles
        var temp = active;
        active = idle;
        idle = temp;

        // Asegurar estados
        idle.Stop();
        idle.volume = 0f;
        active.volume = 1f;
    }
}
