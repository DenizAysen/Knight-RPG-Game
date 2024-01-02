using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillCast : MonoBehaviour
{
    #region SerializedFields
    [Header("Cooldown Icons")]
    [SerializeField] private Image[] cooldownIcons;
    [Header("Out of Mana Icons")]
    [SerializeField] private Image[] outOfManaIcons;

    [SerializeField] private List<float> cooldownTimersList;

    #endregion
    #region Privates
    private bool _faded;
    private bool _canAttack;

    private int[] _fadeImages;

    private Animator _anim;

    private PlayerOnClick _playerOnClick; 
    #endregion
    private void Awake()
    {
        _playerOnClick = GetComponent<PlayerOnClick>();
        _canAttack = true;
        _fadeImages = new int[] { 0, 0, 0, 0, 0, 0 };
        _anim = GetComponent<Animator>();
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
        }
        else
        {
            _canAttack = false;
        }

        CheckToFade();
    }
    private void CheckToFade()
    {
        if(_fadeImages[0] == 1)
        {
            if (FadeAndWait(cooldownIcons[0], 1f))
            {

            }
        }
    }
    private bool FadeAndWait(Image fadeImage, float fadeTime)
    {
        _faded = false;

        if (fadeImage == null)
            return _faded;

        fadeImage.fillAmount -= fadeTime * Time.deltaTime;

        if(fadeImage.fillAmount <= 0f)
        {
            fadeImage.gameObject.SetActive(false);
            _faded = true;
        }
        return _faded;
    }
}
