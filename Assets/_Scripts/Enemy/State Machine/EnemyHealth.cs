using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    //Provisorio 3 golpes y muere
    public int maxHP = 3;
    int _hp;

    public event Action OnDamaged;
    public event Action OnDied;

    void Awake() => _hp = maxHP;

    public void Damage(int amount)
    {
        if (_hp <= 0) return;
        _hp -= amount;
        OnDamaged?.Invoke();
        if (_hp <= 0) OnDied?.Invoke();
    }
}
