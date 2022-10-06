using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutAccessComponentBase : MonoBehaviour
{
    private StatusComponentBase statusObj;
    private DamageCalculateBase damageCalculateObj;
    private DamageActionComponentBase damageActionObj;

    // Start is called before the first frame update
    public void Start()
    {
        this.statusObj = GetComponent<StatusComponentBase>();
        this.damageActionObj = GetComponent<DamageActionComponentBase>();
        if (this.statusObj == null || this.damageActionObj == null)
        {
            Destroy(gameObject);
            return;
        }
        this.damageCalculateObj = new DamageCalculateBase();
    }

    /**
     * ダメージ処理
     */
    public void callCalculateDamage(int attackValue)
    {
        callCalculateDamage(attackValue,null);
    }

    /**
     * ダメージ処理
     */
    public void callCalculateDamage(int attack,string actionCharName)
    {
        //ダメージ量の計算処理
        int calDamage = damageCalculateObj.calculateDamage(attack);
        //HPの減算処理
        int calHp = statusObj.charHp.subHp(calDamage);
        //ログ出力
        if (!string.IsNullOrEmpty(actionCharName) && !string.IsNullOrEmpty(statusObj.charName.name))
        {
            GManager.instance.wrightAttackLog(actionCharName, statusObj.charName.name, calDamage);
        }
        //ダメージを受けた際のアクション
        damageActionObj.reciveDamageAction(calHp);
    }

    /**
     * HPの回復処理
     */
    public void callCalculateRecoveryHp(int recovery)
    {
        statusObj.charHp.addHp(recovery);
    }
}
