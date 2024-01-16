using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Privates
    BossState _bossState;
    private List<GameObject> enemiesOnTheScene = new List<GameObject>();
    #endregion

    #region SerializedFields
    [SerializeField] private GameObject bossHealthBar;
    #endregion

    #region Actions
    public static Action<GameObject> OnSkeletonDied;
    public static Action OnAllSkeletonsDied;
    public static Action OnBossDeath;
    public static Action OnPlayerDeath;
    #endregion

    private void Awake()
    {
        _bossState = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossState>();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        OnSkeletonDied += OnSkeletonDeath;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void UnSubscribeEvents()
    {
        OnSkeletonDied -= OnSkeletonDeath;
    }
    private void OnSkeletonDeath(GameObject skeleton)
    {
        if (enemiesOnTheScene.Contains(skeleton))
        {
            enemiesOnTheScene.Remove(skeleton);
        }
        if (enemiesOnTheScene.Count <= 0)
        {
            OnAllSkeletonsDied?.Invoke();
        }
    }
    private void Start()
    {
        AudioManager.Instance.PlayGameMusic();
        InitEnemyList();
    }
    private void InitEnemyList()
    {
        EnemyWaypointTracker[] enemyList = FindObjectsOfType<EnemyWaypointTracker>();
        foreach (EnemyWaypointTracker enemy in enemyList)
        {
            enemiesOnTheScene.Add(enemy.gameObject);
        }
        if (enemiesOnTheScene.Count == 0)
            OnAllSkeletonsDied?.Invoke();
    }
    private void Update()
    {
        //if (_bossState.CurrentBossState == BossStates.Sleep || _bossState.CurrentBossState == BossStates.Death)
        //    bossHealthBar?.SetActive(false);

        //else
        //    bossHealthBar?.SetActive(true);

        //if (Boss.bossDeath == true)
        //    Invoke("RestartScene", 5f);
        //else if (_playerHealth.IsPlayerDead())
        //    Invoke("RestartScene", 5f);
    }
}
