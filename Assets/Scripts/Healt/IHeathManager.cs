using UnityEngine;

public interface IHeathManager 
{
    int Health { get; set; }
    public void TakeDamage(int damage);
    public void Death();
}
