using UnityEngine;

public class TunerWithBinaryLevers : MonoBehaviour
{

    [Header("Door")]
    public DoorControl doorToOpen;

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
    public float maxPitch = 4.0f;
    public float tolerance = 0.01f;

    [SerializeField] private float targetPitch=0.9f; //Tono a llegar
    private bool isFixed = false;

    void Start()
    {
        Debug.Log("Tono objetivo: " + targetPitch);

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

        //Debug.Log("Pitch actual generado:" + currentPitch.ToString("F2")); //F2 muestra los valores con dos decimales

        if (Mathf.Abs(currentPitch - targetPitch) <= tolerance)
            OnFixed();

        UpdateFeedback(currentPitch);
    }

    float GetCombinedPitch()
    {
        return lever1.GetContribution() + lever2.GetContribution() + lever3.GetContribution();
    }


    void OnFixed()
    {
        isFixed = true;
        toneSource.pitch = targetPitch;
        Debug.Log("¡Correcto! Máquina reparada.");
        // Acá podés avanzar de nivel, activar efectos, etc.
        if (doorToOpen!=null) 
        {
            doorToOpen.ChangeDoorStatus(true);
        }
    }

    /*
    void UpdateFeedback(float currentPitch) Versión re simple, no ayuda tampoco
    {
        if (cubeRenderer == null) return;

        float distance = Mathf.Abs(currentPitch - targetPitch);

        if (distance <= tolerance)
            cubeRenderer.material.color = Color.green;
        else
            cubeRenderer.material.color = Color.red;
    }
    */

    void UpdateFeedback(float currentPitch)
    {
        if (feedbackCube3D == null || cubeRenderer == null) return;

        // Escala el cubo directamente según el pitch actual
        float scaleY = Mathf.Clamp(currentPitch * 2f, minPitch, maxPitch);
        feedbackCube3D.transform.localScale = new Vector3(1f, scaleY, 1f);

        // Color verde si está dentro de la tolerancia, rojo si no
        float distance = Mathf.Abs(currentPitch - targetPitch);
        cubeRenderer.material.color = (distance <= tolerance) ? Color.green : Color.red;
    }



    /*
    void UpdateFeedback(float currentPitch) Muyy complejo, y muy confuso para el jugador
    {
        if (cubeRenderer == null) return;

        float distance = Mathf.Abs(currentPitch - targetPitch);
        float normalized = Mathf.Clamp01(1f - (distance / (maxPitch - minPitch)));
        cubeRenderer.material.color = Color.Lerp(Color.red, Color.green, normalized);
    }
    */
}
