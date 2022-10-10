using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusComponentPlayer : StatusComponentBase
{
    [Header("������")] public WalletClass charWallet;
    [Header("�����x")] public FoodPointClass charFood;
    [Header("�o���l")] public ExperienceClass charExperience;
    [Header("���͓x")] public CharmClass charCarm;

    protected override void Start()
    {
        base.Start();
        //�X�e�[�^�X�A�b�v�p
        charExperience.charHp = charHp;
        charExperience.charAttack = charAttack;
        charExperience.charDefence = charDefence;
    }
}
