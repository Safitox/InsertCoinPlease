using UnityEngine;

public class HealthManager : MonoBehaviour, IHeathManager
{

    [SerializeField] int health;
    public int Health { get { return health;} 
                        set { health = value; } }

    public void Death()
    {
    }

    public void TakeDamege()
    {
    }


}
