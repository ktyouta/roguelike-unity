using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusComponentPlayer : StatusComponentBase
{
    [Header("������")] public WalletClass charWallet;
    [Header("�����x")] public FoodPointClass charFood;
    [Header("�o���l")] public ExperienceClass charExperience;
    [Header("���͓x")] public CharmClass charCarm;
}
