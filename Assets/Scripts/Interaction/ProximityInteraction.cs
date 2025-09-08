using UnityEngine;

[RequireComponent (typeof(Collider))]
public class ProximityInteraction : MonoBehaviour
{
    //UI Interacción   
    [SerializeField] Transform interactionUI;
    [SerializeField] private MeshRenderer meshRendererFill;
    [SerializeField] bool ShowVisualAid = true; //Por si se quiere mantener el trigger pero no mostrar la UI
        

    protected bool playerInRange = false;
    protected Material mat;
    protected bool interacting = false;

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


    protected virtual void FixedUpdate()
    {
        //Hago aparecer el marcador visual
        if (ShowVisualAid)
        {
            interactionUI.LookAt(Camera.main.transform);
            interactionUI.Rotate(0, 180, 0);
        }
      
    }

    protected virtual void Reset()
    {
        interacting = false;
    }

    void ShowVisual(bool show)
    {
        interactionUI.gameObject.SetActive(show && ShowVisualAid);
    }

}
