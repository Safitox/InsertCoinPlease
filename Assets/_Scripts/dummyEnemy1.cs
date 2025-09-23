using UnityEngine;

public class dummyEnemy1 : MonoBehaviour
{
    public GameObject bump;
    private void OnCollisionEnter(Collision collision)
    {

        {
            GetComponent<Rigidbody>().isKinematic = false;
            Destroy(GetComponent<Animator>());
            gameObject.layer = LayerMask.NameToLayer("Default");
            bump.SetActive(true);
        }
    }
}
