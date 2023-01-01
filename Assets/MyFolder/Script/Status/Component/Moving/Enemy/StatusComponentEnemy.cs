using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ComponentSettingManager;

public class StatusComponentEnemy : StatusComponentMoving
{
    [Header("所持金")] public WalletClass charWallet = new WalletClass();
    [Header("経験値増加量")] public int experience;

    /**
     * ステータス情報を取得
     */
    protected override CommonStatusInfoClass getStatusInfo(int jsonId)
    {
        //IDの一致する敵のデータを取得
        EnemyStatusInfoClass enemyStatusInfo = ComponentSettingManager.roguelikeEnemyInfoList
                                            .Where(e => e.id == jsonId).FirstOrDefault()?.status;

        //データの取得に失敗した場合は最初の要素を取得する
        if (enemyStatusInfo == null)
        {
            enemyStatusInfo = ComponentSettingManager.roguelikeEnemyInfoList.FirstOrDefault()?.status;
        }

        return enemyStatusInfo;
    }

    /**
     * ステータス情報をセット
     */
    protected override void setParam(CommonStatusInfoClass statusInfo)
    {
        base.setParam(statusInfo);
        //所持金
        charWallet.money = ((EnemyStatusInfoClass)statusInfo).wallet;
        //経験値
        experience = ((EnemyStatusInfoClass)statusInfo).experience;
    }
}
