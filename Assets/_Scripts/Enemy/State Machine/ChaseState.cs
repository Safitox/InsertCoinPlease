using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyController c, StateMachine s) : base(c, s) { }

    public override void OnEnter()
    {
        enemyController.agent.isStopped = false;
        enemyController.agent.speed = enemyController.config.chaseSpeed;
        if (enemyController.animator) 
            enemyController.animator.Play("Run");
    }

    public override void Tick()
    {
        if (!enemyController.playerDetected) 
        { 
            stateMachine.Change<IdleState>(); 
            return; 
        }

        enemyController.agent.SetDestination(enemyController.playerDetected.position);

        if (enemyController.InAttackRange())
            stateMachine.Change<AttackState>();
        else if (!CanSeeTarget())
            stateMachine.Change<PatrolState>(); 
    }
}
