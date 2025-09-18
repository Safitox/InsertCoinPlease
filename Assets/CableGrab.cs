using UnityEngine;

public class CableGrab : MonoBehaviour
{
    public Transform handTransform; // Mano del jugador
    public Rigidbody cableTipRb;    // Rigidbody de la punta del cable
    public KeyCode grabKey = KeyCode.E;
    public float interactRange = 2f;

    private Transform player;
    private FixedJoint grabJoint;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactRange && Input.GetKeyDown(grabKey))
        {
            GrabCable();
        }

        // Opcional: soltar con la misma tecla
        if (grabJoint != null && Input.GetKeyDown(grabKey))
        {
            ReleaseCable();
        }
    }

    void GrabCable()
    {
        if (grabJoint != null) return; // Ya agarrado

        // Crear un FixedJoint en la mano para unir la punta del cable
        grabJoint = handTransform.gameObject.AddComponent<FixedJoint>();
        grabJoint.connectedBody = cableTipRb;
        grabJoint.breakForce = 1000f; // Ajusta según fuerza deseada
        grabJoint.breakTorque = 1000f;

        Debug.Log("Cable agarrado con física");
    }

    void ReleaseCable()
    {
        if (grabJoint != null)
        {
            Destroy(grabJoint);
            grabJoint = null;
            Debug.Log("Cable soltado");
        }
    }

    //public Transform handTransform;
    //public GameObject cableTip;
    //public KeyCode grabKey = KeyCode.E;
    //public float interactRange = 2f;

    //public float moveSpeed = 5f;    // Velocidad de movimiento hacia la mano
    //public float rotateSpeed = 5f;  // Velocidad de rotación hacia la mano

    //private bool isGrabbing = false;

    //private Transform player;

    //void Start()
    //{
    //    player = GameObject.FindWithTag("Player").transform;
    //}

    //void Update()
    //{
    //    float distance = Vector3.Distance(player.position, transform.position);

    //    // Detecta la interacción
    //    if (distance <= interactRange && Input.GetKeyDown(grabKey))
    //    {
    //        isGrabbing = true;
    //    }

    //    // Si se está agarrando, mover suavemente la punta del cable hacia la mano
    //    if (isGrabbing)
    //    {
    //        cableTip.transform.position = Vector3.Lerp(cableTip.transform.position, handTransform.position, moveSpeed * Time.deltaTime);
    //        cableTip.transform.rotation = Quaternion.Slerp(cableTip.transform.rotation, handTransform.rotation, rotateSpeed * Time.deltaTime);

    //        // Opcional: fijar como hijo cuando esté suficientemente cerca
    //        if (Vector3.Distance(cableTip.transform.position, handTransform.position) < 0.05f)
    //        {
    //            cableTip.transform.SetParent(handTransform);
    //            isGrabbing = false; // Termina la transición
    //        }
    //    }
    //}

    //void Update()
    //{
    //    float distance = Vector3.Distance(player.position, transform.position);
    //    if (distance <= interactRange && Input.GetKeyDown(grabKey))
    //    {
    //        GrabCable();
    //    }
    //}

    //void GrabCable()
    //{
    //    cableTip.transform.position = handTransform.position;
    //    cableTip.transform.SetParent(handTransform);
    //    Debug.Log("Cable agarrado");
    //}
}
