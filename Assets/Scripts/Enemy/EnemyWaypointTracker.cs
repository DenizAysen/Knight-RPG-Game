using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWaypointTracker : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private Transform[] wayPoints;
    [Header("Movement Settings")]
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float patrolTime = 10f;
    [SerializeField] private float walkDistance = 8f;
    [Header("Attack Settings")]
    [SerializeField] private float attackDistance = 1.4f;
    [SerializeField] private float attackRate = 1f;
    #endregion

    #region Privates
    private Transform playerTransform;
    private NavMeshAgent _agent;
    private EnemyAnimationController animationController;
    private EnemyHealth _enemyHealth;

    private float _currentAttackTime;
    private Vector3 _nextDestination;
    private int _waypointIndex;
    #endregion
    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        animationController = GetComponent<EnemyAnimationController>();
        _agent = GetComponent<NavMeshAgent>();
        _waypointIndex = Random.Range(0,wayPoints.Length);
        _enemyHealth = GetComponent<EnemyHealth>();

        if(wayPoints.Length > 0)
            InvokeRepeating("Patrol", Random.Range(0, patrolTime), patrolTime);
    }
    void Start()
    {
        _agent.avoidancePriority = Random.Range(0, 51);   
    }
    void Update()
    {
        if(_enemyHealth.IsEnemyAlive())
            MoveAndAttack();
        else
        {
            animationController.PlayDeathAnimation();
            _agent.enabled = false;
            if (animationController.DeathAnimationCompleted())
            {
                SceneManagement.OnSkeletonDied?.Invoke(gameObject);
                Destroy(gameObject, 5f);
            }

        }
    }
    private void MoveAndAttack()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if(distance > walkDistance)
        {
            if(_agent.remainingDistance >= _agent.stoppingDistance)
            {
                MoveAgent(2f);

                _nextDestination = wayPoints[_waypointIndex].position;
                _agent.SetDestination(_nextDestination);
            }
            else
            {
                StopAgent();

                _nextDestination = wayPoints[_waypointIndex].position;
                _agent.SetDestination(_nextDestination);
            }
        }
        else
        {
            if (distance > attackDistance + .15f  && !playerTransform.GetComponent<PlayerHealth>().IsPlayerDead())
            {
                if (animationController.IsEnemyNotInAttackAnimation())
                {
                    MoveAgent(3f);
                    animationController.ResetAttackAnimation();
                    _agent.SetDestination(playerTransform.position);
                }
                
            }
            else if(distance <= attackDistance && !playerTransform.GetComponent<PlayerHealth>().IsPlayerDead())
            {
                AttackPlayer();
            }
        }
    }
    private void MoveAgent(float agentSpeed)
    {
        _agent.isStopped = false;
        _agent.speed = agentSpeed;
        animationController.PlayWalkAnimation();
    }
    private void StopAgent()
    {
        _agent.isStopped = true;
        _agent.speed = 0f;
        animationController.StopWalkAnimation();
    }
    private void AttackPlayer()
    {
        StopAgent();

        Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y,
            playerTransform.position.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), rotateSpeed * Time.deltaTime);
        if (_currentAttackTime >= attackRate)
        {
            animationController.PlayAttackAnimation();           
            _currentAttackTime = 0;
        }
        else
        {
            _currentAttackTime += Time.deltaTime;
        }
    }
    private void Patrol()
    {
        _waypointIndex = _waypointIndex == wayPoints.Length - 1 ? 0 : _waypointIndex + 1; 
    }
}
