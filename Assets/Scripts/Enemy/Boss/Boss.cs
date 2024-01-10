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
    private BossAnimationController animationController;

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
        animationController = GetComponent<BossAnimationController>();
        allWayPoints.AddRange(GameObject.FindGameObjectsWithTag("WayPoints"));
        bossDeath = false;
    }

    void Update()
    {
        if(_finishedAttacking)
            GetControl();

        else
        {
            animationController.ResetBossAttackAnimation();
            if(animationController.IsBossInIdleAnimation())
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
            animationController.PlayDeathAnimation();
            _targetCollider.enabled = false;
            bossDeath = true;
        }
        else
        {
            if(_bossStateChecker.CurrentBossState == BossStates.Chase)
            {

                _agent.isStopped = false;
                animationController.PlayRunAnimation();
                animationController.StopWalkAnimation();
                animationController.PlayWakeUpAnimation();
                _agent.speed = 3f;
                _agent.SetDestination(_playerTransform.position);
            }
            else if(_bossStateChecker.CurrentBossState == BossStates.Patrol)
            {
                _agent.isStopped = false;
                animationController.ResetShootAnimation();
                animationController.StopRunAnimation();
                animationController.PlayRunAnimation();
                animationController.PlayWakeUpAnimation();
                if (_agent.remainingDistance < 4f || !_agent.hasPath)
                {
                    _agent.speed = 2f;
                    PickRandomLocation();
                }
            }
            else if(_bossStateChecker.CurrentBossState == BossStates.Shoot)
            {
                animationController.StopRunAnimation();
                animationController.PlayWakeUpAnimation();
                animationController.StopWalkAnimation();
                LookPlayer();
                if(_currentAttackTime >= attackRate)
                {
                    animationController.PlayShootFireBallAnimation();
                    GameObject _fireball = Instantiate(fireBall, firePosition.position, Quaternion.identity);
                    _fireball.GetComponent<Spell>()?.Init(transform.forward);
                    _currentAttackTime = 0;
                    _agent.isStopped = true;
                    _finishedAttacking = false;
                }
                else
                {
                    _currentAttackTime += Time.deltaTime;
                }
            }
            else if(_bossStateChecker.CurrentBossState == BossStates.Attack)
            {
                animationController.StopRunAnimation();
                animationController.PlayWakeUpAnimation();
                animationController.StopWalkAnimation();
                LookPlayer();

                if(_currentAttackTime >= attackRate)
                {
                    int attackAnimIndex = Random.Range(1, 3);
                    animationController.PlayBossAttackAnimation(attackAnimIndex);

                    _currentAttackTime = 0f;
                    _finishedAttacking = false;
                    _agent.isStopped = true;
                }
                else
                {
                    _currentAttackTime += Time.deltaTime;
                    animationController.ResetBossAttackAnimation();
                }
            }
            else
            {
                animationController.StopWakeUpAnimation();
                animationController.StopWalkAnimation();
                animationController.StopRunAnimation();
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
