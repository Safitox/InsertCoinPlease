using System;
using UnityEngine;

public class CableProximity : MonoBehaviour
{
    //UI Interacción   
    [SerializeField] Transform interactionUI;
    [SerializeField] private MeshRenderer meshRendererFill;
    [SerializeField] bool ShowVisualAid = true; //Por si se quiere mantener el trigger pero no mostrar la UI
    [SerializeField] float TimeToExecute = 1f;
    [SerializeField] bool oneUse = false;
    public Action OnInteract;

    bool playerInRange = false;
    Material mat;
    bool interacting = false;
    float _interactionTime = 0f;
    float interactionTime
    {
        get { return _interactionTime; }
        set
        {
            _interactionTime = Mathf.Clamp(value, 0f, TimeToExecute);
            //Actualizar UI
            mat.SetFloat("_Fill", 1f - _interactionTime / TimeToExecute);
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
                GrabCable();

                OnInteract?.Invoke();
                if (oneUse)
                    DestroyImmediate(gameObject);
                else
                    Reset();

            }
        }
    }

    private void Reset()
    {
        interactionTime = TimeToExecute;
        interacting = false;
    }

    void ShowVisual(bool show)
    {
        interactionUI.gameObject.SetActive(show && ShowVisualAid);
    }

    //--

    public Transform handTransform;
    public GameObject cableTip;
    public KeyCode grabKey = KeyCode.E;
    public float interactRange = 2f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    //void Update()
    //{
    //    float distance = Vector3.Distance(player.position, transform.position);
    //    if (distance <= interactRange && Input.GetKeyDown(grabKey))
    //    {
    //        GrabCable();
    //    }
    //}

    void GrabCable()
    {
        cableTip.transform.position = handTransform.position;
        cableTip.transform.SetParent(handTransform);
        Debug.Log("Cable agarrado");
    }
}
