using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    [Header("Movimiento")]
    public float walkSpeed = 2.5f;
    public float chaseSpeed = 4.0f;
    public float patrolWait = 1.5f;

    [Header("Detección")]
    public float sightRange = 12f;
    public float sightAngle = 120f;
    public float loseSightTime = 2f;

    [Header("Combate")]
    public float attackRange = 1.6f;
    public float attackCooldown = 1.2f;
    public int damage = 1;

    [Header("Daño y muerte")]
    public float stunnedTime = 1.2f;
    public float deathDelay = 2.0f;
}
