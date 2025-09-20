using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    float timer = 0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (timer > 0)
                return;
            if (other.GetComponentInParent<Rigidbody>().linearVelocity.y >= 0)
                return;
            timer = GetComponent<EnemyController>().config.stunnedTime;
            other.GetComponentInParent<PlayerEnemyCollision>().HitArea();
            GetComponent<EnemyController>().health.Damage(1);
        }
    }


    private void FixedUpdate()
    {
        if (timer <= 0)
            return;
        timer-= Time.fixedDeltaTime;
    }

}
