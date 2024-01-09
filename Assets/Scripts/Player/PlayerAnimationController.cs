using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _anim;
    private float _animationExitTime;

    [SerializeField] private GameObject levelUpVFX;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _animationExitTime = .8f;
    }
    private void Start()
    {
        LevelManager.Instance.OnLevelUp += OnLevelUpAnim;
    }
    public void PlayCastedSkillAnimation(int castedSkillIndex) => _anim.SetInteger("Attack", castedSkillIndex);
    public int GetPlayingSkillAnimationIndex() => _anim.GetInteger("Attack");
    public void PlayBasicAttackAnimation() => _anim.SetTrigger("AttackMove");
    public bool IsPlayingBasicAttackAnimation() => !_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack");
    public void PlayRunAnimation() => _anim.SetFloat("Speed", 1f);
    public void StopRunAnimation() => _anim.SetFloat("Speed", 0f);
    public bool IsPlayingAnimationFinished() => !_anim.IsInTransition(0) && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")
                && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= _animationExitTime;
    public bool IsAnimatorInTransition() => _anim.IsInTransition(0);
    public bool IsPlayingIdleAnimation() => _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    public void OnLevelUpAnim()
    {
        GameObject levelUpEffect = Instantiate(levelUpVFX, transform.position, Quaternion.identity);
        levelUpEffect.transform.SetParent(transform);
        Destroy(levelUpEffect, 3.5f);
    }
    private void OnDisable()
    {
        LevelManager.Instance.OnLevelUp -= OnLevelUpAnim;
    }
}
