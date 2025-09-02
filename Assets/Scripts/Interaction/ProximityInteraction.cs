using UnityEngine;

public class ProximityInteraction : MonoBehaviour
{
    //UI Interacción   
    [SerializeField] Transform interactionUI;
    [SerializeField] bool ShowVisualAid = true; //Por si se quiere mantener el trigger pero no mostrar la UI
    [SerializeField] IInteract  interactableObject;

    bool playerInRange = false;
    float _interactionTime = 0f;
    float interactionTime
    {
        get{ return _interactionTime; }
        set
        {
            _interactionTime = Mathf.Clamp(value, 0f, interactableObject.TimeToExecute);
            //TODO: Actualizar UI
        }
    }

    private void Awake()
    {
        ShowVisual(false);
        interactionTime = interactableObject.TimeToExecute;
    }
    private void OnTriggerEnter(Collider other)
    {
        playerInRange = other.CompareTag("Player");
        ShowVisual(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        playerInRange = false;
        ShowVisual(false);
        interactionTime = interactableObject.TimeToExecute;
        this.enabled = true;
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetButtonUp("Interact"))
            interactionTime = interactableObject.TimeToExecute;
        
    }
    private void FixedUpdate()
    {
        if (!playerInRange)
            return;

        //Hago aparecer el marcador visual
        if (ShowVisualAid)
        {
            interactionUI.LookAt(Camera.main.transform);
            interactionUI.Rotate(0, 180, 0);
        }

        //Intercepto el botón de interacción
        if (Input.GetButton("Interact"))
        {
            interactionTime -= Time.fixedDeltaTime;
            if (interactionTime <= 0f)
            {
                interactableObject.Interact();
                interactionTime = interactableObject.TimeToExecute;
            }
        }
        
        
    }


    void ShowVisual(bool show)
    {
        interactionUI.gameObject.SetActive(show && ShowVisualAid);
    }

}
