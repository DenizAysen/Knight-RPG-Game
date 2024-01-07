using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    Animator _anim;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    #region Hit Animation
    public void PlayHitAnimation()
    {
        _anim.SetTrigger("Hit");
    } 
    #endregion
    #region Walk Animation Methods
    public void PlayWalkAnimation()
    {
        _anim.SetBool("Walk", true);
    }
    public void StopWalkAnimation()
    {
        _anim.SetBool("Walk", false);
    }
    #endregion
    #region Attack Animation Methods
    public void PlayAttackAnimation()
    {
        _anim.SetTrigger("Attack");
    }
    public void ResetAttackAnimation()
    {
        _anim.ResetTrigger("Attack");
    }
    public bool IsEnemyNotInAttackAnimation()
    {
        return !_anim.IsInTransition(0) && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }
    #endregion
    #region Death Animation
    public void PlayDeathAnimation()
    {
        _anim.SetBool("Death", true);
    } 
    #endregion
}
