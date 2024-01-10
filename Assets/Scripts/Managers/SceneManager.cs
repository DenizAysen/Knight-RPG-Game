using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    BossState _bossState;
    [SerializeField] private GameObject bossHealthBar;
    private void Awake()
    {
        _bossState = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_bossState.CurrentBossState == BossStates.Sleep || _bossState.CurrentBossState == BossStates.Death)
        {
            bossHealthBar.SetActive(false);
        }
        else
            bossHealthBar.SetActive(true);
    }
}
