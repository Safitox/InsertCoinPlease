using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] Transform handTransform;
    public static GameObject objectInHand;
    private bool hasRB = false;

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
    }


    void GetObject()
    {
        if (objectInHand != null) return;
        Debug.Log("Trying to get object");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position +  Vector3.down*0.5f, radius, targetLayers);

        // Iterate through the found colliders
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log("Found collider: " + hitCollider.name);
            if (hitCollider.TryGetComponent<ICarryable>(out ICarryable carryable))
            {
                Debug.Log("Carrying object: " + hitCollider.name);
                objectInHand = hitCollider.gameObject;
                objectInHand.transform.position = handTransform.position;
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
        if (hasRB && objectInHand.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            hasRB = false;
        }
        if (objectInHand.TryGetComponent<ICarryable>(out ICarryable carryable))
        {
            carryable.OnDrop();
        }
        objectInHand.GetComponent<Collider>().enabled = false;
        objectInHand.transform.parent = null;
        objectInHand.transform.position = transform.position + transform.forward* 0.5f + ;
        objectInHand = null;
    }


}
