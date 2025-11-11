using UnityEngine;
using UnityEngine.Rendering;

public class ElectricBallElectricityShake : MonoBehaviour
{
    [SerializeField] float intensity = 0.2f;
    private void FixedUpdate()
    {
        float range = 1 + intensity;
        transform.rotation *= Quaternion.Euler(new Vector3(
            Random.Range( -range, range),
            Random.Range(-range, range),
            Random.Range(-range, range)
        ));
    }
}
