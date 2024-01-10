using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float attackRate;

    [SerializeField] private SphereCollider _targetCollider;
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform firePosition;

    private Transform _playerTransform;
    private BossState _bossStateChecker;
    private NavMeshAgent _agent;
    private Animator _anim;

    private bool _finishedAttacking = true;
    private float _currentAttackTime;
    private List<GameObject> allWayPoints = new List<GameObject>();

    public static bool bossDeath = false;
    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _bossStateChecker = GetComponent<BossState>();
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        allWayPoints.AddRange(GameObject.FindGameObjectsWithTag("WayPoints"));
        bossDeath = false;
    }

    void Update()
    {
        if(_finishedAttacking)
            GetControl();

        else
        {
            _anim.SetInteger("Attack", 0);
            if(!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _finishedAttacking = true;
            }
        }
    }
    private void GetControl()
    {
        if(_bossStateChecker.CurrentBossState == BossStates.Death)
        {
            _agent.isStopped = true;
            _anim.SetBool("Death", true);
            _targetCollider.enabled = false;
            bossDeath = true;
        }
        else
        {
            if(_bossStateChecker.CurrentBossState == BossStates.Chase)
            {

                _agent.isStopped = false;
                _anim.SetBool("Run", true);
                _anim.SetBool("Walk", false);
                _anim.SetBool("WakeUp", true);
                _agent.speed = 3f;
                _agent.SetDestination(_playerTransform.position);
            }
            else if(_bossStateChecker.CurrentBossState == BossStates.Patrol)
            {
                _agent.isStopped = false;
                _anim.ResetTrigger("Shoot");
                _anim.SetBool("Run", false);
                _anim.SetBool("Walk", true);
                _anim.SetBool("WakeUp", true);
                if (_agent.remainingDistance < 4f || !_agent.hasPath)
                {
                    _agent.speed = 2f;
                    PickRandomLocation();
                }
            }
            else if(_bossStateChecker.CurrentBossState == BossStates.Shoot)
            {
                _anim.SetBool("Run", false);
                _anim.SetBool("WakeUp", true);
                _anim.SetBool("Walk", false);
                LookPlayer();
                if(_currentAttackTime >= attackRate)
                {
                    _anim.SetTrigger("Shoot");
                    //Instantiate(fireBall, firePosition.position, Quaternion.identity);
                    _currentAttackTime = 0;
                    _finishedAttacking = false;
                }
                else
                {
                    _currentAttackTime += Time.deltaTime;
                }
            }
            else if(_bossStateChecker.CurrentBossState == BossStates.Attack)
            {
                _anim.SetBool("Run", false);
                _anim.SetBool("WakeUp", true);
                _anim.SetBool("Walk", false);
                _agent.isStopped = true;
                LookPlayer();

                if(_currentAttackTime >= attackRate)
                {
                    int attackAnimIndex = Random.Range(1, 3);
                    _anim.SetInteger("Attack", attackAnimIndex);

                    _currentAttackTime = 0f;
                    _finishedAttacking = false;
                }
                else
                {
                    _currentAttackTime += Time.deltaTime;
                    _anim.SetInteger("Attack", 0);
                }
            }
            else
            {
                _anim.SetBool("WakeUp", false);
                _anim.SetBool("Walk", false);
                _anim.SetBool("Run", false);
                _agent.isStopped = true;
            }
        }
    }
    private void PickRandomLocation()
    {
        GameObject pos = GetRandomPoint();
        _agent.SetDestination(pos.transform.position);
    }
    private GameObject GetRandomPoint()
    {
        int index = Random.Range(0, allWayPoints.Count);
        return allWayPoints[index];
    }
    void LookPlayer()
    {
        Vector3 targetPos = new Vector3(_playerTransform.position.x, transform.position.y, _playerTransform.position.z);
        Vector3 moveDir = (targetPos - transform.position).normalized;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }
}
