using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform target ;
    [SerializeField] private Camera cam;

    [Header("Control")]
    [SerializeField] private float mouseSensitivity = 150f;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float cameraYOffset = 1f;
    [SerializeField] private float verticalMin = -35f;
    [SerializeField] private float verticalMax = 60f;

    [Header("Zoom (FOV)")]
    [SerializeField] private float maxzoom = 70f;
    [SerializeField] private float minzoom = 50f;

    [Header("Colisiones / Suavizado")]
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private float cameraSmooth = 0.08f;

    // exposiciones públicas para otros scripts
    public Transform CamTransform => cam != null ? cam.transform : null;

    float yaw, pitch;
    Vector3 camVel;

    void Reset()
    {
        if (cam == null) cam = Camera.main;
    }

    void Start()
    {
        if (cam == null) cam = Camera.main;
        // Init yaw/pitch from current rotation if possible
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = e.x;
        target = transform;
    }

    void Update()
    {
        if (target == null || cam == null) return;

        // Input de rotación
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, verticalMin, verticalMax);

        // Zoom por scroll -> ajusta FOV
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0f && cam.fieldOfView < maxzoom) cam.fieldOfView += 1f;
        if (scroll > 0f && cam.fieldOfView > minzoom) cam.fieldOfView -= 1f;
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        Vector3 targetOffset = new Vector3(0f, cameraYOffset, -cameraDistance);
        Quaternion camRot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredCamPos = target.position + camRot * targetOffset;
        Vector3 focusPoint = target.position + Vector3.up * 1.5f;

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

    // Método público para reubicar target en runtime si lo necesitás
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
