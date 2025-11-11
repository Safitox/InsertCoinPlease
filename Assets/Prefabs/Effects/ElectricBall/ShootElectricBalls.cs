using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootElectricBalls : MonoBehaviour
{

    private Pool electricBallPool;
    static Pool electricEffectPool;
    [SerializeField] private GameObject electricBallPrefab;
    [SerializeField] private GameObject electricEffectPrefab;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] Transform player;
    [SerializeField] private float ballForce;
    [SerializeField] private Transform shootSpawn;
    static Transform effectParent;
    static GameObject electricExplosionEffect;

    static Queue<GameObject> electricEffectsQueue = new Queue<GameObject>();
    void Start()
    {
        if (effectParent == null)
        {
            GameObject effectParentGo = new GameObject("ElectricEffectsPool");
            effectParent = effectParentGo.transform;
        }
        electricBallPool = new Pool(electricBallPrefab, this.transform);
        electricEffectPool = new Pool(electricEffectPrefab, effectParent);
        electricExplosionEffect= Instantiate(explosionEffectPrefab);
        electricExplosionEffect.SetActive(false);
        //player = GameManager.Instance.player;
        InvokeRepeating("Shoot", 2f, 3f);
    }

    public void ShootElectricBall()
    {
        GameObject ball= electricBallPool.GiveMeAnItem();
        ball.transform.position = shootSpawn.position;
        Rigidbody rb= ball.GetComponent<Rigidbody>();
        ball.SetActive(true);
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        Vector3 direction= (player.position - shootSpawn.position).normalized;
        rb.AddForce(ballForce * direction, ForceMode.Impulse);

    }

    public static void AddEffect(Vector3 position)
    {
        GameObject go = electricEffectPool.SpawnAndReturn(position,Quaternion.identity,true).gameObject;
        go.transform.rotation= Random.rotation;
        electricEffectsQueue.Enqueue(go);
    }

    private void FixedUpdate()
    {
        if (electricEffectsQueue.Count > 0)
        {
            GameObject go = electricEffectsQueue.Dequeue();
            go.SetActive(false);
        }
    }

    public static void PlayExplosionEffect(Vector3 position)
    {
        electricExplosionEffect.transform.position = position;
        electricExplosionEffect.SetActive(true);
        
    }


    public void Shoot()
    {
        RaycastHit hit;
        Physics.Raycast(shootSpawn.position, (player.position - shootSpawn.position), out hit, 30);
        {
            if (hit.collider.CompareTag("Player"))
            {
                ShootElectricBall();
            }
        }

    }


    

}
