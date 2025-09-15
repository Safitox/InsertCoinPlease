using UnityEngine;

public class DeadState : EnemyState
{
    float _timer;
    public DeadState(EnemyController c, StateMachine f) : base(c, f) { }

    public override void OnEnter()
    {
        _timer = enemyController.config.deathDelay;
        enemyController.agent.isStopped = true;
        if (enemyController.animator) 
            enemyController.animator.Play("Die");

        //foreach (var col in enemyController.GetComponentsInChildren<Collider>())
        //{
        //      col.enabled = false;
        //}
    }

    public override void Tick()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            GameObject.Destroy(enemyController.gameObject);
    }
}
