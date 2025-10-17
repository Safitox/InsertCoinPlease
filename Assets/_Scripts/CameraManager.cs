using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera; // Tu cámara real con seguimiento por script
    public CinemachineBrain brain; // El componente CinemachineBrain
    public CinemachineCamera camFeedback;
    public CinemachineCamera camEscenario;

    public enum CameraMode { Manual, Feedback, Escenario }
    private CameraMode currentMode = CameraMode.Manual;

    void Awake()
    {
        if (!mainCamera) mainCamera = Camera.main;
        if (!brain) brain = mainCamera.GetComponent<CinemachineBrain>();

        // Buscar cámaras virtuales por nombre
        camFeedback = GameObject.Find("CamFeedback")?.GetComponent<CinemachineCamera>();
        camEscenario = GameObject.Find("CamEscenario")?.GetComponent<CinemachineCamera>();
    }

    void Start()
    {
        SetCameraMode(CameraMode.Manual); // Arranca con tu sistema
    }

    public void SetCameraMode(CameraMode mode)
    {
        currentMode = mode;

        switch (mode)
        {
            case CameraMode.Manual:
                brain.enabled = false;
                mainCamera.enabled = true;
                DisableAllVirtualCameras();
                break;

            case CameraMode.Feedback:
                brain.enabled = true;
                mainCamera.enabled = true;
                ActivateVirtualCamera(camFeedback);
                break;

            case CameraMode.Escenario:
                brain.enabled = true;
                mainCamera.enabled = true;
                ActivateVirtualCamera(camEscenario);
                break;
        }
    }

    private void ActivateVirtualCamera(CinemachineCamera cam)
    {
        DisableAllVirtualCameras();
        cam.gameObject.SetActive(true);
        cam.Priority = 100;
    }

    private void DisableAllVirtualCameras()
    {
        camFeedback.Priority = 10;
        camEscenario.Priority = 10;
    }
}


