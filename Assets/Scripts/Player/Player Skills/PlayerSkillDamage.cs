using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillDamage : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] private float radius = .5f;
    [SerializeField] private float damageAmount = 10f;
    #endregion
    protected GameObject hittedGMO;
    #region Privates
    private const string _enemy = "Enemy";
    private const string _player = "Player";
    protected bool _collided;

    Collider[] _hits;
    private EnemyHealth _enemyHealth;
    private PlayerHealth _playerHealth; 
    #endregion
    internal virtual void Update()
    {
        ControlHittedObjectAndDamage();
    }
    private void ControlHittedObjectAndDamage()
    {
        _hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        foreach (Collider hit in _hits)
        {
            if (enemyLayer == (1 << LayerMask.NameToLayer(_enemy)))
            {
                _enemyHealth = hit.gameObject.GetComponent<EnemyHealth>();
                hittedGMO = hit.gameObject;
                _collided = true;
            }
            else if (enemyLayer == (1 << LayerMask.NameToLayer(_player)))
            {
                _playerHealth = hit.gameObject.GetComponent<PlayerHealth>();
                _collided = true;
            }

            if (_collided)
            {
                if (enemyLayer == (1 << LayerMask.NameToLayer(_enemy)))
                {
                    _enemyHealth?.TakeDamage(damageAmount);
                    enabled = false;
                }
                else if (enemyLayer == (1 << LayerMask.NameToLayer(_player)))
                {
                    _playerHealth?.TakeDamage(damageAmount);
                    enabled = false;
                }

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
