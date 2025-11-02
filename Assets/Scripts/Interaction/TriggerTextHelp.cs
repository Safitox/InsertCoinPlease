using System.Linq.Expressions;
using UnityEngine;

public class TriggerTextHelp : MonoBehaviour
{
    [SerializeField] string helpText;
    [SerializeField] bool HideOnExit = false;
    [SerializeField] string destroyOnGlobalEvent = "";

    private void Start()
    {
        if (destroyOnGlobalEvent != "")
        {
            GameManager.Instance.globalEvent += OnGlobalEvent;
        }
    }


    void OnGlobalEvent(string parameter)
    {
        if (parameter == destroyOnGlobalEvent)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnDestroy()
    {
        try { 
        if (destroyOnGlobalEvent != "")
            GameManager.Instance.globalEvent -= OnGlobalEvent;
    } catch{}
        
    }
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
