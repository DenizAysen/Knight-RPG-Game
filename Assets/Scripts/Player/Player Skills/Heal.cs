using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private float healAmount = 20f;
    private PlayerHealth _playerHealth;
    void Start()
    {
        _playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        _playerHealth.HealPlayer(20f);
    }
}
