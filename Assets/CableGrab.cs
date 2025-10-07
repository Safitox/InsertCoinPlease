using UnityEngine;

public class CableGrab : MonoBehaviour, ICarryable
{
    [Header("Punto donde el jugador sostiene el cable")]
    [SerializeField] private Transform holdPoint;

    [SerializeField] private GameObject currentGrabPoint;
    private FixedJoint joint;
    //[SerializeField] private bool isHolding = false;
    [SerializeField] private float interactRange = 2f;

    private Transform player;

    public string Identity { get; set; } = "Cable1";
    public bool EnableCarry { get; set; } = true;
    public bool CarryMoving { get; set; } = true;

    //void Start()
    //{
    //    player = GameObject.FindWithTag("Player").transform;
    //}

    //void Update()
    //{
    //    return;
    //    float distance = Vector3.Distance(player.position, transform.position);

    //    if (distance <= interactRange )
    //    {
    //        if (currentGrabPoint != null && Input.GetKeyDown(KeyCode.E))
    //        {
    //            Rigidbody grabRb = currentGrabPoint.GetComponent<Rigidbody>();

    //            if (!isHolding && grabRb != null)
    //            {
    //                // Mover físicamente al holdPoint
    //                grabRb.transform.position = holdPoint.position;
    //                grabRb.transform.rotation = holdPoint.rotation;

    //                // Crear el joint en el HoldPoint
    //                Rigidbody holdRb = holdPoint.GetComponent<Rigidbody>();
    //                if (holdRb == null)
    //                {
    //                    holdRb = holdPoint.gameObject.AddComponent<Rigidbody>();
    //                    holdRb.isKinematic = true;
    //                    holdRb.useGravity = false;
    //                }

    //                joint = holdPoint.gameObject.AddComponent<FixedJoint>();
    //                joint.connectedBody = grabRb;

    //                isHolding = true;
    //            }
    //            else if (isHolding)
    //            {
    //                if (joint != null)
    //                {
    //                    Destroy(joint);
    //                }
    //                isHolding = false;
    //            }
    //        }
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("GrabPoint"))
    //    {
    //        currentGrabPoint = other.gameObject;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("GrabPoint") && !isHolding)
    //    {
    //        currentGrabPoint = null;
    //    }
    //}

    public void OnCarry()
    {
       
    }

    public void OnDrop()
    {
       
    }

   
}
