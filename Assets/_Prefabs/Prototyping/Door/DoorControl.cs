using UnityEngine;

public class DoorControl : MonoBehaviour
{

    public void ChangeDoorStatus(bool value)
    {
        GetComponent<Animator>().SetBool("OpenDoor", value);
    }

}
