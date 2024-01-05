using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillEffect : MonoBehaviour
{
    [SerializeField] private List<SkillEffect> skillEffects = new List<SkillEffect>();
    private void HammerSkillEffect()
    {
        Instantiate(skillEffects[0].SkillParticle, skillEffects[0].SkillSpawnPos.position, Quaternion.identity);
    }
    private void SpellCastEffect()
    {
        Instantiate(skillEffects[1].SkillParticle, skillEffects[1].SkillSpawnPos.position, Quaternion.identity);
    }
    private void KickSkillEffect()
    {
        Instantiate(skillEffects[2].SkillParticle, skillEffects[2].SkillSpawnPos.position, Quaternion.identity);
    }
    private void ShieldSpellEffect()
    {
        Instantiate(skillEffects[3].SkillParticle, transform.position, Quaternion.identity);
    }
    private void HealSpellEffect()
    {
        Instantiate(skillEffects[4].SkillParticle, transform.position, Quaternion.identity);
    }
    private void SlashComboEffect()
    {
        Instantiate(skillEffects[5].SkillParticle, skillEffects[5].SkillSpawnPos.position, Quaternion.identity);
    }
}
[Serializable]
public class SkillEffect
{
    public GameObject SkillParticle;
    public Transform SkillSpawnPos;
    public SkillType SkillType;
}
public enum SkillType
{
    HammerSkill,
    KickSkill,
    SpellCastSkill,
    ShieldSkill,
    HealSkill,
    ComboSkill
}
