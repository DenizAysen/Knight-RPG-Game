using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Image healthBar;
    #endregion

    #region Privates
    private float _currentHealth;
    private bool _isShielded;
    #endregion
    #region Unity Methods
    private void Awake()
    {
        _currentHealth = maxHealth;
    }
    private void Start()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        XPManager.Instance.OnLevelUp += OnLevelUp;
    }

    private void OnLevelUp()
    {
        _currentHealth = _currentHealth == 100f ? _currentHealth : maxHealth;

        UpdateHealthImage();
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void UnSubscribeEvents()
    {
        XPManager.Instance.OnLevelUp -= OnLevelUp;
    }
    #endregion
    #region Methods
    public void TakeDamage(float damageAmount)
    {
        if (_isShielded)
            return;

        _currentHealth -= damageAmount;

        UpdateHealthImage();

        if (_currentHealth <= 0f)
        {
            LevelManager.OnPlayerDeath?.Invoke();
        }
    }
    public bool IsPlayerDead()
    {
        return _currentHealth <= 0f;
    }
    public void HealPlayer(float healAmount)
    {
        _currentHealth += healAmount;
        _currentHealth = _currentHealth > 100f ? maxHealth : _currentHealth;

        UpdateHealthImage();
    }
    private void UpdateHealthImage() => healthBar.fillAmount = (_currentHealth / maxHealth);
    #endregion
    #region Properties
    public bool IsShielded { get { return _isShielded; } set { _isShielded = value; } } 
    #endregion
    
}
