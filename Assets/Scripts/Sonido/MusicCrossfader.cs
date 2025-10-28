using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicCrossfader : MonoBehaviour
{
    //Tiempo para realizar el crossfade
    public float defaultFade = 2f;


    //Fuentes de audio (active = la que se escucha, idle = la que está en espera)
    public AudioSource active, idle;

    void Awake()
    {
        // Inicialización
        idle.volume = 0f;
    }

    private void Start()
    {
        // Buscar la referencia a la fuente de música principal
        active = GameObject.Find("Music").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador entra en el trigger, iniciar el crossfade
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
        // Si no se especifica un tiempo de fade, usar el por defecto
        if (fadeSeconds <= 0f) fadeSeconds = defaultFade;

        // Programar el inicio en la reproducción de la pista idle
        double startDsp = AudioSettings.dspTime + 0.05; // pequeño offset
        idle.PlayScheduled(startDsp);

        // Iniciar la rutina de crossfade
        StopAllCoroutines();
        StartCoroutine(CrossFade(fadeSeconds, startDsp));
    }

    IEnumerator CrossFade(float fadeSeconds, double startDsp)
    {
        float t = 0f;
        // Volúmenes iniciales
        float startVolActive = active.volume;


        // Esperar hasta el momento programado para iniciar la pista idle
        while (AudioSettings.dspTime < startDsp) yield return null;

        // Realizar el crossfade
        while (t < fadeSeconds)
        {
            t += Time.deltaTime;
            // Calcular interpolación
            float k = Mathf.Clamp01(t / fadeSeconds);
            // Ajustar volúmenes
            active.volume = Mathf.Lerp(startVolActive, 0f, k);
            // Ajustar volúmen de la pista idle
            idle.volume = Mathf.Lerp(0f, 1f, k);
            yield return null;
        }

        // Asegurar volúmenes finales
        var temp = active;
        active = idle;
        idle = temp;

        // Parar la pista que quedó inactiva
        idle.Stop();
        idle.volume = 0f;
        active.volume = 1f;
    }
}
