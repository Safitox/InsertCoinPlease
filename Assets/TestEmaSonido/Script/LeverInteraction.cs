using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class LeverInteraction : InteractionObject
{
    [Header("Lever State")]
    public bool isUp = false; // Estado actual

    [Header("Lever Contribution")]
    public float contributionUp = 1f;
    public float contributionDown = 0f;

    public float GetContribution()
    {
        return isUp ? contributionUp : contributionDown;
    }


    public override void Interact()
    {
        isUp = !isUp; // Cambia el estado
                      //UpdateVisual();
        GetComponent<Animator>().SetBool("MoveLever", isUp);
        Debug.Log("Palanca " + gameObject.name + " ahora está " + (isUp ? "arriba" : "abajo"));
    }

}