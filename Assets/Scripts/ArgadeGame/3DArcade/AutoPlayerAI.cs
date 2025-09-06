using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AutoPlayerAI : MonoBehaviour
{
    [Header("Objetivo y ataque")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float retargetInterval = 0.4f;
    [SerializeField] private float attackRange = 1.7f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackDamage = 35f;

    [Header("Salto")]
    [SerializeField] private float jumpArcHeight = 0.9f;
    [SerializeField] private float jumpDuration = 0.35f;

    //Referencias
    NavMeshAgent agent => GetComponent<NavMeshAgent>();
    Transform currentTarget;
    float nextRetarget;
    float nextAttack;

    //Estados
    bool isCrossing;

    void Awake()
    {
        agent.autoTraverseOffMeshLink = false; // anulo el default porque apesta... uso corrutina
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance; // menos zig-zag
    }


    void Update()
    {
        //TODO: Mover todo a corrutina
        // Buscar objetivo cada X seg
        if (Time.time >= nextRetarget)
        {
            currentTarget = FindClosestEnemy();
            nextRetarget = Time.time + retargetInterval;
            if (currentTarget != null)
                agent.SetDestination(currentTarget.position);
        }

        if (currentTarget == null) return;

        // Actualizar destino constantemente (enemigo se mueve)
        if (!agent.pathPending)
            agent.SetDestination(currentTarget.position);

        // Ataque
        if (Time.time >= nextAttack && DistanceXZ(transform.position, currentTarget.position) <= attackRange + 0.1f)
            TryAttack();

        // Salto por OffMeshLink
        if (agent.isOnOffMeshLink && !isCrossing)
            StartCoroutine(CrossLinkSmooth(agent.currentOffMeshLinkData));
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies.Length == 0) return null;

        Transform best = null; float bestSqr = float.PositiveInfinity;
        Vector3 p = transform.position;
        foreach (var e in enemies)
        {
            if (e == null) continue;
            float d = (e.transform.position - p).sqrMagnitude;
            if (d < bestSqr) { bestSqr = d; best = e.transform; }
        }
        return best;
    }

    void TryAttack()
    {
        //TODO: Hacer un ataque distinto... considerar espadas en lugar de disparos
        nextAttack = Time.time + attackCooldown;

        //Intento directo al objetivo
        if (currentTarget != null)
        {
            if (currentTarget.TryGetComponent<Health>(out var hpDirect))
            {
                hpDirect.Damage(attackDamage);
                return;
            }
            // Si el Health está en el padre/hijo
            var hpParent = currentTarget.GetComponentInParent<Health>();
            if (hpParent != null) { hpParent.Damage(attackDamage); return; }
            var hpChild = currentTarget.GetComponentInChildren<Health>();
            if (hpChild != null) { hpChild.Damage(attackDamage); return; }
        }

        //Fallback: esfera de golpeo alrededor del frente del jugador 
        Vector3 center = transform.position + transform.forward * (attackRange * 0.6f);
        float radius = attackRange * 0.8f;
        var hits = Physics.OverlapSphere(center, radius, ~0, QueryTriggerInteraction.Collide);
        foreach (var h in hits)
        {
            var hp = h.GetComponent<Health>() ?? h.GetComponentInParent<Health>() ?? h.GetComponentInChildren<Health>();
            if (hp != null) { hp.Damage(attackDamage); }
        }
    }

    IEnumerator CrossLinkSmooth(OffMeshLinkData data)
    {
        //TODO: Emprolijar... sigue siendo una porquería
        isCrossing = true;

        Vector3 start = transform.position;
        Vector3 end = data.endPos + Vector3.up * 0.05f;

        float t = 0f;
        agent.updatePosition = false; // forzar movimiento manual

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.05f, jumpDuration);
            // arco simple
            Vector3 mid = (start + end) * 0.5f + Vector3.up * jumpArcHeight;
            Vector3 p1 = Vector3.Lerp(start, mid, t);
            Vector3 p2 = Vector3.Lerp(mid, end, t);
            transform.position = Vector3.Lerp(p1, p2, t);

            // Cambio de orientación
            Vector3 dir = (end - transform.position);
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.25f);

            yield return null;
        }

        agent.CompleteOffMeshLink();
        agent.updatePosition = true;
        isCrossing = false;
    }

    float DistanceXZ(Vector3 a, Vector3 b)
    {
        //TODO: Vector2.Distance en lugar de esto o usar magnitud
        a.y = b.y = 0;
        return Vector3.Distance(a, b);
    }

    // Gizmo de golpe
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + transform.forward * (attackRange * 0.6f);
        Gizmos.DrawWireSphere(center, attackRange * 0.8f);
    }
}
