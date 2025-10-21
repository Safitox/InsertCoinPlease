using UnityEngine;

public class TriggerTextHelp : MonoBehaviour
{
    [SerializeField] string helpText;
    [SerializeField] bool HideOnExit = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PanelHelp.Instance.ShowHelp(helpText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && HideOnExit)
        {
            PanelHelp.Instance.HideHelp();
        }
    }

}
