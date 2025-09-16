using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyState
{
    float _waitTimer;
    bool _walking = false;

    public PatrolState(EnemyController c, StateMachine s) : base(c, s) { }

    public override void OnEnter()
    {
        _walking = false;
        enemyController.agent.isStopped = false;
        enemyController.agent.speed = enemyController.config.walkSpeed;
        GoNext();

    }

    void GoNext()
    {
        if (enemyController.patrolPoints == null || enemyController.patrolPoints.Length == 0)
        { 
            stateMachine.Change<IdleState>(); 
            return; 
        }

        enemyController.patrolIndex = (enemyController.patrolIndex + 1) % enemyController.patrolPoints.Length; // vago... pero como no espero que duren mucho...
        enemyController.agent.SetDestination(enemyController.patrolPoints[enemyController.patrolIndex].position);
        _waitTimer = enemyController.config.patrolWait;
        if (!_walking)
        {
            if (enemyController.animator)
                enemyController.animator.Play("Walk");
            _walking = true;
        }
    }

    public override void Tick()
    {
        if (enemyController.playerDetected && CanSeeTarget()) { stateMachine.Change<ChaseState>(); return; }

        if (!enemyController.agent.pathPending && enemyController.agent.remainingDistance <= 0.2f)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f) 
                GoNext();
        }
    }
}

