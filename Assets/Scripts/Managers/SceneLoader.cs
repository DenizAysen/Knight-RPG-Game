using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    #region Unity Methods
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        LevelManager.OnBossDeath += OnBossDeath;
        LevelManager.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        StartCoroutine(RestartScene());
    }

    private void OnBossDeath()
    {
        StartCoroutine(RestartScene());
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void UnSubscribeEvents()
    {
        LevelManager.OnBossDeath -= OnBossDeath;
        LevelManager.OnPlayerDeath -= OnPlayerDeath;
    }  
    #endregion
    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(0);
    }
    
}
