using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;        // El objeto del jugador
    [SerializeField] private Transform cam;           // Cámara principal
    [SerializeField] private Rigidbody rb; 

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float smoothTurnTime = 0.1f;

    [Header("Cámara")]
    [SerializeField] private float mouseSensitivity = 150f;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float verticalMin = -35f;
    [SerializeField] private float verticalMax = 60f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 7f;       // fuerza del salto
    [SerializeField] private LayerMask groundMask;       // qué capas son suelo
    [SerializeField] private float groundCheckDist = 0.3f;


    float turnSmoothVelocity;
    float yaw;   // Rotación horizontal
    float pitch; // Rotación vertical
    float verticalVel;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb= GetComponent<Rigidbody>();

    }

    void Update()
    {
        // --- Movimiento del jugador ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 _direction = new Vector3(h, 0f, v).normalized;

        if (_direction.magnitude >= 0.1f)
        {
            // Rotar hacia la dirección relativa a la cámara (es un opcional.... pero queda bien)
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(player.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTurnTime);
            player.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            player.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.World);
        }

        // --- Salto ---
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        // --- Rotación de cámara con el mouse ---
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, verticalMin, verticalMax);

    }

    private void FixedUpdate()
    {
        // Posicionar cámara alrededor del jugador
        // Primero chequeo obstrucción de cámara
        RaycastHit _hit;
        Vector3 _direction = cam.position - player.position;
        Ray _ray = new Ray(player.position + Vector3.up * 1.5f, _direction.normalized);
        if (Physics.Raycast(_ray, out _hit, cameraDistance))
        {
            if (_hit.collider.gameObject != player.gameObject)
            {
                cam.position = _hit.point;
            }
            else //si no hay obstrucción mantengo cameradistance
            {
                Vector3 _offset = new Vector3(0, 1.5f, -cameraDistance);
                Quaternion _rotation = Quaternion.Euler(pitch, yaw, 0);
                cam.position = player.position + _rotation * _offset;
            }
        }
        cam.LookAt(player.position + Vector3.up * 1.5f);
    }

    bool IsGrounded()
    {
        // chequeo con raycast desde el centro hacia abajo
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDist, groundMask);
    }
}
