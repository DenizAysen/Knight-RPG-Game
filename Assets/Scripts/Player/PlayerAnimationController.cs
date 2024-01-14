using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Privates
    private Animator _anim;
    private float _animationExitTime;
    #endregion
    #region SerializedFields
    [SerializeField] private GameObject levelUpVFX;
    [SerializeField] private GameObject dustParticle;
    #endregion
    #region Actions
    public static Action<GameObject> OnHitDustInit; 
    #endregion
    #region Unity Methods
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _animationExitTime = .8f;
    }
    private void Start()
    {
        SubscribeEvents();
    }
    private void SubscribeEvents()
    {
        LevelManager.Instance.OnLevelUp += OnLevelUpAnim;
        OnHitDustInit += CreateHitDust;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();

    }
    private void UnSubscribeEvents()
    {
        LevelManager.Instance.OnLevelUp -= OnLevelUpAnim;
        OnHitDustInit -= CreateHitDust;
    }
    #endregion
    #region Skill Animation Methods
    public void PlayCastedSkillAnimation(int castedSkillIndex) => _anim.SetInteger("Attack", castedSkillIndex);
    public int GetPlayingSkillAnimationIndex() => _anim.GetInteger("Attack");
    #endregion
    #region Basic Attack Animation Methods
    public void PlayBasicAttackAnimation() => _anim.SetTrigger("AttackMove");
    public bool IsPlayingBasicAttackAnimation() => !_anim.IsInTransition(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack");
    private void CreateHitDust(GameObject hit)
    {
        Vector3 initPos = new Vector3(hit.transform.position.x, hit.transform.position.y + 1.25f, hit.transform.position.z);
        Instantiate(dustParticle, initPos, Quaternion.identity);
    }
    #endregion
    #region Run Animation Methods
    public void PlayRunAnimation() => _anim.SetFloat("Speed", 1f);
    public void StopRunAnimation() => _anim.SetFloat("Speed", 0f);
    #endregion
    #region Animation Control Methods
    public bool IsPlayingAnimationFinished() => !_anim.IsInTransition(0) && !_anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= _animationExitTime;
    public bool IsAnimatorInTransition() => _anim.IsInTransition(0);
    public bool IsPlayingIdleAnimation() => _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    #endregion
    #region Level Up Animation
    public void OnLevelUpAnim()
    {
        GameObject levelUpEffect = Instantiate(levelUpVFX, transform.position, Quaternion.identity);
        levelUpEffect.transform.SetParent(transform);
        Destroy(levelUpEffect, 3.5f);
    } 
    #endregion
}
