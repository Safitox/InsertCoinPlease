using System.Collections;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody rb;
    public Animator animator;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accel = 20f;
    [SerializeField] private float rotationSmoothTime = 0.1f;
    [SerializeField] private float runSpeedMultiplier = 1.5f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;

    bool running = false;

    [Header("Cámara")]
    [SerializeField] private float mouseSensitivity = 150f;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float cameraYOffset = 1f;
    [SerializeField] private float verticalMin = -35f;
    [SerializeField] private float verticalMax = 60f;
    [SerializeField] private float maxzoom = 70f;
    [SerializeField] private float minzoom = 50f;
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private float cameraSmooth = 0.08f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDist = 0.35f;

    [Header("Escaleras / Escalones")]
    [SerializeField] private float stepHeight = 0.5f;   // altura máxima del escalón
    [SerializeField] private float stepSmooth = 0.08f;  // qué tan suave sube

    float yaw, pitch;
    float turnSmoothVelocity;
    Vector3 camVel;
    float stunTimer = 0f;
    float inputH, inputV;
    bool jumpPressed;
    bool crouching;
    bool stunned = false;

    void Start()
    {
        GameManager.Instance.player = transform;
        if (!rb) rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        crouching = Input.GetKey(KeyCode.LeftControl);
        if (Input.GetKeyDown(KeyCode.LeftControl))
            animator.SetBool("Crouch", true);
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            animator.SetBool("Crouch", false);
        running = Input.GetKey(KeyCode.LeftShift) && !crouching;
        animator.SetBool("Running", running);
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, verticalMin, verticalMax);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0 && cam.fieldOfView < maxzoom) cam.fieldOfView += 1;
        if (scroll > 0 && cam.fieldOfView > minzoom) cam.fieldOfView -= 1;
    }

    void FixedUpdate()
    {
        if (!stunned)
        { 
            Vector3 inputDir = new Vector3(inputH, 0f, inputV).normalized;

            Vector3 camForward = cam.transform.forward; camForward.y = 0f; camForward.Normalize();
            Vector3 camRight = cam.transform.right; camRight.y = 0f; camRight.Normalize();

            Vector3 desiredVel = Vector3.zero;
            if (inputDir.sqrMagnitude > 0.0001f)
            {
                Vector3 moveDir = (camForward * inputDir.z + camRight * inputDir.x).normalized;
                desiredVel = moveDir * moveSpeed;
                if (crouching) desiredVel *= crouchSpeedMultiplier;
                desiredVel *= running ? runSpeedMultiplier : 1f;

                float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(player.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
                player.rotation = Quaternion.Euler(0f, angle, 0f);
                animator.SetBool("Walking", true    );
                float dirDot = Vector3.Dot(moveDir, player.forward);
                float moveMagnitude = dirDot * inputDir.magnitude;
                animator.SetFloat("WalkingBlend", moveMagnitude);

            }
            else
                animator.SetBool("Walking", false);

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
                    animator.SetTrigger("Jump");
                }
                jumpPressed = false;
            }

            //  Nuevo: chequeo de escalones
            StepClimb();
        }

        Vector3 targetOffset = new Vector3(0, cameraYOffset, -cameraDistance);
        Quaternion camRot = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredCamPos = player.position + camRot * targetOffset;
        Vector3 focusPoint = player.position + Vector3.up * 1.5f;

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
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, groundCheckDist, groundMask, QueryTriggerInteraction.Ignore);
    }

    void StepClimb()
    {
        Vector3 _lb = rb.linearVelocity;
        _lb.y = 0;
        if (_lb.magnitude < 0.5f) return;

        Vector3 moveDir = rb.linearVelocity.normalized;

        // Origen bajo (pies)
        Vector3 originLow = transform.position + Vector3.down * 0.8f;
        // Origen alto (altura máxima del escalón)
        Vector3 originHigh = originLow + Vector3.up* stepHeight;

        float checkDist = 0.8f;

        // Dibujar los rayos en la vista de escena
        Debug.DrawRay(originLow, moveDir * checkDist, Color.red);   // rayo bajo
        Debug.DrawRay(originHigh, moveDir * checkDist, Color.green); // rayo alto

        // Raycast bajo
        if (Physics.Raycast(originLow, moveDir, out RaycastHit lowHit, checkDist, groundMask))
        {
            // Raycast alto
            if (!Physics.Raycast(originHigh, moveDir, checkDist, groundMask))
            {
                Vector3 targetPos = rb.position + Vector3.up * stepSmooth;
                rb.MovePosition(targetPos);
            }
        }
    }


    public void Stun(float time)
    {
        stunTimer = time;
        animator.Play("Stunned");
        StartCoroutine(StunCoroutine());
    }

    IEnumerator StunCoroutine()
    {
        stunned = true;

        while (stunTimer >= 0f)
        {
            stunTimer -= Time.deltaTime;
            yield return Time.fixedDeltaTime;
        }
        animator.SetTrigger("StopShake");
        stunned = false;
    }
}
