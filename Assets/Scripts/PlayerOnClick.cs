using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnClick : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 15f;

    [SerializeField] private LayerMask groundlayer;
    #endregion

    #region Privates
    private Animator _anim;
    private CharacterController _controller;
    private CollisionFlags _collisionFlags;

    private Vector3 _moveVector;
    private Vector3 _newMovePoint;
    private Vector3 _targetMovePoint;

    private float _currentSpeed;
    private float _playerToPointDistance;
    private float _gravity;
    private float _height;
    private float _animationExitTime;

    private bool _canMove;
    private bool _finishedMovement = true;
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
        _gravity = 9.8f;
        _animationExitTime = .8f;
    }

    void Update()
    {
        CalculateHeight();
        CheckIfFinishedMovement();
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
                        _targetMovePoint = hit.point;
                    }
                }
            }
        }

        if (_canMove)
        {
            _anim.SetFloat("Speed", 1f);

            _newMovePoint = new Vector3(_targetMovePoint.x, transform.position.y, _targetMovePoint.z);
            //Vector3 moveDir = (_newMovePoint - transform.position).normalized;

            //transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_newMovePoint - transform.position), rotateSpeed*Time.deltaTime);

            _moveVector = transform.forward * _currentSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, _newMovePoint) <= .2f)
            {
                _canMove = false;
            }
        }

        else
        {
            _moveVector = Vector3.zero;
            _anim.SetFloat("Speed", 0f);
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
}
