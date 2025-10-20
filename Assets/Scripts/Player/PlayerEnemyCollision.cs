using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour
{
    [SerializeField] AudioClip hitSound;
    public void HitArea()
    {
        transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 8f + Vector3.forward*8f, ForceMode.Impulse);
        GetComponent<AudioSource>().PlayOneShot(hitSound);
    }

}
