using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Collider))]
public class CableOpenDoor : MonoBehaviour
{
    //[SerializeField] private DoorControl doorControl;

    //[SerializeField] private Light redLigth;
    //[SerializeField] private Light greenLigth;

    //[SerializeField] private Renderer redRenderer;
    //[SerializeField] private Renderer greenRenderer;

    //[SerializeField] private Color greenEmissionOn = Color.green;
    //[SerializeField] private Color emissionOff = Color.black;

    //[SerializeField] private bool isUsed;

    //private void Update()
    //{
    //    //isUsed = ProximityPositioner.cableUsed;
    //    if (isUsed == true)
    //    {
    //        if (redRenderer.material.HasProperty("_EmissionColor"))
    //        {
    //            redRenderer.material.SetColor("_EmissionColor", emissionOff);
    //            redLigth.enabled = !isUsed;
    //        }

    //        if (greenRenderer.material.HasProperty("_EmissionColor"))
    //        {
    //            greenRenderer.material.SetColor("_EmissionColor", greenEmissionOn);
    //            greenLigth.enabled = isUsed;
    //        }

    //        doorControl.ChangeDoorStatus(true);
    //    }
    //}
}

