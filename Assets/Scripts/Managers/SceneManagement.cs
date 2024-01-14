using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement : MonoBehaviour
{
    BossState _bossState;
    private PlayerHealth _playerHealth;
    private List<GameObject> enemiesOnTheScene = new List<GameObject>();

    [SerializeField] private GameObject bossHealthBar;

    public static Action<GameObject> OnSkeletonDied;
    public static Action OnAllSkeletonsDied;
    #region Unity Methods
    private void Awake()
    {
        _bossState = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossState>();
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
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

    private void Start()
    {
        AudioManager.Instance.PlayGameMusic();
        InitEnemyList();
    } 
    void Update()
    {
        if (_bossState.CurrentBossState == BossStates.Sleep || _bossState.CurrentBossState == BossStates.Death)
        {
            bossHealthBar?.SetActive(false);
        }
        else
            bossHealthBar?.SetActive(true);

        if(Boss.bossDeath == true)
        {
            Invoke("RestartScene", 5f);           
        }
        else if (_playerHealth.IsPlayerDead())
        {
            Invoke("RestartScene", 5f);
        }
    }
    #endregion
    private void InitEnemyList()
    {
        EnemyWaypointTracker[] enemyList = FindObjectsOfType<EnemyWaypointTracker>();
        foreach (EnemyWaypointTracker enemy in enemyList)
        {
            enemiesOnTheScene.Add(enemy.gameObject);
        }
        Debug.Log(enemiesOnTheScene.Count);
    }
    void RestartScene() => SceneManager.LoadScene(0);
    private void OnSkeletonDeath(GameObject skeleton)
    {
        if (enemiesOnTheScene.Contains(skeleton))
        {
            enemiesOnTheScene.Remove(skeleton);
        }
        if(enemiesOnTheScene.Count <= 0)
        {
            OnAllSkeletonsDied?.Invoke();
        }
    }
}
