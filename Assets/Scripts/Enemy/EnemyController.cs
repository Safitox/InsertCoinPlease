using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("Config")]
    public EnemyConfig config;
    public Transform[] patrolPoints;

    [Header("Refs")]
    public Animator animator=>GetComponent<Animator>();
    public EnemyPlayerDEtection detection;
    public EnemyHealth health;
    public GameObject stars;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform playerDetected;      // jugador actual
    [HideInInspector] public int patrolIndex;

    StateMachine _stateMachine;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();

        // Crear estados y registrarlos
        _stateMachine.AddState(new IdleState(this, _stateMachine));
        _stateMachine.AddState(new PatrolState(this, _stateMachine));
        _stateMachine.AddState(new ChaseState(this, _stateMachine));
        _stateMachine.AddState(new AttackState(this, _stateMachine));
        _stateMachine.AddState(new StunnedState(this, _stateMachine));
        _stateMachine.AddState(new DeadState(this, _stateMachine));

        health.OnDamaged += HandleDamaged;
        health.OnDied += HandleDied;
    }

    void Start()
    {
        agent.speed = config.walkSpeed;
        _stateMachine.Change<IdleState>();
    }

    void Update()
    {
        // playerDetected desde percepción
        playerDetected = detection.CurrentTarget;
        _stateMachine.Tick();

        
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void FixedUpdate() => _stateMachine.FixedTick();

    public void HandleDamaged() => _stateMachine.Change<StunnedState>();
    void HandleDied() => _stateMachine.Change<DeadState>();

    public bool InAttackRange()
    {
        if (!playerDetected) return false;
        return Vector3.SqrMagnitude(transform.position-playerDetected.position)  <= config.attackRange * config.attackRange;
    }

    public bool HasLineOfSight()
    {
        return detection.HasLineOfSight;
    }
}
