using UnityEngine;



public class ProximityInteraction : MonoBehaviour
{
    //UI Interacción   
    bool playerInRange = false;
    [SerializeField] Transform interactionUI;
    [SerializeField] bool ShowVisualAid = true; //Por si se quiere mantener el trigger pero no mostrar la UI


    private void Awake()
    {
        ShowVisual(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        playerInRange = other.CompareTag("Player");
        ShowVisual(true);
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = !other.CompareTag("Player");
        ShowVisual(false);
    }

    private void FixedUpdate()
    {
        //TODO: Optimizar usando corrutinas tal vez?
        if (playerInRange)
        {
            interactionUI.LookAt(Camera.main.transform);
            interactionUI.Rotate(0, 180, 0);
        }
        
    }

    void ShowVisual(bool show)
    {
        interactionUI.gameObject.SetActive(show && ShowVisualAid);
    }

}
