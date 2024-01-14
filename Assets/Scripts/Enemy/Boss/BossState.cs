using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
   
    #region Privates
    private Transform _playerTransform;
    private BossStates _bossState = BossStates.Sleep;
    private float _distanceToTarget;
    private EnemyHealth _enemyHealth;
    #endregion
    #region Unity Methods
    private void Awake()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _bossState = BossStates.Sleep;
    }
    private void Start()
    {        
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        SceneManagement.OnAllSkeletonsDied += OnAllSkeletonsDied;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();        
    }

    private void UnSubscribeEvents()
    {
        SceneManagement.OnAllSkeletonsDied -= OnAllSkeletonsDied;
    }

    private void Update()
    {
        SetBossState();
    } 
    #endregion
    private void SetBossState()
    {
        _distanceToTarget = Vector3.Distance(transform.position , _playerTransform.position);

        if(_bossState == BossStates.Sleep)
        {
            //int enemyCount = FindObjectsOfType<EnemyWaypointTracker>().Length;
            if(_enemyHealth.GetCurrentHealth() < _enemyHealth.GetMaxHealth())
            {
                _bossState = BossStates.None;
            }
            else if(_distanceToTarget < 4f)
            {
                _bossState = BossStates.None;
            }
            //else if(enemyCount == 0)
            //{
            //    _bossState = BossStates.None;
            //}
            else
            {
                _bossState = BossStates.Sleep;
            }
        }
        else if( _bossState != BossStates.Sleep || _bossState != BossStates.Death)
        {
            if(_distanceToTarget > 4f && _distanceToTarget < 8f)
            {
                _bossState = BossStates.Chase;
            }
            else if (_distanceToTarget > 8f && _distanceToTarget <= 12f)
            {
                _bossState = BossStates.Shoot;
            }
            else if(_distanceToTarget > 12f)
            {
                _bossState = BossStates.Patrol;
            }
            else if(_distanceToTarget <= 4f)
            {
                _bossState = BossStates.Attack;
            }
            else
            {
                _bossState = BossStates.None;
            }
        }
        if(_enemyHealth.GetCurrentHealth() <= 0f)
        {
            _bossState = BossStates.Death;
        }
    }
    private void OnAllSkeletonsDied() => _bossState = BossStates.None;
    public BossStates CurrentBossState { get {return _bossState; } set { _bossState = value; } } 

}
public enum BossStates
{
    None,
    Sleep,
    Patrol,
    Chase,
    Attack,
    Shoot,
    Death
}