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
    private Animator _anim;
    private NavMeshAgent _agent;

    private float _currentAttackTime;
    private Vector3 _nextDestination;
    private int _waypointIndex;
    #endregion
    private void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _waypointIndex = Random.Range(0,wayPoints.Length);
        if(wayPoints.Length > 0)
            InvokeRepeating("Patrol", Random.Range(0, patrolTime), patrolTime);
    }
    void Start()
    {
        _agent.avoidancePriority = Random.Range(0, 51);   
    }

    // Update is called once per frame
    void Update()
    {
        MoveAndAttack();
    }
    private void MoveAndAttack()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if(distance > walkDistance)
        {
            if(_agent.remainingDistance >= _agent.stoppingDistance)
            {
                StartMoveAnimation(2f);

                _nextDestination = wayPoints[_waypointIndex].position;
                _agent.SetDestination(_nextDestination);
            }
            else
            {
                StopWalkAnimation();

                _nextDestination = wayPoints[_waypointIndex].position;
                _agent.SetDestination(_nextDestination);
            }
        }
        else
        {
            if (distance > attackDistance + .15f && !IsAttacking())
            {
                StartMoveAnimation(3f);
                _anim.ResetTrigger("Attack");
                _agent.SetDestination(playerTransform.position);
            }
            else if(distance <= attackDistance)
            {
                AttackPlayer();
            }
        }
    }
    private void StartMoveAnimation(float agentSpeed)
    {
        _agent.isStopped = false;
        _agent.speed = agentSpeed;
        _anim.SetBool("Walk", true);
    }
    private void StopWalkAnimation()
    {
        _agent.isStopped = true;
        _agent.speed = 0f;
        _anim.SetBool("Walk", false);
    }
    private void AttackPlayer()
    {
        StopWalkAnimation();

        Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y,
            playerTransform.position.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), rotateSpeed * Time.deltaTime);
        if (_currentAttackTime >= attackRate)
        {
            _anim.SetTrigger("Attack");
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
    private bool IsAttacking()
    {
        return _anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }
}
