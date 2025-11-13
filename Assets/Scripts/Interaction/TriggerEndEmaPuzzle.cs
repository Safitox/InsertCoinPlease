using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerEndEmaPuzzle : MonoBehaviour
{
    [SerializeField] private AudioSource puzzleSound;


    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador entra en el trigger, iniciar el crossfade
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CrossFade());

        }
    }


    IEnumerator CrossFade()
    {
        float t = 0f;
        float fadeSeconds = 3f;
        // Volúmenes iniciales
        float startVolActive = puzzleSound.volume;


        // Realizar el crossfade
        while (t < fadeSeconds)
        {
            t += Time.deltaTime;
            // Calcular interpolación
            float k = Mathf.Clamp01(t / fadeSeconds);
            // Ajustar volúmenes
            puzzleSound.volume = Mathf.Lerp(startVolActive, 0f, k);
            yield return null;
        }


        // Parar la pista que quedó inactiva
        puzzleSound.volume = 0f;
        Destroy(this);
    }


}
