using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage;
    EnemyHealth _enemyHealth;
    private void OnTriggerEnter(Collider other)
    {
        _enemyHealth = other.GetComponent<EnemyHealth>();
        _enemyHealth?.TakeDamage(attackDamage);
    }
}
