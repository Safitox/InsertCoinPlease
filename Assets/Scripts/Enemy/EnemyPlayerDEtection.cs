using UnityEngine;

public class EnemyPlayerDEtection : MonoBehaviour
{
    public LayerMask obstacleMask;
    public Transform eyes;
    public EnemyConfig config;
    public Transform player;

    public bool HasLineOfSight { get; private set; }
    public Transform CurrentTarget { get; private set; }
    float _loseTimer;

    private void Start()
    {
        player = GameManager.Instance.player;
    }
    void Update()
    {
        HasLineOfSight = false;
        if (!player) 
        { 
            CurrentTarget = null; 
            return; 
        }

        Vector3 dir = player.position - eyes.position;
        float dist = dir.magnitude;
        if (dist > config.sightRange) { Lose(); return; }

        float angle = Vector3.Angle(eyes.forward, dir.normalized);
        if (angle > config.sightAngle * 0.5f) 
        {
            Lose();
            return; 
        }

        if (Physics.Raycast(eyes.position, dir.normalized, out var hit, dist, ~obstacleMask))
        {
            if (hit.transform == player)
            {
                HasLineOfSight = true;
                CurrentTarget = player;
                _loseTimer = config.loseSightTime;
            }
            else Lose();
        }
    }

    void Lose()
    {
        _loseTimer -= Time.deltaTime;
        if (_loseTimer <= 0f) 
            CurrentTarget = null;
    }
}
