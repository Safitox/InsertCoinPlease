using UnityEngine;

public class LeverTest : MonoBehaviour
{
    [SerializeField] private DoorControl doorControl;
    ProximitySwitch proximitySwitch=>GetComponentInChildren<ProximitySwitch>();

    private void Start()
    {
        if (proximitySwitch != null)
            proximitySwitch.OnSwitch +=  Interact;
    }
     void Interact(bool switchValue)
    {
        GetComponent<Animator>().SetBool("MoveLever", switchValue);
       doorControl.ChangeDoorStatus(switchValue);
    }


}
