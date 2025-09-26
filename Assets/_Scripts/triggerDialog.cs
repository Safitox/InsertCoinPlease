using UnityEngine;
using UnityEngine.Events;

public class TriggerDialog : MonoBehaviour
{
    [SerializeField] private string initialDialogKey;
    [SerializeField] private bool destroyOnEnd = true;

    [Header("Trigger settings")]
    [SerializeField] private bool useTriggerCollider = true;


    [Header("Events")]
    [SerializeField] private UnityEvent OnStartDialogue;
    [SerializeField] private UnityEvent OnEndDialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (!useTriggerCollider) return;
        if (other.CompareTag("Player"))
            StartDialogue();
    }

    public void StartDialogue()
    {
        DialogView dialogView = ServiceLocator.Instance.GetService<DialogView>();
        if (dialogView != null)
        {
            dialogView.DisplayMessage(initialDialogKey);
            // Optionally, disable the collider to prevent retriggering
            // GetComponent<Collider>().enabled = false;
            dialogView.OnEndDialog += ExecuteEndAction;
            OnStartDialogue.Invoke();
        }
        else
        {
            Debug.LogWarning("DialogView service not found.");
        }
    }
    void ExecuteEndAction()
    {
        OnEndDialogue.Invoke();
        //GameManager.Instance.player.GetComponent<ThirdPersonController>().jumpEnabled = true;
        if (destroyOnEnd)
            Destroy(gameObject); // Destroy the trigger object after activation
    }
}
