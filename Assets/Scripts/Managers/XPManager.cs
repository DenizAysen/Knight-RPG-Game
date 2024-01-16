using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    #region Privates
    private int _currentExp;
    private int _playerLevel;
    private int _expToNextLevel;
    #endregion

    #region Singleton
    public static XPManager Instance;

    private void Awake()
    {
        _currentExp = 0;
        _playerLevel = 0;
        _expToNextLevel = 70;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        UIManager.Instance.OnExpGained?.Invoke(0f);
        UIManager.Instance.OnLevelGained?.Invoke(GetPlayerLevel);
    }
    #endregion
    public Action OnLevelUp;
    #region Unity Methods
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        EnemyHealth.OnDeath += AddExp;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void UnSubscribeEvents()
    {
        EnemyHealth.OnDeath += AddExp;
    } 
    #endregion
    public void AddExp(int amount)
    {
        _currentExp += amount;
        //float current = (float)_currentExp;
        //float toNextLevel = (float)_expToNextLevel;
        UIManager.Instance.OnExpGained?.Invoke((float)_currentExp / _expToNextLevel);
        if (_currentExp >= _expToNextLevel)
        {
            _playerLevel++;
            OnLevelUp?.Invoke();
            UIManager.Instance.OnLevelGained?.Invoke(GetPlayerLevel);
            _currentExp -= _expToNextLevel;
            UIManager.Instance.OnExpGained?.Invoke(0f);
        }
    }
    public int GetPlayerLevel { get { return _playerLevel + 1; } }
}
