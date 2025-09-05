using UnityEngine;


//Clase a heredar
public class InteractionObject : MonoBehaviour
{
    public float TimeToExecute ;

    //TODO: Cambiar a abstract!
    public virtual void Interact()
    {
        Debug.Log("Presionado");
    }
}


