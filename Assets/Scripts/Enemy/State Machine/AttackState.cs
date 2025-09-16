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
                // TODO: PROGRAMAR ATAQUE
                
                if (enemyController.animator) enemyController.animator.Play("Attack", 0, 0f);
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
