using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusComponentPlayer : StatusComponentBase
{
    [Header("所持金")] public WalletClass charWallet;
    [Header("満腹度")] public FoodPointClass charFood;
    [Header("経験値")] public ExperienceClass charExperience;
    [Header("魅力度")] public CharmClass charCarm;

    protected override void Start()
    {
        base.Start();
        //ステータスアップ用
        charExperience.charHp = charHp;
        charExperience.charAttack = charAttack;
        charExperience.charDefence = charDefence;
    }
}
