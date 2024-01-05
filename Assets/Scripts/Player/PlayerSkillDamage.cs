using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillDamage : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float radius = .5f;
    [SerializeField] private float damageAmount = 10f;

    private EnemyHealth _enemyHealth;
    private bool _collided;
    Collider[] _hits;
    void Update()
    {
        _hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        foreach (Collider hit in _hits)
        {
            _enemyHealth = hit.gameObject.GetComponent<EnemyHealth>();
            _collided = true;
        }
        if (_collided)
        {
            _enemyHealth.TakeDamage(damageAmount);
            enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
