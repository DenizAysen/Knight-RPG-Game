using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float effectDestroyTime = 5f;
    PlayerHealth _playerHealth;
    private void Awake()
    {
        StartCoroutine(ShieldEffect());
    }
    private IEnumerator ShieldEffect()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        _playerHealth.IsShielded = true;
        yield return new WaitForSeconds(effectDestroyTime);
        Destroy(gameObject);
    }
    private void OnDisable()
    {
        _playerHealth.IsShielded = false;
    }
}
