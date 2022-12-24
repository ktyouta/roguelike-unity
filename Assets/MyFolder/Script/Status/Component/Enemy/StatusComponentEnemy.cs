using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ComponentSettingManager;

public class StatusComponentEnemy : StatusComponentBase
{
    [Header("所持金")] public WalletClass charWallet = new WalletClass();
    [Header("経験値増加量")] public int experience;

    protected override void Start()
    {
        //josnから該当するデータを取得してパラメータをセット
        int? enemyId = GetComponent<Enemy>()?.enemyId;
        if (enemyId == null)
        {
            Destroy(gameObject);
        }

        //IDの一致する敵のデータを取得
        EnemyStatusInfoClass enemyStatusInfo = ComponentSettingManager.roguelikeEnemyInfoList
                                            .Where(e => e.id == enemyId).FirstOrDefault()?.status;

        // データの取得に失敗した場合はオブジェクトを削除する
        if (enemyStatusInfo == null)
        {
            Destroy(gameObject);
        }

        //名前
        charName.name = enemyStatusInfo.name;
        //HP
        charHp.hp = enemyStatusInfo.hp;
        //攻撃力
        charAttack.totalAttack = enemyStatusInfo.attack;
        //防御力
        charDefence.totalDefence = enemyStatusInfo.defence;
        //所持金
        charWallet.money = enemyStatusInfo.wallet;
        //経験値
        experience = enemyStatusInfo.experience;
    }
}
