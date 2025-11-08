using UnityEngine;

public class FinalStairs : MonoBehaviour
{
    [Header("Boton del puzzle")]
    [SerializeField] private PuzzleButton button4;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private string ThirdPuzzle = "Activated";
    [SerializeField] private Light lights;

    private bool complete = false;

    private void Update()
    {
        if (complete) return;

        if (button4.IsPressed)
        {
            complete = true;
            animator.SetTrigger("Activated");
            if (!lights.enabled)
            {
                lights.enabled = true;
            }
        }
    }
}
