using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class DoorCameraTrigger : MonoBehaviour
{
    public CinemachineCamera camFeedback;
    public CinemachineCamera camEscenario;
    public Camera camaraNormal;
    public float tiempoAntesDeEscenario = 3f;
    CameraManager camManager;

    void Start()
    {
        //camaraNormal = GetComponent<Camera>();
        camaraNormal = Camera.main;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camManager.SetCameraMode(CameraManager.CameraMode.Feedback);
            StartCoroutine(TransicionAEscenario());
        }
    }

    IEnumerator TransicionAEscenario()
    {
        yield return new WaitForSeconds(3f);
        camManager.SetCameraMode(CameraManager.CameraMode.Escenario);
    }

}