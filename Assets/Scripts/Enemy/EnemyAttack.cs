using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage;
    PlayerHealth _playerHealth;
    private void OnTriggerEnter(Collider other)
    {
        _playerHealth = other.GetComponent<PlayerHealth>();
        Debug.Log(other.gameObject.name);
        _playerHealth?.TakeDamage(attackDamage);
    }
}
