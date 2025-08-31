using UnityEngine;


public class ShowInteraction : MonoBehaviour
{
    bool playerInRange = false;
    [SerializeField] Transform interactionUI;


    private void OnTriggerEnter(Collider other)
    {
        playerInRange = other.CompareTag("Player");
        interactionUI.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = !other.CompareTag("Player");
        interactionUI.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (playerInRange)
        {
            interactionUI.LookAt(Camera.main.transform);
            interactionUI.Rotate(0, 180, 0);
        }
        
    }

}
