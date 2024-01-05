using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManaController : MonoBehaviour
{
    [SerializeField] private float totalMana = 100f;
    [SerializeField] private float manaRegenSpeed = 2f;
    [SerializeField] private Image manaBar;
    private void Update()
    {
        RegenerateManaBar();
    }
    public bool CanCastSkill(float manaCost)
    {
        return totalMana >= manaCost;
    }
    public void SpendMana(float manaCost)
    {
        totalMana -= manaCost;
    }
    private void RegenerateManaBar()
    {
        if (!(totalMana < 100f))
            return;

        totalMana += Time.deltaTime * manaRegenSpeed;
        manaBar.fillAmount = (totalMana / 100f);
    }
}
