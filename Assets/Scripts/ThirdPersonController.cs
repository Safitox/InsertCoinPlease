using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;        // El objeto del jugador
    [SerializeField] private Transform cam;           // Cámara principal

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float smoothTurnTime = 0.1f;

    [Header("Cámara")]
    [SerializeField] private float mouseSensitivity = 150f;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float verticalMin = -35f;
    [SerializeField] private float verticalMax = 60f;

    float turnSmoothVelocity;
    float yaw;   // Rotación horizontal
    float pitch; // Rotación vertical

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- Movimiento del jugador ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(h, 0f, v).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Rotar hacia la dirección relativa a la cámara
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(player.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTurnTime);
            player.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            player.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.World);
        }

        // --- Rotación de cámara con el mouse ---
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, verticalMin, verticalMax);

        // Posicionar cámara alrededor del jugador
        Vector3 offset = new Vector3(0, 1.5f, -cameraDistance);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        cam.position = player.position + rotation * offset;
        cam.LookAt(player.position + Vector3.up * 1.5f);
    }
}
