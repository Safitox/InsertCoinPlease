using UnityEngine;

public class puzzleTwo : MonoBehaviour
{
    [SerializeField] PressurePlate pressure;
    public Animator animator;
    [SerializeField] private Light lights;

    private void Start()
    {
        if (pressure != null)
            pressure.onPressed.AddListener(PlayAnimation);
    }
    public void PlayAnimation()
    {
        animator.SetTrigger("Pressed");
        if (!lights.enabled)
        {
            lights.enabled = true;
        }
    }
}