using UnityEngine;

public abstract class EnemyState : IState
{
    protected readonly EnemyController enemyController;
    protected readonly StateMachine stateMachine;

    protected EnemyState(EnemyController controller, StateMachine stm)
    { enemyController = controller; stateMachine = stm; }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void Tick() { }
    public virtual void FixedTick() { }

    protected bool CanSeeTarget() => enemyController.playerDetected && enemyController.HasLineOfSight();
}
