using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public Transform holdPoint; // punto donde el jugador sostiene el objeto
    private GameObject currentGrabPoint;
    private bool isHolding = false;

    void Update()
    {
        if (currentGrabPoint != null && Input.GetKeyDown(KeyCode.E))
        {
            if (!isHolding)
            {
                // Agarra el punto
                currentGrabPoint.transform.SetParent(holdPoint);
                currentGrabPoint.transform.localPosition = Vector3.zero;
                isHolding = true;
            }
            else
            {
                // Suelta el punto
                currentGrabPoint.transform.SetParent(null);
                isHolding = false;
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
}
