using UnityEngine;

public class ElectricityBallSpawnDamage : MonoBehaviour
{
    [SerializeField] private int amount = 20;
    [SerializeField] private float radius = 5f;
    [SerializeField] GameObject pf_RaySpawn;


    //daño al jugador
    [SerializeField] private float damage = 10f;
    [SerializeField] private float damageOnSpread = 1f;

    bool playerHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playerHit)
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.Damage(damage);
                playerHit = true;
            }
        }
        Explo();
        Destroy(gameObject);
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
                        Health playerHealth = hit.collider.gameObject.GetComponent<Health>();
                        if (playerHealth != null)
                        {
                            playerHealth.Damage(damageOnSpread);
                        }
                    }
                }
                GameObject go = Instantiate(pf_RaySpawn, hit.point, Quaternion.LookRotation(hit.point - transform.position));
                Destroy(go, Random.Range(2f,3f));
            }
        }


    }


}
