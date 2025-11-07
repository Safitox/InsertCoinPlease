using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Animator otherAnimator;
    [SerializeField] private string triggerName = "Bullet";

    [SerializeField] private bool buttonOne = false;
    public bool IsPressed => buttonOne;
    private ProximityPulse proximityPulse => GetComponentInChildren<ProximityPulse>();

    private void Start()
    {
        proximityPulse.OnPulse += Interact;
    }

    private void Interact()
    {
        GetComponent<Animator>().SetTrigger("ButtonPuzzle");
        buttonOne = true;

        if (otherAnimator != null)
            otherAnimator.SetTrigger(triggerName);
    }
}
