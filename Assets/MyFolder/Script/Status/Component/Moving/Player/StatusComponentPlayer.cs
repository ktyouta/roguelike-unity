using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ComponentSettingManager;

[System.Serializable]
public class StatusComponentPlayer : StatusComponentMoving
{
    //所持金
    [HideInInspector] public WalletClass charWallet = new WalletClass();
    //満腹度
    [HideInInspector] public FoodPointClass charFood = new FoodPointClass();
    //経験値
    [HideInInspector] public ExperienceClass charExperience = new ExperienceClass();
    //魅力度
    [HideInInspector] public CharmClass charCarm = new CharmClass();
    //装備しているアイテム名
    [HideInInspector] public string weaponName = "なし";
    [HideInInspector] public string shieldName = "なし";

    protected override void Start()
    {
        //2F以降はセットしない
        if (!string.IsNullOrWhiteSpace(charName.name))
        {
            return;
        }

        base.Start();

        //レベルアップ時のステータスアップ用
        charExperience.charHp = charHp;
        charExperience.charAttack = charAttack;
        charExperience.charDefence = charDefence;
    }

    /**
     * ステータス情報を取得
     */
    protected override CommonStatusInfoClass getStatusInfo(int jsonId)
    {
        //IDの一致するプレイヤーのデータを取得
        PlayerStatusInfoClass playerStatusInfo = ComponentSettingManager.roguelikePlayerInfoList
                                            .Where(e => e.id == jsonId).FirstOrDefault()?.status;

        //データの取得に失敗した場合は最初の要素を取得する
        if (playerStatusInfo == null)
        {
            playerStatusInfo = ComponentSettingManager.roguelikePlayerInfoList.FirstOrDefault()?.status;
        }

        return playerStatusInfo;
    }

    /**
     * ステータス情報をセット
     */
    protected override void setParam(CommonStatusInfoClass statusInfo)
    {
        base.setParam(statusInfo);
        //所持金
        charWallet.money = ((PlayerStatusInfoClass)statusInfo).wallet;
        //満腹度
        charFood.foodPoint = ((PlayerStatusInfoClass)statusInfo).food;
        //魅力度
        charCarm.charmPoint = ((PlayerStatusInfoClass)statusInfo).charm;
    }

    /**
     * 各ステータスのMAX値の初期設定
     */
    protected override void initializeStatus()
    {
        base.initializeStatus();
        charFood.initializeMaxFoodPoint();
    }
}
