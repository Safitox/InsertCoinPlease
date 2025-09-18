using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] Transform handTransform;
    [SerializeField] Transform dropPointTransform;


    public static GameObject objectInHand;
    private bool hasRB = false;
    public  Transform oldParent;
    ICarryable carryInHand;


    [SerializeField] float radius = 18f;
    [SerializeField] LayerMask targetLayers;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (objectInHand == null)
            {
                GetObject();
            }
            else
            {
                DropObject();
            }
        }   
        if (objectInHand == null) return;
        if (carryInHand.CarryMoving) 
            objectInHand.transform.position = handTransform.position;
    }


    void GetObject()
    {
        if (objectInHand != null) return;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position +  Vector3.down*0.5f, radius, targetLayers);

        // Busco el primer carryable
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<ICarryable>(out ICarryable carryable))
            {
                if (carryable.EnableCarry)
                {
                    carryInHand = carryable; 
                    objectInHand = hitCollider.gameObject;
                    objectInHand.transform.position = handTransform.position;
                    oldParent= objectInHand.transform.parent;
                    if (!carryable.CarryMoving) ;
                        objectInHand.transform.parent = handTransform;
                    objectInHand.GetComponent<Collider>().enabled = false;
                    if (objectInHand.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        hasRB = true;
                        rb.isKinematic = true;
                    }
                    carryable.OnCarry();
                    break;
                }
            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    // Set the color for the gizmo
    //    Gizmos.color = Color.yellow;

    //    // Draw a solid sphere at the GameObject's position
    //    Gizmos.DrawSphere(transform.position +  Vector3.down*0.5f, radius);
    //}


    void DropObject()
    {
        if (objectInHand == null) return;
        if (CheckPositoners()) return;
        if (hasRB && objectInHand.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            hasRB = false;
        }
        if (objectInHand.TryGetComponent<ICarryable>(out ICarryable carryable))
        {
            carryable.OnDrop();
        }
        objectInHand.GetComponent<Collider>().enabled = true;
        if (!carryable.CarryMoving) ;
            objectInHand.transform.parent = oldParent;
        objectInHand.transform.position = dropPointTransform.position  ;
        objectInHand = null;

    }

    bool CheckPositoners()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position + Vector3.down * 0.5f, radius, targetLayers);

        // Busco ProximityPositioners
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<ProximityPositioner>(out ProximityPositioner proximityPositioner))
            {
                if (proximityPositioner.Identity == objectInHand.GetComponent<ICarryable>().Identity)
                {
                    // Dejar el objeto en la posicion del positioner
                    objectInHand.transform.position = proximityPositioner.target.position;
                    objectInHand.transform.rotation = proximityPositioner.target.rotation;
                    proximityPositioner.used = true;
                    Destroy(objectInHand.GetComponent<Rigidbody>());
                    objectInHand.GetComponent<ICarryable>().EnableCarry = false;
                    objectInHand.GetComponent<Collider>().enabled = true;
                    objectInHand.transform.parent = hitCollider.transform;
                    objectInHand = null;

                    return true;
                }
            }


        }
        return false;


    }

}
