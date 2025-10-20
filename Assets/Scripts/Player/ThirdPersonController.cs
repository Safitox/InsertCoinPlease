using System.Collections;
using UnityEngine;


public class ThirdPersonController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rb;
    public Animator animator;
    [SerializeField] private CameraController cameraController;

    [Header("Sonidos")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip stunClip;
    [SerializeField] AudioClip stepClip;
    [SerializeField] AudioClip jumpClip;

    [SerializeField] float stepInterval = 0.5f;
    [SerializeField] float runPitch = 1.4f;
    [SerializeField] float walkPitch = 1f;

    float stepTimer;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accel = 20f;
    [SerializeField] private float rotationSmoothTime = 0.1f;
    [SerializeField] private float runSpeedMultiplier = 1.5f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    public bool jumpEnabled;

    bool running = false;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDist = 0.35f;

    [Header("Escaleras / Escalones")]
    [SerializeField] private float stepSmooth = 0.08f;  // qué tan suave sube
    [SerializeField] private Transform feetLevel;   // punto desde donde se lanzan los rayos
    [SerializeField] private Transform stairDetector; // punto desde donde se detectan las escaleras

    float turnSmoothVelocity;
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
        if (jumpEnabled)
            if (Input.GetButtonDown("Jump"))
                jumpPressed = true;
    }

    void FixedUpdate()
    {
        if (!stunned)
        {
            Vector3 inputDir = new Vector3(inputH, 0f, inputV).normalized;


            Transform camT = cameraController != null ? cameraController.CamTransform : Camera.main.transform;

            Vector3 camForward = camT.forward; camForward.y = 0f; camForward.Normalize();
            Vector3 camRight = camT.right; camRight.y = 0f; camRight.Normalize();

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
                animator.SetBool("Walking", true);
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

            if (jumpPressed && jumpEnabled)
            {
                if (IsGrounded())
                {
                    Vector3 v = rb.linearVelocity;
                    if (v.y < 0f) v.y = 0f;
                    rb.linearVelocity = v;
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    animator.SetTrigger("Jump");

                    if (audioSource != null && jumpClip != null)
                        audioSource.PlayOneShot(jumpClip);
                }
                jumpPressed = false;
            }

            //  Nuevo: chequeo de escalones
            StepClimb();

            // Sonido de pasos
            bool isMoving = inputH != 0 || inputV != 0;

            if (isMoving && IsGrounded())
            {
                stepTimer -= Time.deltaTime;
                if (stepTimer <= 0f)
                {
                    audioSource.pitch = running ? runPitch : walkPitch;
                    audioSource.PlayOneShot(stepClip);
                    stepTimer = running ? stepInterval * 0.7f : stepInterval;
                }
            }
            else
            {
                stepTimer = 0f; // Reinicia si no se mueve
            }
        }

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
        Vector3 originLow = feetLevel.position;
        // Origen alto (altura máxima del escalón)
        Vector3 originHigh = stairDetector.position;

        float checkDist = 0.5f;

        Debug.DrawRay(originLow, moveDir * checkDist, Color.red);   // rayo bajo
        Debug.DrawRay(originHigh, moveDir * checkDist, Color.green); // rayo alto

        if (Physics.Raycast(originLow, moveDir, out RaycastHit lowHit, checkDist, groundMask))
        {
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

        if (audioSource != null && stunClip != null)
            audioSource.PlayOneShot(stunClip);

        StartCoroutine(StunCoroutine());
    }

    IEnumerator StunCoroutine()
    {
        stunned = true;

        while (stunTimer >= 0f)
        {
            stunTimer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        animator.SetTrigger("StopShake");
        stunned = false;
    }

    public void ChangeJumpStatus(bool status)
    {
        jumpEnabled = status;
    }
}
