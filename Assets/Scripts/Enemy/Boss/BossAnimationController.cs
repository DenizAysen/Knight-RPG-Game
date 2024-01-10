using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : EnemyAnimationController
{
    #region Privates
    Animator _animator;
    private const string _hit = "Hit";
    private const string _death = "Death";
    private const string _walk = "Walk";
    private const string _attack = "Attack";
    private const string _shoot = "Shoot";
    private const string _wakeUp = "WakeUp";
    private const string _run = "Run"; 
    #endregion
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    internal override void PlayHitAnimation()
    {
        _animator.SetTrigger(_hit);
    }
    internal override void PlayDeathAnimation()
    {
        _animator.SetBool(_death, true);
    }
    #region Walk Animation
    internal override void PlayWalkAnimation()
    {
        _animator.SetBool(_walk, true);
    }
    internal override void StopWalkAnimation()
    {
        _animator.SetBool(_walk, false);
    }
    #endregion
    #region Attack Animation
    public void PlayBossAttackAnimation(int basicAttackAnimIndex)
    {
        _animator.SetInteger(_attack, basicAttackAnimIndex);
    }
    public void ResetBossAttackAnimation()
    {
        _animator.SetInteger(_attack, 0);
    }
    #endregion
    #region Fire Ball Animation
    public void PlayShootFireBallAnimation()
    {
        _animator.SetTrigger(_shoot);
    }
    public void ResetShootAnimation()
    {
        _animator.ResetTrigger(_shoot);
    }
    #endregion
    #region Wake Up Animation
    public void PlayWakeUpAnimation()
    {
        _animator.SetBool(_wakeUp, true);
    }
    public void StopWakeUpAnimation()
    {
        _animator.SetBool(_wakeUp, false);
    }
    #endregion
    #region Run Animation
    public void PlayRunAnimation()
    {
        _animator.SetBool(_run, true);
    }
    public void StopRunAnimation()
    {
        _animator.SetBool(_run, false);
    } 
    #endregion
    public bool IsBossInIdleAnimation() => !_animator.IsInTransition(0) && _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
}
