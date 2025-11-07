using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [Header("Botones del puzzle")]
    [SerializeField] private PuzzleButton button1;
    [SerializeField] private PuzzleButton button2;
    [SerializeField] private PuzzleButton button3;

    [Header("Animator del puzzle")]
    [SerializeField] private Animator puzzleAnimator;
    [SerializeField] private string SecondPuzzle = "Activate";
    [SerializeField] private Light lights;

    private bool puzzleCompleted = false;

    private void Update()
    {
        if (puzzleCompleted) return;

        if (button1.IsPressed && button2.IsPressed && button3.IsPressed)
        {
            puzzleCompleted = true;
            puzzleAnimator.SetTrigger(SecondPuzzle);

            if (!lights.enabled) {
                lights.enabled = true;
            }
        }
    }

}