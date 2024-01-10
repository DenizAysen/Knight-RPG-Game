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

    [SerializeField] private List<float> manaCostList;
    [SerializeField] private List<float> cooldownTimersList;

    [Header("Required Level")]
    [SerializeField] private List<int> requiredLevelList;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    #endregion
    #region Privates
    private bool _faded;
    private bool _canAttack;
    private int _castedSkillIndex;
    private int[] _fadeImages;
    private FrameInput _frameInput;

    private PlayerAnimationController _animationController;
    private PlayerOnClick _playerOnClick;
    private PlayerManaController manaController;
    private PlayerInput playerInput;

    private LevelManager _levelManager;
    #endregion
    private void Awake()
    {
        _playerOnClick = GetComponent<PlayerOnClick>();
        _canAttack = true;
        _fadeImages = new int[] { 0, 0, 0, 0, 0, 0 };
        playerInput = GetComponent<PlayerInput>();
        manaController = GetComponent<PlayerManaController>();
        _animationController = GetComponent<PlayerAnimationController>();
        _levelManager = FindObjectOfType<LevelManager>();
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

    void Update()
    {
        if(!_animationController.IsAnimatorInTransition() && _animationController.IsPlayingIdleAnimation())
        {
            _canAttack = true;
            //if (virtualCamera.m_Follow != transform)
            //    virtualCamera.m_Follow = transform;
        }
        else
        {
            _canAttack = false;
        }       

        if (_animationController.IsAnimatorInTransition() && _animationController.IsPlayingIdleAnimation() && _castedSkillIndex > -1)
        {
            TurnThePlayer();
        }
        CheckLevel();
        CheckMana();
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
            if(_levelManager.GetLevel>= requiredLevelList[i])
            {
                if (!manaController.CanCastSkill(manaCostList[i]))
                    outOfManaIcons[i].gameObject.SetActive(true);

                else
                    outOfManaIcons[i].gameObject.SetActive(false);
            }         
        }
    }
    private void CheckLevel()
    {
        for (int i = 0; i < outOfManaIcons.Length; i++)
        {
            if (_levelManager.GetLevel < requiredLevelList[i])
                outOfManaIcons[i].gameObject.SetActive(true);

            else
                outOfManaIcons[i].gameObject.SetActive(false);
        }
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
        if (_animationController.GetPlayingSkillAnimationIndex() == 0)
        {
            if (!_animationController.IsAnimatorInTransition() && _animationController.IsPlayingIdleAnimation())
            {
                _playerOnClick.FinishedMovement = true;
            }
            else
            {
                _playerOnClick.FinishedMovement = false;
            }
        }
        if (_castedSkillIndex < 0)
        {
            if (_animationController.GetPlayingSkillAnimationIndex() != 0)
                _animationController.PlayCastedSkillAnimation(0);
            return;
        }
        else
        {
            if (manaController.CanCastSkill(manaCostList[_castedSkillIndex]) && _levelManager.GetLevel >= requiredLevelList[_castedSkillIndex])
            {
                _playerOnClick.TargetPosition = transform.position;
                if (_playerOnClick.FinishedMovement && _fadeImages[_castedSkillIndex] != 1 && _canAttack)
                {
                    _fadeImages[_castedSkillIndex] = 1;
                    manaController.SpendMana(manaCostList[_castedSkillIndex]);
                    _animationController.PlayCastedSkillAnimation(_castedSkillIndex + 1);
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
        //virtualCamera.m_Follow = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit))
        {
            targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position),
            _playerOnClick.RotateSpeed * 100 * Time.deltaTime);
    }
}
