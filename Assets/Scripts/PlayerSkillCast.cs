using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerSkillCast : MonoBehaviour
{
    #region SerializedFields
    [Header("Cooldown Icons")]
    [SerializeField] private Image[] cooldownIcons;

    [Header("Out of Mana Icons")]
    [SerializeField] private Image[] outOfManaIcons;

    [Header("Mana Settings")]
    [SerializeField] private float totalMana = 100f;
    [SerializeField] private float manaRegenSpeed = 2f;
    [SerializeField] private Image manaBar;

    [SerializeField] private List<float> manaCostList;
    [SerializeField] private List<float> cooldownTimersList;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    #endregion
    #region Privates
    private bool _faded;
    private bool _canAttack;
    private int _castedSkillIndex;
    private int[] _fadeImages;
    private FrameInput _frameInput;

    private Animator _anim;

    private PlayerOnClick _playerOnClick;
    private PlayerInput playerInput;
    #endregion
    private void Awake()
    {
        _playerOnClick = GetComponent<PlayerOnClick>();
        _canAttack = true;
        _fadeImages = new int[] { 0, 0, 0, 0, 0, 0 };
        _anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }
    void Start()
    {
        for (int i = 0; i < cooldownIcons.Length; i++)
        {
            if(_fadeImages[i] == 1)
            {
                if (FadeAndWait(cooldownIcons[i], cooldownTimersList[i]))
                {
                    _fadeImages[i] = 0;
                }
            }            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _canAttack = true;
            if (virtualCamera.m_Follow != transform)
                virtualCamera.m_Follow = transform;
        }
        else
        {
            _canAttack = false;
        }
        if (_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            TurnThePlayer();
        }

        CheckMana();
        RegenerateMana();
        CheckToFade();
        CheckInput();
        CastSkill();
    }
    private void CheckInput()
    {
        _frameInput = playerInput.GetInput();
        _castedSkillIndex = _frameInput.SkillIndex;
    }
    private void CheckToFade()
    {
        for (int i = 0; i < cooldownIcons.Length; i++)
        {
            if (_fadeImages[i] == 1)
            {
                if (FadeAndWait(cooldownIcons[i], 1f))
                {
                    _fadeImages[i] = 0;
                }
            }
        }       
    }
    private void CheckMana()
    {
        for (int i = 0; i < manaCostList.Count; i++)
        {
            if(totalMana < manaCostList[i])
            {
                outOfManaIcons[i].gameObject.SetActive(true);
            }
            else
                outOfManaIcons[i].gameObject.SetActive(false);
        }
    }
    private void RegenerateMana()
    {
        if (!(totalMana < 100f))
            return;

        totalMana += Time.deltaTime * manaRegenSpeed;
        manaBar.fillAmount = (totalMana / 100f);
    }
    private bool FadeAndWait(Image fadeImage, float fadeTime)
    {
        _faded = false;

        if (fadeImage == null)
            return _faded;

        if (!fadeImage.gameObject.activeInHierarchy)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.fillAmount = 1f;
        }            

        fadeImage.fillAmount -= fadeTime * Time.deltaTime;

        if(fadeImage.fillAmount <= 0f)
        {
            fadeImage.gameObject.SetActive(false);
            _faded = true;
        }
        return _faded;
    }
    private void CastSkill()
    {
        if (_anim.GetInteger("Attack") == 0)
        {
            if (!_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _playerOnClick.FinishedMovement = true;
            }
            else
            {
                _playerOnClick.FinishedMovement = false;
            }
        }
        if(_castedSkillIndex < 0)
        {
            _anim.SetInteger("Attack", 0);
        }
        else
        {
            if (totalMana >= manaCostList[_castedSkillIndex])
            {
                _playerOnClick.TargetPosition = transform.position;
                if (_playerOnClick.FinishedMovement && _fadeImages[_castedSkillIndex] != 1 && _canAttack)
                {
                    _fadeImages[_castedSkillIndex] = 1;
                    totalMana -= manaCostList[_castedSkillIndex];
                    _anim.SetInteger("Attack", _castedSkillIndex+1);
                }
            }
        }
        //if (_castedSkillIndex == 0 && totalMana >= manaCostList[_castedSkillIndex])
        //{
        //    _playerOnClick.TargetPosition = transform.position;
        //    if (_playerOnClick.FinishedMovement && _fadeImages[_castedSkillIndex] != 1 && _canAttack)
        //    {
        //        _fadeImages[_castedSkillIndex] = 1;
        //        totalMana -= manaCostList[_castedSkillIndex];
        //        _anim.SetInteger("Attack", 1);
        //    }
        //}
        //else if (_castedSkillIndex == 1 && totalMana >= manaCostList[_castedSkillIndex])
        //{
        //    _playerOnClick.TargetPosition = transform.position;
        //    if (_playerOnClick.FinishedMovement && _fadeImages[1] != 1 && _canAttack)
        //    {
        //        _fadeImages[1] = 1;
        //        totalMana -= manaCostList[_castedSkillIndex];
        //        _anim.SetInteger("Attack", 2);
        //    }
        //}
        //else if (_castedSkillIndex == 2 && totalMana >= manaCostList[_castedSkillIndex])
        //{
        //    _playerOnClick.TargetPosition = transform.position;
        //    if (_playerOnClick.FinishedMovement && _fadeImages[2] != 1 && _canAttack)
        //    {
        //        _fadeImages[2] = 1;
        //        totalMana -= manaCostList[_castedSkillIndex];
        //        _anim.SetInteger("Attack", 3);
        //    }
        //}
        //else if (_castedSkillIndex == 3 && totalMana >= manaCostList[_castedSkillIndex])
        //{
        //    _playerOnClick.TargetPosition = transform.position;
        //    if (_playerOnClick.FinishedMovement && _fadeImages[3] != 1 && _canAttack)
        //    {
        //        _fadeImages[3] = 1;
        //        totalMana -= manaCostList[_castedSkillIndex];
        //        _anim.SetInteger("Attack", 4);
        //    }
        //}
        //else if (_castedSkillIndex == 4)
        //{
        //    _playerOnClick.TargetPosition = transform.position;
        //    if (_playerOnClick.FinishedMovement && _fadeImages[4] != 1 && _canAttack)
        //    {
        //        _fadeImages[4] = 1;
        //        totalMana -= manaCostList[_castedSkillIndex];
        //        _anim.SetInteger("Attack", 5);
        //    }
        //}
        //else if (_castedSkillIndex == 5)
        //{
        //    _playerOnClick.TargetPosition = transform.position;
        //    if (_playerOnClick.FinishedMovement && _fadeImages[5] != 1 && _canAttack)
        //    {
        //        _fadeImages[5] = 1;
        //        totalMana -= manaCostList[_castedSkillIndex];
        //        _anim.SetInteger("Attack", 6);
        //    }
        //}
        //else
        //{
        //    _anim.SetInteger("Attack", 0);
        //}
    }
    private void TurnThePlayer()
    {
        Vector3 targetPos = Vector3.zero;
        virtualCamera.m_Follow = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit))
        {
            targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position),
            _playerOnClick.RotateSpeed * Time.deltaTime);
    }
}
