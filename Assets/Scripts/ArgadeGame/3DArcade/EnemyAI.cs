using UnityEngine;
using UnityEngine.AI;

//TODO: Componentes para detectar player y atacar o perseguirlo

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float patrolRadius = 3f;
    [SerializeField] private float idleTime = 1.5f;

    NavMeshAgent agent;
    float waitUntil;

    void Awake() => agent = GetComponent<NavMeshAgent>();

    void Start() => SetRandomPatrolPoint();

    void Update()
    {
        if (Time.time < waitUntil) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            waitUntil = Time.time + idleTime;
            SetRandomPatrolPoint();
        }
    }

    void SetRandomPatrolPoint()
    {
        Vector2 rnd = Random.insideUnitCircle * patrolRadius;
        Vector3 dest = transform.position + new Vector3(rnd.x, 0, rnd.y);
        if (NavMesh.SamplePosition(dest, out var hit, 2f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }
}
