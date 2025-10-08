using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ProximityPositioner : MonoBehaviour
{
    public string Identity;
    public Transform target;
    public bool used = false;
    public string allowOnlyTag="";

    //[SerializeField] private DoorControl doorControl;

    [SerializeField] private Light redLigth;
    [SerializeField] private Light greenLigth;

    [SerializeField] private Renderer redRenderer;
    [SerializeField] private Renderer greenRenderer;

    [SerializeField] private Color greenEmissionOn = Color.green;
    [SerializeField] private Color emissionOff = Color.black;

    private static bool isUsed;

    public Action<bool, string> Connected;

    private void Update()
    {
        isUsed = used;
        if (used == true)
        {
            if (redRenderer.material.HasProperty("_EmissionColor"))
            {
                redRenderer.material.SetColor("_EmissionColor", emissionOff);
                redLigth.enabled = !used;
            }

            if (greenRenderer.material.HasProperty("_EmissionColor"))
            {
                greenRenderer.material.SetColor("_EmissionColor", greenEmissionOn);
                greenLigth.enabled = used;
            }

            //doorControl.ChangeDoorStatus(true);
        }
    }
}