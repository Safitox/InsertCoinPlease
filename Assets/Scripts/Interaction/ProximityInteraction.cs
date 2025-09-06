using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class ProximityInteraction : MonoBehaviour
{
    //UI Interacción   
    [SerializeField] Transform interactionUI;
    [SerializeField] private MeshRenderer meshRendererFill;
    [SerializeField] bool ShowVisualAid = true; //Por si se quiere mantener el trigger pero no mostrar la UI
    [SerializeField] InteractionObject  interactableObject;
        

    bool playerInRange = false;
    Material mat;
    bool interacting = false;
    float _interactionTime = 0f;
    float interactionTime
    {
        get{ return _interactionTime; }
        set
        {
            _interactionTime = Mathf.Clamp(value, 0f, interactableObject.TimeToExecute);
            //Actualizar UI
            mat.SetFloat("_Fill",1f - _interactionTime/ interactableObject.TimeToExecute);
        }
    }

    private void Awake()
    {
        ShowVisual(false);
        mat = meshRendererFill.material;
        Reset();
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
        Reset();

        this.enabled = true;
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetButtonDown("Interact") && !interacting)
            interacting = true;
        if (Input.GetButtonUp("Interact"))
            Reset();

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

        if (!interacting)
            return;
        if (Input.GetButton("Interact"))
        {
            interactionTime -= Time.fixedDeltaTime;
            if (interactionTime <= 0f)
            {
                interactableObject.Interact();
                if (interactableObject.OneUse)
                    DestroyImmediate(gameObject);
                else
                    Reset();
                
            }
        }
        
        
    }

    private void Reset()
    {
        interactionTime = interactableObject.TimeToExecute;
        interacting = false;
    }

    void ShowVisual(bool show)
    {
        interactionUI.gameObject.SetActive(show && ShowVisualAid);
    }

}
