using UnityEngine;

public class WaveFeedback : MonoBehaviour
{
    [Header("Wave Settings")]
    public LineRenderer currentWave;
    public LineRenderer targetWave;
    [SerializeField] public int points = 150;
    [SerializeField] public float width = 30f;
    [SerializeField] public float amplitude = 0.5f;
    [SerializeField] public float speed = 2f;
    [SerializeField] public float separation = 0.1f; // <-- nueva línea

    [HideInInspector] public float targetPitch = 1f;
    [HideInInspector] public float currentPitch = 1f;

    private float timeOffset;

    void Start()
    {
        if (currentWave != null) currentWave.positionCount = points;
        if (targetWave != null) targetWave.positionCount = points;
    }

    void Update()
    {
        timeOffset += Time.deltaTime * speed;

        if (targetWave != null)
            DrawWave(targetWave, targetPitch, Color.green, separation); // onda fija

        if (currentWave != null)
            DrawWave(currentWave, currentPitch, Color.red, -separation); // onda del jugador
    }

    void DrawWave(LineRenderer lr, float pitch, Color color, float offsetZ)
    {
        lr.startColor = lr.endColor = color;

        for (int i = 0; i < points; i++)
        {
            float x = (float)i / points * width;
            float y = Mathf.Sin((x * pitch * 4f) + timeOffset) * amplitude;
            lr.SetPosition(i, new Vector3(x - width / 2, y, offsetZ)); //desplazamos una un poco en Z
        }
    }
}

