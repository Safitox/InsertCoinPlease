using UnityEngine;

public class WaveLife : MonoBehaviour
{
    public float lifeTime = 4f;
    public int lifeToTaker = 30;


    private void OnEnable()
    {
        Invoke(nameof(Disable), lifeTime);
    }


    private void Disable()
    {
        gameObject.SetActive(false);
        CancelInvoke();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthManager>()?.TakeDamage(lifeToTaker);
            gameObject.SetActive(false);
        }
    }
}
