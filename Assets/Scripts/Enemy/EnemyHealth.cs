using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float _currentHealth;
    private EnemyAnimationController animationController;
    private void Awake()
    {
        _currentHealth = maxHealth;
        animationController = GetComponent<EnemyAnimationController>();
    }
    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth > 0)
            animationController.PlayHitAnimation();
        else
            animationController.PlayDeathAnimation();
        Debug.Log("Enemy health : "+_currentHealth);
    }
}
