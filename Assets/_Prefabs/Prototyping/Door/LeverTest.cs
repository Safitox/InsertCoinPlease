using UnityEngine;

public class LeverTest : InteractionObject
{
    [SerializeField] private DoorControl doorControl;
    bool isOpen = false;
    public override void Interact()
    {
        isOpen = !isOpen;
        GetComponent<Animator>().SetBool("MoveLever", isOpen);
       doorControl.ChangeDoorStatus();
    }


}
