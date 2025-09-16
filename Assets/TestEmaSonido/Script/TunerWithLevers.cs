using UnityEngine;

public class TunerWithLevers : MonoBehaviour
{
    [Header("References")]
    public AudioSource toneSource;
    public GameObject feedbackCube3D;
    private Renderer cubeRenderer;

    [Header("Levers")]
    public LeverInteraction lever1;
    public LeverInteraction lever2;
    public LeverInteraction lever3;

    [Header("Settings")]
    public float minPitch = 0.5f;
    public float maxPitch = 2.0f;
    public float tolerance = 0.02f;

    private float targetPitch;
    private bool isFixed = false;

    void Start()
    {
        targetPitch = Random.Range(minPitch + 0.05f, maxPitch - 0.05f);
        Debug.Log("Sonido a encontrar: " + targetPitch);

        if (toneSource != null)
        {
            toneSource.loop = true;
            toneSource.playOnAwake = false;
            toneSource.pitch = 1f;
            toneSource.Play();
        }

        if (feedbackCube3D != null)
            cubeRenderer = feedbackCube3D.GetComponent<Renderer>();
    }

    void Update()
    {
        if (isFixed) return;

        float currentPitch = GetCombinedPitch();
        toneSource.pitch = currentPitch;

        if (Mathf.Abs(currentPitch - targetPitch) <= tolerance)
            OnFixed();

        UpdateFeedback(currentPitch);
    }

    float GetCombinedPitch()
    {
        float total = lever1.GetContribution() + lever2.GetContribution() + lever3.GetContribution(); // 0 a 3
        float normalized = total / 3f; // 0.0 a 1.0
        return Mathf.Lerp(minPitch, maxPitch, normalized);
    }

    void OnFixed()
    {
        isFixed = true;
        toneSource.pitch = targetPitch;
        Debug.Log("¡Correcto! Máquina reparada.");
        // Podés agregar efectos visuales o avanzar de nivel acá
    }

    void UpdateFeedback(float currentPitch)
    {
        if (cubeRenderer == null) return;

        float distance = Mathf.Abs(currentPitch - targetPitch);
        float normalized = Mathf.Clamp01(1f - (distance / (maxPitch - minPitch)));
        cubeRenderer.material.color = Color.Lerp(Color.red, Color.green, normalized);
    }
}
