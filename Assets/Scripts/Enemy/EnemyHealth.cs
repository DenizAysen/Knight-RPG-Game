using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Image healthBar;
    [SerializeField] private SphereCollider targetCollider;
    [SerializeField] private int expAmount = 10;
    #endregion

    #region Privates
    private float _currentHealth;
    private EnemyAnimationController animationController;
    #endregion
    public static Action<int> OnDeath;
    private void Awake()
    {
        _currentHealth = maxHealth;
        animationController = GetComponent<EnemyAnimationController>();
    }
    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;

        _currentHealth = _currentHealth < 0f ? 0f : _currentHealth;

        healthBar.fillAmount = _currentHealth / maxHealth;

        if (_currentHealth > 0)
            animationController.PlayHitAnimation();

        else
        {
            Canvas canvas = healthBar.gameObject.GetComponentInParent<Canvas>();
            OnDeath?.Invoke(expAmount);
            canvas?.gameObject.SetActive(false);
            targetCollider?.gameObject.SetActive(false);
        }
        Debug.Log("Enemy health : "+_currentHealth);
    }
    public bool IsEnemyAlive() => _currentHealth > 0f;
}
