using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    BossState _bossState;
    private PlayerHealth _playerHealth;
    [SerializeField] private GameObject bossHealthBar;
    private void Awake()
    {
        _bossState = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossState>();
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }
    private void Start()
    {
        AudioManager.Instance.PlayGameMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (_bossState.CurrentBossState == BossStates.Sleep || _bossState.CurrentBossState == BossStates.Death)
        {
            bossHealthBar?.SetActive(false);
        }
        else
            bossHealthBar?.SetActive(true);

        if(Boss.bossDeath == true)
        {
            Invoke("RestartScene", 5f);           
        }
        else if (_playerHealth.IsPlayerDead())
        {
            Invoke("RestartScene", 5f);
        }
    }
    void RestartScene() => SceneManager.LoadScene(0);
}
