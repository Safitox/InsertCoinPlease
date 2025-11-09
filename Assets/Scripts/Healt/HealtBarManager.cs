using UnityEngine;
using UnityEngine.UI;

public class HealtBarManager : MonoBehaviour
{
    private Slider sliderHealthBar;
    private Camera _camera;
    private HealthManager healthManager;

    void Start()
    {
        sliderHealthBar = GetComponentInChildren<Slider>();
        healthManager = GetComponentInParent<HealthManager>();

        // evento
        CameraEvents.OnCameraRegistered += OnCameraChanged;

        // Si ya hay una cámara activa, la usamos
        _camera = CameraEvents.CurrentCamera;
    }

    void OnDestroy()
    {
        CameraEvents.OnCameraRegistered -= OnCameraChanged;
    }

    private void OnCameraChanged(Camera cam)
    {
        _camera = cam;
    }

    void Update()
    {
        if (_camera == null) return;

        transform.LookAt(_camera.transform);
        sliderHealthBar.value = healthManager.Health / 100f;
    }
}
