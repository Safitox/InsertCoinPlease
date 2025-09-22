using UnityEngine;

public class triggerDialog : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogView dialogView = ServiceLocator.Instance.GetService<DialogView>();
            if (dialogView != null)
            {
                dialogView.DisplayMessage("dialog1");
                // Optionally, disable the collider to prevent retriggering
                // GetComponent<Collider>().enabled = false;
            }
            else
            {
                Debug.LogWarning("DialogView service not found.");
            }
            Destroy(gameObject); // Destroy the trigger object after activation
        }
    }
}
