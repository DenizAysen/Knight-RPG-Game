using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Privates
    private int _currentExp;
    private int _level;
    private int _expToNextLevel;
    #endregion

    #region Singleton
    public static LevelManager Instance;

    private void Awake()
    {
        _currentExp = 0;
        _level = 0;
        _expToNextLevel = 100;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        UIManager.Instance.OnExpGained?.Invoke(0f);
        UIManager.Instance.OnLevelGained?.Invoke(GetLevel);
    }
    #endregion
    public Action OnLevelUp;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddExp(50);
        }
    }
    public void AddExp(int amount)
    {
        _currentExp += amount;
        //float current = (float)_currentExp;
        //float toNextLevel = (float)_expToNextLevel;
        UIManager.Instance.OnExpGained?.Invoke((float) _currentExp / _expToNextLevel);
        if(_currentExp >= _expToNextLevel)
        {
            _level++;
            OnLevelUp?.Invoke();
            UIManager.Instance.OnLevelGained?.Invoke(GetLevel);
            _currentExp -= _expToNextLevel;
            UIManager.Instance.OnExpGained?.Invoke(0f);
        }
    }
    public int GetLevel { get { return _level + 1; } }
}
