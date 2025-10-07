using UnityEngine;

public class MusicDisabler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // O el tag que uses para el jugador
        {
            GameObject musicObj = GameObject.Find("Music");
            if (musicObj != null)
            {
                AudioSource audio = musicObj.GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.Stop(); // O Destroy(musicObj);
                }
            }
        }
    }
}

