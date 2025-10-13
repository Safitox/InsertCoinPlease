using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class DoorCameraTrigger : MonoBehaviour
{
    public CinemachineCamera camFeedback;
    public CinemachineCamera camEscenario;
    public Camera camaraNormal;
    public float tiempoAntesDeEscenario = 3f;

    void Start()
    {
        //camaraNormal = GetComponent<Camera>();
        camaraNormal = Camera.main;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Activar Cinemachine feedback primero
            camaraNormal.enabled = false;
            camFeedback.Priority = 100;
            camEscenario.Priority = 50;

            Invoke("ActivarVistaEscenario", tiempoAntesDeEscenario);
        }
    }

    void ActivarVistaEscenario()
    {
        camFeedback.Priority = 10;
        camEscenario.Priority = 100;
    }
}