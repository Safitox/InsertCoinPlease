using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    [SerializeField] private DoorControl doorControl;
    ProximityPulse proximityPulse => GetComponentInChildren<ProximityPulse>();

    private void Start()
    {
        proximityPulse.OnPulse += Interact;
    }
    private void Interact()
    {
       GetComponent<Animator>().SetTrigger("PressButton");
       doorControl.ChangeDoorStatus(true);
    }


}
