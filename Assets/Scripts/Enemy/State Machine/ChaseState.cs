using UnityEngine;

public class ChaseState : EnemyState
{

    float _timer;
    public ChaseState(EnemyController c, StateMachine s) : base(c, s) { }

    public override void OnEnter()
    {
        //Debug.Log("Chase");
        enemyController.agent.isStopped = false;
        enemyController.agent.speed = enemyController.config.chaseSpeed;
        if (enemyController.animator) 
            enemyController.animator.Play("Run");
        _timer = enemyController.config.loseSightTime;
    }

    public override void Tick()
    {
        if (!enemyController.playerDetected) 
        { 
            _timer-= Time.deltaTime;
            if (_timer <= 0f)
            {
                stateMachine.Change<IdleState>();
                return;
            }
        }
        else 
            _timer = enemyController.config.loseSightTime;
        if (enemyController.playerDetected)
            enemyController.agent.SetDestination(enemyController.playerDetected.position);

        if (enemyController.InAttackRange())
            stateMachine.Change<AttackState>();
        //else if (!CanSeeTarget() )
        //    stateMachine.Change<PatrolState>(); 
    }

    //public override void OnExit()
    //{
    //    UnityEngine.Debug.Log("Lost");
    //}
}
