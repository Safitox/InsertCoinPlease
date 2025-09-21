using UnityEngine;
using UnityEngine.UI;

public class GlitchManager : MonoBehaviour
{
    public CanvasGroup glitchOverlay;
    private float targetAlpha = 0f;

    public void UpdateGlitch(float healthPercent)
    {
        targetAlpha = 1f - healthPercent;
    }

    void Update()
    {
        // Interpolación suave hacia el valor objetivo
        glitchOverlay.alpha = Mathf.Lerp(glitchOverlay.alpha, targetAlpha, Time.deltaTime * 5f);
    }
}

/*
public class GlitchManager : MonoBehaviour
{
    public CanvasGroup glitchOverlay;

    public void UpdateGlitch(float healthPercent)
    {
        float intensity = 1f - healthPercent; // Menos salud = más glitch
        glitchOverlay.alpha = intensity; // Cambia la transparencia del overlay
    }
}
*/


