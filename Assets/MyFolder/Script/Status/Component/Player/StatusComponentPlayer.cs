using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusComponentPlayer : StatusComponentBase
{
    [Header("所持金")] public WalletClass charWallet;
    [Header("満腹度")] public FoodPointClass charFood;
    [Header("経験値")] public ExperienceClass charExperience;
    [Header("魅力度")] public CharmClass charCarm;
}
