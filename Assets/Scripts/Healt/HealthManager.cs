using UnityEngine;

public class HealthManager : MonoBehaviour, IHeathManager
{

    [SerializeField] int health = 100;
    public int Health { get { return health;} 
                        set { health = value; } }

    //public GlitchManager glitch; Quise agregar un glicth que aparezca transicionando de invisible a visible por cada daño, pero no me salio

    public void Death()
    {
        //animación de muerte

        //destroy va a destrui todo, y va a esperar 5 segundos
        Destroy(gameObject, 5);
    }

    public void TakeDamege(int damage)
    {
        health -= damage;
        float healthPercent = health / 100;
        Debug.Log(transform.name + " recibió daño. Vida actual: " + health);
        if (health <= 0)
        {
            Death();
        }
        //glitch.UpdateGlitch(healthPercent);

    }


}
