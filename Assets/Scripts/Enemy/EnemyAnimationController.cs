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
    internal virtual void PlayHitAnimation()
    {
        _anim.SetTrigger("Hit");
    } 
    #endregion
    #region Walk Animation Methods
    internal virtual void PlayWalkAnimation()
    {
        _anim.SetBool("Walk", true);
    }
    internal virtual void StopWalkAnimation()
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
    internal virtual void PlayDeathAnimation()
    {
        _anim.SetBool("Death", true);
    }
    public bool DeathAnimationCompleted() => !_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && 
        _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f;
    #endregion
    #region Animation Events
    public void PlayAttackSFX() => AudioManager.Instance.PlaySFX(2);
    public void PlaySkeletonDeathSFX() => AudioManager.Instance.PlaySFX(5); 
    #endregion
    //public bool AnimatorIsInTransition() => _anim.IsInTransition(0);
    //public bool AnimatorIsInDeathState() => _anim.GetCurrentAnimatorStateInfo(0).IsName("Death");
    //public bool IsAnimatorCurrentStateCompleted() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f;
}
