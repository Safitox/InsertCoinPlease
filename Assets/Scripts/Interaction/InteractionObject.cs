using UnityEngine;


//Clase a heredar
public class InteractionObject : MonoBehaviour
{
    public float TimeToExecute ;
    public bool press;

    //TODO: Cambiar a abstract!
    public virtual void Interact()
    {
        press = true;
        Debug.Log("Presionado");
    }
}


