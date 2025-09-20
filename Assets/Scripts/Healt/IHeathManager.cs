using UnityEngine;

public interface IHeathManager 
{
    int Health { get; set; }
    public void TakeDamege(int damage);
    public void Death();
}
