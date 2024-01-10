using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : PlayerSkillDamage
{
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private float spellMoveSpeed;
    public void Init(Vector3 throwDirection)
    {
        transform.forward = throwDirection;
        StartCoroutine(DestroyItself());
    }
    internal override void Update()
    {
        base.Update();
        transform.Translate(Vector3.forward * (Time.deltaTime * spellMoveSpeed));
        if (_collided)
        {
           // Debug.Log(hittedGMO.name);
            Instantiate(explosionVFX,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
    private IEnumerator DestroyItself()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
