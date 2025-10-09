using UnityEngine;

public class AttackState : EnemyState
{
    float _cooldown;

    public AttackState(EnemyController c, StateMachine f) : base(c, f) { }

    public override void OnEnter()
    {
        _cooldown = 0f;
        enemyController.agent.isStopped = true;
        if (enemyController.animator) enemyController.animator.Play("Attack");
    }

    public override void Tick()
    {
        if (!enemyController.playerDetected) 
        { 
            stateMachine.Change<IdleState>(); 
            return; 
        }

        // Dirigir al jugador
        Vector3 dir = (enemyController.playerDetected.position - enemyController.transform.position);
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
            enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);

        _cooldown -= Time.deltaTime;

        if (enemyController.InAttackRange())
        {
            if (_cooldown <= 0f)
            {
               
                if (enemyController.animator) 
                    enemyController.animator.Play("Attack", 0, 0f);
                if (enemyController.config.stunTimeOnHit!=0f) 
                    enemyController.playerDetected.GetComponent<ThirdPersonController>().Stun(enemyController.config.stunTimeOnHit);

                //Aplico daño (ema)
                IHeathManager playerHealth = enemyController.playerDetected.GetComponent<IHeathManager>();
                if (playerHealth != null )
                {
                    if (playerHealth.Health > 0)
                    { 
                        Debug.Log("El daño es de:" + enemyController.config.damage);
                        playerHealth.TakeDamege(enemyController.config.damage);
                    }

                }




                _cooldown = enemyController.config.attackCooldown;
            }
        }
        else
        {
            enemyController.agent.isStopped = false;
            stateMachine.Change<ChaseState>();
        }
    }
}
