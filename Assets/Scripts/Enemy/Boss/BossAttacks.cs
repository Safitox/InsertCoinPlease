using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    [SerializeField] BossController bossConntroller;

    public void Shock()
    { 
        bossConntroller.Shock();
    }

    public void Shoot()
    {
        bossConntroller.Shoot();
    }




}
