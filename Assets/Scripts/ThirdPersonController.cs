using System.Collections;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;  
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody rb;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;          
    [SerializeField] private float accel = 20f;             
    [SerializeField] private float rotationSmoothTime = 0.1f;
    [SerializeField] private float runSpeedMultiplier = 1.5f;
    bool running = false;

    [Header("Cámara")]
    [SerializeField] private float mouseSensitivity = 150f;
    [SerializeField] private float cameraDistance = 5f;    
    [SerializeField] private float verticalMin = -35f;
    [SerializeField] private float verticalMax = 60f;
    [SerializeField] private float maxzoom = 70f;
    [SerializeField] private float minzoom = 50f;
    [SerializeField] private float cameraCollisionRadius = 0.2f; // para evitar clipping
    [SerializeField] private float cameraSmooth = 0.08f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDist = 0.35f;


    float yaw, pitch;
    float turnSmoothVelocity;
    Vector3 camVel; // para SmoothDamp de cámara

    // input cache
    float inputH, inputV;
    bool jumpPressed;

    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // evita caídas laterales
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        running = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetButtonDown("Jump"))
        {

            jumpPressed = true;

        }

        // Cámara orbital
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, verticalMin, verticalMax);

        // Zoom por FOV 
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0 && cam.fieldOfView < maxzoom) cam.fieldOfView += 1;
        if (scroll > 0 && cam.fieldOfView > minzoom) cam.fieldOfView -= 1;
    }

    void FixedUpdate()
    {

        Vector3 inputDir = new Vector3(inputH, 0f, inputV).normalized;

        // direcciones relativas a cámara
        Vector3 camForward = cam.transform.forward; camForward.y = 0f; camForward.Normalize();
        Vector3 camRight = cam.transform.right; camRight.y = 0f; camRight.Normalize();

        Vector3 desiredVel = Vector3.zero;
        if (inputDir.sqrMagnitude > 0.0001f)
        {
            Vector3 moveDir = (camForward * inputDir.z + camRight * inputDir.x).normalized;
            desiredVel = moveDir * moveSpeed;
            desiredVel *= running ? runSpeedMultiplier : 1f;

            // rotación suave hacia la dirección de movimiento
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(player.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
            player.rotation = Quaternion.Euler(0f, angle, 0f);
        }


        Vector3 currentVel = rb.linearVelocity;
        Vector3 horizVel = new Vector3(currentVel.x, 0f, currentVel.z);
        Vector3 newHorizVel = Vector3.MoveTowards(horizVel, desiredVel, accel * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector3(newHorizVel.x, currentVel.y, newHorizVel.z);


        if (jumpPressed)
        {
            if (IsGrounded())
            {
                Vector3 v = rb.linearVelocity;
                if (v.y < 0f) v.y = 0f;
                rb.linearVelocity = v;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            jumpPressed = false;
        }

        Vector3 targetOffset = new Vector3(0, 1.5f, -cameraDistance);
        Quaternion camRot = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredCamPos = player.position + camRot * targetOffset;
        Vector3 focusPoint = player.position + Vector3.up * 1.5f;

        // TODO: cambio rayhit por spherecast. mas testeo
        Vector3 toCam = desiredCamPos - focusPoint;
        float dist = toCam.magnitude;
        Vector3 dir = dist > 0.001f ? toCam / dist : cam.transform.forward;

        float allowedDist = dist;
        if (Physics.SphereCast(focusPoint, cameraCollisionRadius, dir, out RaycastHit hit, dist, ~0, QueryTriggerInteraction.Ignore))
        {
            allowedDist = Mathf.Max(0.3f, hit.distance - 0.02f); 
        }

        Vector3 finalCamPos = focusPoint + dir * allowedDist;
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, finalCamPos, ref camVel, cameraSmooth);
        cam.transform.rotation = camRot;
        cam.transform.LookAt(focusPoint);
    }

    bool IsGrounded()
    {
        // Raycast al piso desde el centro del player
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        bool result = Physics.Raycast(origin, Vector3.down, groundCheckDist, groundMask, QueryTriggerInteraction.Ignore);
        return result;
    }
}
