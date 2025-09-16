using UnityEngine;

public class StunnedState : EnemyState
{
    float _timer;

    public StunnedState(EnemyController c, StateMachine s) : base(c, s) { }

    public override void OnEnter()
    {
        //enemyController.health.Damage(1); // Quito 1 de vida por cada salto en la cabeza
        _timer = enemyController.config.stunnedTime;
        enemyController.agent.isStopped = true;
        if (enemyController.animator) enemyController.animator.Play("Stunned");
        enemyController.stars.SetActive(true);
        // TODO: Acciones al ser aturdido
    }

    public override void Tick()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            // Si ve al jugador, perseguir; si no, patrullar
            if (enemyController.playerDetected && enemyController.HasLineOfSight())
                stateMachine.Change<ChaseState>();
            else
                stateMachine.Change<PatrolState>();
        }
    }

    public override void OnExit()
    {

        enemyController.stars.SetActive(false);
    }
}
