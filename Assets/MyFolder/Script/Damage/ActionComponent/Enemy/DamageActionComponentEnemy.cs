using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentEnemy : DamageActionComponentBase
{
    protected bool isDefeat = false;
    private StatusComponentEnemy statusComponentEnemyObj;
    [SerializeField, HideInInspector] public StatusComponentPlayer statusComponentObj;

    protected override void Start()
    {
        base.Start();
        statusComponentEnemyObj = (StatusComponentEnemy)this.statusObj;
        statusComponentObj = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();
    }

    /**
     * ダメージを受けた際の共通アクション
     */
    public override void reciveDamageAction(int hp)
    {
        if (isDefeat)
        {
            return;
        }
        if (hp <= 0)
        {
            isDefeat = true;
            //プレイヤーの所持金を追加
            statusComponentObj?.charWallet.addMoney(statusComponentEnemyObj.charWallet.money);
            //経験値を追加
            statusComponentObj?.charExperience.addExperience(statusComponentEnemyObj.experience);
            //GManager.instance.wrightDeadLog(statusComponentEnemyObj.charName.name);
            LogMessageManager.wrightLog(MessageManager.createMessage("6", statusComponentEnemyObj.charName.name));
            GManager.instance.removeEnemyToList(GetComponent<Enemy>());
            //敵ごとの固有アクション
            specialAction();
            Destroy(gameObject, 0.5f);
        }
    }

    /**
     * 敵ごとの固有アクション
     */
    protected virtual void specialAction() { }
}
