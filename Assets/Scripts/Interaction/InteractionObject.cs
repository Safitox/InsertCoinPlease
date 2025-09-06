using UnityEngine;


//Clase a heredar
public abstract class InteractionObject : MonoBehaviour
{
    public float TimeToExecute;
    public bool OneUse = false;

    public abstract void Interact();
}



