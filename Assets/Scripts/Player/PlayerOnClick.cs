using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnClick : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private float attackRange = 2f;

    [SerializeField] private LayerMask groundlayer;
    #endregion

    #region Privates
    private Animator _anim;
    private CharacterController _controller;
    private CollisionFlags _collisionFlags;

    private Vector3 _moveVector;
    private Vector3 _newMovePoint;
    private Vector3 _targetMovePoint;
    private Vector3 _targetAttackPoint;
    private Vector3 _newAttackPoint;

    private float _currentSpeed;
    private float _playerToPointDistance;
    private float _gravity;
    private float _height;
    private float _animationExitTime;

    private bool _canMove;
    private bool _canAttackMove;
    private bool _finishedMovement = true;

    private GameObject _enemy;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _currentSpeed = maxMoveSpeed;
    }
    void Start()
    {
        _collisionFlags = CollisionFlags.None;
        _moveVector = Vector3.zero;
        _targetMovePoint = Vector3.zero;
        _targetAttackPoint = Vector3.zero;
        _gravity = 9.8f;
        _animationExitTime = .8f;
    }

    void Update()
    {
        CalculateHeight();
        CheckIfFinishedMovement();
        AttackMove();
    }
    #endregion

    #region Player Movement
    private bool IsGrounded()
    {
        return _collisionFlags == CollisionFlags.CollidedBelow ? true : false;
    }
    private void CalculateHeight()
    {
        if (IsGrounded())
            _height = 0f;
        else
            _height -= _gravity * Time.deltaTime;
    }
    void MovePlayer()
    {
        if (Input.GetMouseButtonDown(1))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _playerToPointDistance = Vector3.Distance(transform.position, hit.point);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {

                    if (_playerToPointDistance > 1f)
                    {
                        _canMove = true;
                        _canAttackMove = false;
                        _targetMovePoint = hit.point;
                    }
                }
                else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
                {
                    _enemy = hit.collider.gameObject.transform.parent.gameObject;
                    _canMove = true;
                    _canAttackMove = true;
                }
  
            }
        }

        if (_canMove)
        {
            _anim.SetFloat("Speed", 1f);

            if (!_canAttackMove)
            {
                _newMovePoint = new Vector3(_targetMovePoint.x, transform.position.y, _targetMovePoint.z);
                //Vector3 moveDir = (_newMovePoint - transform.position).normalized;

                //transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_newMovePoint - transform.position), rotateSpeed * Time.deltaTime);
            }

            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_newAttackPoint - transform.position), rotateSpeed * Time.deltaTime);
            }

            _moveVector = transform.forward * _currentSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, _newMovePoint) <= .2f && !_canAttackMove)
            {
                _canMove = false;
                _canAttackMove = false;
            }
            else if (_canAttackMove)
            {
                if(Vector3.Distance(transform.position, _newAttackPoint) <= attackRange)
                {
                    _moveVector = Vector3.zero;
                    _anim.SetFloat("Speed", 0f);
                    _targetAttackPoint = Vector3.zero;
                    _anim.SetTrigger("AttackMove");
                    _canAttackMove = false;
                    _canMove = false;
                }
            }
        }

        else
        {
            _moveVector = Vector3.zero;
            _anim.SetFloat("Speed", 0f);
        }
    }
    private void AttackMove()
    {
        if (_canAttackMove)
        {
            _targetAttackPoint = _enemy.gameObject.transform.position;

            _newAttackPoint = new Vector3(_targetAttackPoint.x, transform.position.y, _targetAttackPoint.z);           
        }
        if (!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack"))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_newAttackPoint - transform.position), rotateSpeed * Time.deltaTime);
        }
    }
    private void CheckIfFinishedMovement()
    {
        if (!_finishedMovement)
        {
            if (!_anim.IsInTransition(0) && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")
                && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= _animationExitTime)
            {
                _finishedMovement = true;
            }
        }
        else
        {
            MovePlayer();
            _moveVector.y = _height * Time.deltaTime;
            _collisionFlags = _controller.Move(_moveVector);
        }
    }
    #endregion

    #region Properties
    public bool FinishedMovement
    {
        get
        {
            return _finishedMovement;
        }
        set
        {
            _finishedMovement = value;
        }
    }
    public bool CanMove
    {
        get
        {
            return _canMove;
        }
        set
        {
            _canMove = value;
        }
    }
    public Vector3 TargetPosition
    {
        get
        {
            return _targetMovePoint;
        }
        set
        {
            _targetMovePoint = value;
        }
    }
    public float RotateSpeed
    {
        get
        {
            return rotateSpeed;
        }
    } 
    #endregion
}
