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
    private const string _boss = "Boss";
    private const string _enemy = "Enemy";

    private EnemyAnimationController animationController;
    #endregion
    public static Action<int> OnDeath;
    private void Awake()
    {
        animationController = GetComponent<EnemyAnimationController>();
        Init();
    }
    public void TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;

        _currentHealth = _currentHealth < 0f ? 0f : _currentHealth;

        healthBar.fillAmount = _currentHealth / maxHealth;

        if (_currentHealth > 0)
        {
            animationController?.PlayHitAnimation();

            if(gameObject.tag == _boss)
            {
                AudioManager.Instance.PlaySFX(6);
            }

            else if(gameObject.tag == _enemy)
                AudioManager.Instance.PlaySFX(3);
        }

        else
        {
            Canvas canvas = healthBar.gameObject.GetComponentInParent<Canvas>();
            OnDeath?.Invoke(expAmount);
            if(canvas != null && canvas.renderMode == RenderMode.WorldSpace)
                canvas.gameObject.SetActive(false);
            if (gameObject.tag == _boss)
                LevelManager.OnBossDeath?.Invoke();

            targetCollider?.gameObject.SetActive(false);
        }
    }
    private void Init()
    {
        if (gameObject.tag == _boss)
            maxHealth = UnityEngine.Random.Range(150, 200);
        else if (gameObject.tag == _enemy)
            maxHealth = UnityEngine.Random.Range(30, 50);

        _currentHealth = maxHealth;
    }
    public bool IsEnemyAlive() => _currentHealth > 0f;
    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => maxHealth;
}
