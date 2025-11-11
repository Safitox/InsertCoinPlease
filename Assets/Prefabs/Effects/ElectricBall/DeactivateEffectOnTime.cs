using UnityEngine;

public class DeactivateEffectOnTime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        Invoke("KillMe", 1f);
    }

    void KillMe()
    {
        gameObject.SetActive(false);
    }
}

