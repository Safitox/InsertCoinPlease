using UnityEngine;

public interface IInteract 
{
    public float TimeToExecute { get; set; }
    public void Interact();
}
