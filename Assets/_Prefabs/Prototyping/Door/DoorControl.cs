using UnityEngine;

public class DoorControl : MonoBehaviour
{
    bool isOpen = false;
    public void ChangeDoorStatus()
    {
        isOpen= !isOpen;
        GetComponent<Animator>().SetBool("OpenDoor", isOpen);
    }

}
