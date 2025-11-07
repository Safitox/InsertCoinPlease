using UnityEngine;

public class puzzleTwo : MonoBehaviour
{
    [SerializeField] PressurePlate pressure;
    public Animator animator;

    private void Start()
    {
        if (pressure != null)
            pressure.onPressed.AddListener(PlayAnimation);
    }
    public void PlayAnimation()
    {
        animator.SetTrigger("Pressed");
    }
}