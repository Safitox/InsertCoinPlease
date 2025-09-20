using UnityEngine;

public interface IHeathManager 
{
    int Health { get; set; }
    public void TakeDamege();
    public void Death();
}
