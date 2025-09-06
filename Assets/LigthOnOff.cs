using UnityEngine;

public class LigthOnOff : InteractionObject
{
    [SerializeField] private InteractionObject buttonLigth;

    [SerializeField] private Light redLigth;
    [SerializeField] private Light greenLigth;

    [SerializeField] private Renderer redRenderer;
    [SerializeField] private Renderer greenRenderer;

    [SerializeField] private Color greenEmissionOn = Color.green;
    [SerializeField] private Color emissionOff = Color.black;

    public override void Interact()
    {
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
}
