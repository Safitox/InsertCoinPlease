using UnityEngine;

public class BossTrigger : MonoBehaviour
{

    [SerializeField] public GameObject boss;
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            activated = true;

            // Buscar el MusicManager en la escena y cambiar la música
            MusicManager musicManager = FindObjectOfType<MusicManager>();
            if (musicManager != null && boss != null)
            {
                musicManager.PlayBossMusic();
                boss.SetActive(true);
            }
        }
    }
}
