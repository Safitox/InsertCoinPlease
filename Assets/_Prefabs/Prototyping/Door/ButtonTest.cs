using UnityEngine;

public class ButtonTest : InteractionObject
{
    [SerializeField] private DoorControl doorControl;
    ProximityPulse proximityPulse => GetComponentInChildren<ProximityPulse>();

    private void Start()
    {
        proximityPulse.OnPulse += Interact;
    }
    public override void Interact()
    {
       GetComponent<Animator>().SetTrigger("PressButton");
       doorControl.ChangeDoorStatus(true);
    }


}
