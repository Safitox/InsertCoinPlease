using UnityEngine;
using UnityEngine.UI;

public class Tuner : MonoBehaviour
{
    [Header("References")]
    public AudioSource toneSource; // AudioSource
    public Slider pitchSlider;     // barra que usa el jugador para cambiar el tono
    //public Image feedbackImage;    // Feedback visual
    public GameObject feedbackCube3D; //Feedback visual en 3D
    private Renderer cubeRenderer; // para no llamar GetComponent cada frame

    [Header("Settings")] //Valores del tono
    public float minPitch = 0.5f;
    public float maxPitch = 2.0f;
    public float tolerance = 0.02f; // cuán cerca debe quedar para "acertar"

    private float targetPitch; //Tono que el jugador debe encontrar
    private bool isFixed = false;

    void Start()
    {
 
        // elegimos un target aleatorio dentro del rango (para variar el puzzle)
        targetPitch = Random.Range(minPitch + 0.05f, maxPitch - 0.05f);

        Debug.Log("Sonido a encontrar "+ targetPitch);

        // configuro el slider
        if (pitchSlider != null)
        {
            pitchSlider.minValue = minPitch;
            pitchSlider.maxValue = maxPitch;
            pitchSlider.value = (minPitch + maxPitch) / 2f;
            // registramos el callback para cuando el slider cambie
            pitchSlider.onValueChanged.AddListener(OnSliderChanged);
        }

        // arrancamos el tono
        if (toneSource != null)
        {
            toneSource.loop = true;
            toneSource.playOnAwake = false;
            toneSource.pitch = pitchSlider != null ? pitchSlider.value : 1f;
            toneSource.Play();
        }

        // Si hay cubo 3D, guardamos el Renderer
        if (feedbackCube3D != null)
        {
            cubeRenderer = feedbackCube3D.GetComponent<Renderer>();
        }

        UpdateFeedback();
    }

    // metodo que llama al mover la barra
    public void OnSliderChanged(float value)
    {
        if (isFixed) return;

        if (toneSource != null) toneSource.pitch = value;

        Debug.Log("Tono actual: " + value);

        if (Mathf.Abs(value - targetPitch) <= tolerance)
        {
            OnFixed();
        }

        UpdateFeedback();
    }

    void OnFixed()
    {
        isFixed = true;
        // fijo el valor exacto del target
        if (pitchSlider != null) pitchSlider.value = targetPitch;
        if (toneSource != null) toneSource.pitch = targetPitch;

        Debug.Log("¡Correcto! Máquina reparada.");
        // TODO: añadir animación / sonido de éxito / avanzar de nivel
    }

    void UpdateFeedback()
    {
        //if (feedbackImage == null || pitchSlider == null) return;

        if (cubeRenderer == null || pitchSlider == null) return;
        float distance = Mathf.Abs(pitchSlider.value - targetPitch);
        float normalized = Mathf.Clamp01(1f - (distance / (maxPitch - minPitch))); // 0..1
        //feedbackImage.color = Color.Lerp(Color.red, Color.green, normalized);
        cubeRenderer.material.color = Color.Lerp(Color.red, Color.green, normalized);
    }
}
