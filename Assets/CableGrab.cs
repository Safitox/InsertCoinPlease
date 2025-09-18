using UnityEngine;

public class CableGrab : MonoBehaviour
{
    [Header("Punto donde el jugador sostiene el cable")]
    [SerializeField] private Transform holdPoint;

    [SerializeField] private GameObject currentGrabPoint;
    private FixedJoint joint;
    [SerializeField] private bool isHolding = false;
    [SerializeField] private float interactRange = 2f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactRange /*&& Input.GetKeyDown(grabKey)*/)
        {
            if (currentGrabPoint != null && Input.GetKeyDown(KeyCode.E))
            {
                Rigidbody grabRb = currentGrabPoint.GetComponent<Rigidbody>();

                if (!isHolding && grabRb != null)
                {
                    // Mover físicamente al holdPoint
                    grabRb.transform.position = holdPoint.position;
                    grabRb.transform.rotation = holdPoint.rotation;

                    // Crear el joint en el HoldPoint
                    Rigidbody holdRb = holdPoint.GetComponent<Rigidbody>();
                    if (holdRb == null)
                    {
                        holdRb = holdPoint.gameObject.AddComponent<Rigidbody>();
                        holdRb.isKinematic = true;
                        holdRb.useGravity = false;
                    }

                    joint = holdPoint.gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = grabRb;

                    isHolding = true;
                }
                else if (isHolding)
                {
                    if (joint != null)
                    {
                        Destroy(joint);
                    }
                    isHolding = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrabPoint"))
        {
            currentGrabPoint = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrabPoint") && !isHolding)
        {
            currentGrabPoint = null;
        }
    }

    //public Transform handTransform; // Mano del jugador
    //public Rigidbody cableTipRb;    // Rigidbody de la punta del cable
    //public KeyCode grabKey = KeyCode.E;
    //public float interactRange = 2f;

    //private Transform player;
    //private FixedJoint grabJoint;

    //void Start()
    //{
    //    player = GameObject.FindWithTag("Player").transform;
    //}

    //void Update()
    //{
    //    float distance = Vector3.Distance(player.position, transform.position);

    //    if (distance <= interactRange && Input.GetKeyDown(grabKey))
    //    {
    //        GrabCable();
    //    }

    //Opcional: soltar con la misma tecla
    //    if (grabJoint != null && Input.GetKeyDown(grabKey))
    //    {
    //        ReleaseCable();
    //    }
    //}

    //void GrabCable()
    //{
    //    if (grabJoint != null) return; // Ya agarrado

    //    Crear un FixedJoint en la mano para unir la punta del cable
    //    grabJoint = handTransform.gameObject.AddComponent<FixedJoint>();
    //    grabJoint.connectedBody = cableTipRb;
    //    grabJoint.breakForce = 1000f; // Ajusta seg�n fuerza deseada
    //    grabJoint.breakTorque = 1000f;

    //    Debug.Log("Cable agarrado con f�sica");
    //}

    //void ReleaseCable()
    //{
    //    if (grabJoint != null)
    //    {
    //        Destroy(grabJoint);
    //        grabJoint = null;
    //        Debug.Log("Cable soltado");
    //    }
    //}

    //public Transform handTransform;
    //public GameObject cableTip;
    //public KeyCode grabKey = KeyCode.E;
    //public float interactRange = 2f;

    //public float moveSpeed = 5f;    // Velocidad de movimiento hacia la mano
    //public float rotateSpeed = 5f;  // Velocidad de rotaci�n hacia la mano

    //private bool isGrabbing = false;

    //private Transform player;

    //void Start()
    //{
    //    player = GameObject.FindWithTag("Player").transform;
    //}

    //void Update()
    //{
    //    float distance = Vector3.Distance(player.position, transform.position);

    //    // Detecta la interacci�n
    //    if (distance <= interactRange && Input.GetKeyDown(grabKey))
    //    {
    //        isGrabbing = true;
    //    }

    //    // Si se est� agarrando, mover suavemente la punta del cable hacia la mano
    //    if (isGrabbing)
    //    {
    //        cableTip.transform.position = Vector3.Lerp(cableTip.transform.position, handTransform.position, moveSpeed * Time.deltaTime);
    //        cableTip.transform.rotation = Quaternion.Slerp(cableTip.transform.rotation, handTransform.rotation, rotateSpeed * Time.deltaTime);

    //        // Opcional: fijar como hijo cuando est� suficientemente cerca
    //        if (Vector3.Distance(cableTip.transform.position, handTransform.position) < 0.05f)
    //        {
    //            cableTip.transform.SetParent(handTransform);
    //            isGrabbing = false; // Termina la transici�n
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
