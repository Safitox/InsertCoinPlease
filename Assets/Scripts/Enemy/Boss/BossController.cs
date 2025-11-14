using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] Animator bossAnimator;
    [SerializeField] float ballMinFrequency = 2f;
    [SerializeField] float ballMaxFrequency = 3f;
    [SerializeField] float shockMinFrequency = 3f;
    [SerializeField] float shockMaxFrequency = 6f;
    [SerializeField] ShootElectricBalls shootElectricBalls;
    [SerializeField] ShockWaveEmitter shockWaveEmitter;
    Rigidbody rb=> GetComponent<Rigidbody>();
    Transform player;
    bool dead = false;
    

    private void OnEnable()
    {
        player= GameObject.FindWithTag("Player").transform;
        Invoke(nameof(StartAttacks), 3f);
        
    }

    public void StartAttacks()
    {
               StartCoroutine(BallAttack());
    }


    private void FixedUpdate()
    {
        if (dead) return;
        transform.LookAt(player.position);
    }

    public void KillBoss()
    {
        StopAllCoroutines();
        CancelInvoke();
        dead = true;
        bossAnimator.SetBool("Die",true);
    }



    IEnumerator BallAttack()
    {
        float ballAttackInterval = Random.Range(ballMinFrequency, ballMaxFrequency);
        yield return new WaitForSeconds(ballAttackInterval);
        bossAnimator.SetTrigger("EnergyBall");
        StartCoroutine(ShockAttack());


    }

    public void Shoot()
    {
        shootElectricBalls.ShootElectricBall();

    }

    IEnumerator  ShockAttack()
    {
        float shockAttackInterval = Random.Range(shockMinFrequency, shockMaxFrequency);
        yield return new WaitForSeconds(shockAttackInterval);
        bossAnimator.SetTrigger("Shock");
        StartCoroutine(BallAttack());
    }

    public void Shock()
    {
        shockWaveEmitter.EmitShockWave();

    }

}
