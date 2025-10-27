using UnityEngine;

public class DoorControl : MonoBehaviour
{
    [SerializeField] string globalEventToTrigger="";
    public void ChangeDoorStatus(bool value)
    {
        GetComponent<Animator>().SetBool("OpenDoor", value);
        if (value)
        {
            GetComponent<AudioSource>().Play();
            if (globalEventToTrigger!="")
                GameManager.Instance.globalEvent?.Invoke(globalEventToTrigger);
        }
    }

}
