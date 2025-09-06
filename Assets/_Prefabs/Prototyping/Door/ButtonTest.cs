using UnityEngine;

public class ButtonTest : InteractionObject
{
    [SerializeField] private DoorControl doorControl;
    public override void Interact()
    {
       GetComponent<Animator>().SetTrigger("PressButton");
       doorControl.ChangeDoorStatus();
    }


}
