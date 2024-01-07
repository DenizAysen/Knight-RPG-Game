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
    private Animator _anim;

    private float _currentHealth;
    private bool _isShielded;
    #endregion
    #region Unity Methods
    private void Awake()
    {
        _currentHealth = maxHealth;
        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(30);
        }
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
            Death();
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
        Debug.Log(_currentHealth);
    }
    private void UpdateHealthImage() => healthBar.fillAmount = (_currentHealth / maxHealth);
    private void Death() => _anim.SetBool("Death", true); 
    #endregion
    #region Properties
    public bool IsShielded { get { return _isShielded; } set { _isShielded = value; } } 
    #endregion
    //public bool IsShielded()
    //{
    //    return _isShielded;
    //}
}
