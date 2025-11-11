using UnityEngine;

public class ElectricityBallSpawnDamage : MonoBehaviour
{
    [SerializeField] private int amount = 20;
    [SerializeField] private float radius = 5f;


    //daño al jugador
    [SerializeField] private int damage = 10;
    [SerializeField] private int damageOnSpread = 1;

    bool playerHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playerHit)
        {
            HealthManager playerHealth = collision.gameObject.GetComponent<HealthManager>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                playerHit = true;
            }
        }
        Explo();
        gameObject.SetActive(false);
        playerHit = false;
    }

    private void Explo() {

        for (int i = 0; i < amount; i++)
        {
            RaycastHit hit;
            Vector3 randomDirection = Random.onUnitSphere;
            Physics.Raycast(transform.position+Vector3.up, randomDirection, out hit, radius);
            if (hit.collider != null) 
            {
                if (!playerHit) {
                    if (hit.collider.CompareTag("Player") )
                    {
                        HealthManager playerHealth = hit.collider.gameObject.GetComponent<HealthManager>();
                        if (playerHealth != null)
                        {
                            playerHealth.TakeDamage(damageOnSpread);
                        }
                    }
                }
                ShootElectricBalls.AddEffect(hit.point);


            }
        }

        ShootElectricBalls.PlayExplosionEffect(transform.position);


    }


}
