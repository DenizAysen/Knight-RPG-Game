using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage;
    [SerializeField] private float shakeAmount = 2f;
    EnemyHealth _enemyHealth;
    private void OnTriggerEnter(Collider other)
    {
        _enemyHealth = other.GetComponent<EnemyHealth>();
        _enemyHealth?.TakeDamage(attackDamage);
        PlayerAnimationController.OnHitDustInit?.Invoke(other.gameObject);
        ScreenShake.Instance.Shake(shakeAmount);
    }
}
