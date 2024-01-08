using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Image healthBar; 
    #endregion

    #region Privates
    private float _currentHealth;
    private EnemyAnimationController animationController; 
    #endregion
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
            canvas.gameObject.SetActive(false);
        }
        Debug.Log("Enemy health : "+_currentHealth);
    }
    public bool IsEnemyAlive() => _currentHealth > 0f;
}
