using UnityEngine;
using System;


public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float currentHealth;
    public event Action<Health> OnDeath;

    void Awake() => currentHealth = maxHealth;

    public void Damage(float amount)
    {
        if (currentHealth <= 0f) return;
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
