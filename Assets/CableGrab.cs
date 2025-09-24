using UnityEngine;

public class CableGrab : MonoBehaviour, ICarryable
{
    [Header("Punto donde el jugador sostiene el cable")]
    [SerializeField] private Transform holdPoint;

    [SerializeField] private GameObject currentGrabPoint;
    private FixedJoint joint;
    //[SerializeField] private bool isHolding = false;
    //[SerializeField] private float interactRange = 2f;

    private Transform player;

    public string Identity { get; set; } = "Cable1";
    public bool EnableCarry { get; set; } = true;
    public bool CarryMoving { get; set; } = true;

    public void OnCarry()
    {
       
    }

    public void OnDrop()
    {
       
    }

   
}
