using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
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
    }
}