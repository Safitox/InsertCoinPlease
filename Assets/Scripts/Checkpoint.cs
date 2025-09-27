using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.lastCheckpoint= transform.position;
            Debug.Log("Checkpoint reached at: " + transform.position);
            // Optionally, you can add some visual or audio feedback here

        }
    }
}
