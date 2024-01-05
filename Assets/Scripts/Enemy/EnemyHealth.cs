using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;
    private void Awake()
    {
        _currentHealth = maxHealth;
    }
    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;
    }
}
