using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour
{
    public void HitArea()
    {
        transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 8f + Vector3.forward*8f, ForceMode.Impulse);
    }

}
