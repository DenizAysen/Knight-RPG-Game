using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    #endregion

    #region Serialized Fields
    [SerializeField] private Image expBar;
    [SerializeField] private Text levelText;
    #endregion
    public Action<float> OnExpGained;
    public Action<int> OnLevelGained;
    private void OnEnable()
    {
        OnExpGained += UpdateExpBar;
        OnLevelGained += UpdateLevelText;
    }
    private void OnDisable()
    {
        OnExpGained -= UpdateExpBar;
        OnLevelGained -= UpdateLevelText;
    }
    void Start()
    {
        
    }

    private void UpdateExpBar(float XP)
    {
        expBar.fillAmount = XP;
    }
    private void UpdateLevelText(int level)
    {
        levelText.text = level.ToString();
    }
}
