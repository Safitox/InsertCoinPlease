using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Animator otherAnimator;
    [SerializeField] private string triggerName = "Bullet";

    [SerializeField] private bool button = false;
    public bool IsPressed => button;
    private ProximityPulse proximityPulse => GetComponentInChildren<ProximityPulse>();

    private void Start()
    {
        proximityPulse.OnPulse += Interact;
    }

    private void Interact()
    {
        GetComponent<Animator>().SetTrigger("ButtonPuzzle");
        button = true;

        if (otherAnimator != null)
        {
            otherAnimator.SetTrigger(triggerName);
        }

    }
}
