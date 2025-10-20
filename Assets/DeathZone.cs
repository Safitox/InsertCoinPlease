using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthManager hm = other.GetComponent<HealthManager>();
            if(hm != null)
            {
                hm.Death();
            }
        }
    }
}
