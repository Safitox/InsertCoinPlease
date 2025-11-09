using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ComandDoor : MonoBehaviour
{
    [SerializeField] bool playerDetected = false;
    [SerializeField] GameObject player;
    [SerializeField] Collider stairCollider;

    [SerializeField] private Light redLigth;
    [SerializeField] private Light greenLigth;

    [SerializeField] private Renderer redRenderer;
    [SerializeField] private Renderer greenRenderer;

    [SerializeField] private Color greenEmissionOn = Color.green;
    [SerializeField] private Color emissionOff = Color.black;

    private void OnTriggerEnter(Collider stairCollider)
    {
        if (stairCollider.CompareTag("Player"))
        {
            playerDetected = true;
            if (redRenderer.material.HasProperty("_EmissionColor"))
            {
                redRenderer.material.SetColor("_EmissionColor", emissionOff);
                redLigth.enabled = false;
            }

            if (greenRenderer.material.HasProperty("_EmissionColor"))
            {
                greenRenderer.material.SetColor("_EmissionColor", greenEmissionOn);
                greenLigth.enabled = true;
            }
        }
        if (playerDetected)
        {
            GetComponent<Animator>().SetBool("OpenDoor", true);
        }
    }
}
