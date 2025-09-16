using UnityEngine;

public class LigthOnOff : MonoBehaviour
{
    [SerializeField] private Light redLigth;
    [SerializeField] private Light greenLigth;

    [SerializeField] private Renderer redRenderer;
    [SerializeField] private Renderer greenRenderer;

    [SerializeField] private Color greenEmissionOn = Color.green;
    [SerializeField] private Color emissionOff = Color.black;

    [SerializeField] private ProximitySwitch proximitySwitch;

    private void Start()
    {
        proximitySwitch.OnSwitch += Interact;
    }
    void Interact(bool value)
    {

        if (redRenderer.material.HasProperty("_EmissionColor"))
        {
            redRenderer.material.SetColor("_EmissionColor", emissionOff);
            redLigth.enabled = !value;
        }
                
         

        if (greenRenderer.material.HasProperty("_EmissionColor"))
        {
            greenRenderer.material.SetColor("_EmissionColor", greenEmissionOn);
            greenLigth.enabled = value;
        }
    }
}
