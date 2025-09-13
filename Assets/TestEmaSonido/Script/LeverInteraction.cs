using UnityEngine;

public class LeverInteraction : InteractionObject
{
    [Header("Lever State")]
    public bool isUp = false; // Estado actual

    //public Transform leverVisual; // Parte visual que rota
    //public Vector3 upRotation = new Vector3(-45f, 0f, 0f);
    //public Vector3 downRotation = new Vector3(45f, 0f, 0f);

    public override void Interact()
    {
        isUp = !isUp; // Cambia el estado
        //UpdateVisual();
        Debug.Log("Palanca " + gameObject.name + " ahora está " + (isUp ? "arriba" : "abajo"));
    }
    /*
    void UpdateVisual()
    {
        if (leverVisual != null)
        {
            leverVisual.localEulerAngles = isUp ? upRotation : downRotation;
        }
    }
    */

    public float GetContribution()
    {
        return isUp ? 1f : 0f; // 1 si está arriba (agudo), 0 si está abajo (grave)
    }
}