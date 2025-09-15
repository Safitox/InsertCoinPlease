using UnityEngine;

public class IdleState : EnemyState
{
    float _timer;

    public IdleState(EnemyController c, StateMachine s) : base(c, s) { }

    public override void OnEnter()
    {
        _timer = Random.Range(0.5f, 1.5f);
        enemyController.agent.isStopped = true;
        if (enemyController.animator) enemyController.animator.Play("Idle");
    }

    public override void Tick()
    {
        if (enemyController.playerDetected && CanSeeTarget()) 
        { 
            stateMachine.Change<ChaseState>(); 
            return; 
        }

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            stateMachine.Change<PatrolState>();
    }
}
