using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ダメージ計算、ダメージを受けた後の行動
public class DamageActionComponentBase : MonoBehaviour
{
    [HideInInspector] public StatusComponentBase statusObj;
    [HideInInspector] public DamageCalculateBase damageCalculateObj;
    [HideInInspector] public DamageActionBase damageActionObj;

    // Start is called before the first frame update
    public void Start()
    {
        this.statusObj = GetComponent<StatusComponentBase>();
        this.damageActionObj = GetComponent<DamageActionBase>();
        if (this.statusObj == null || this.damageActionObj == null)
        {
            Destroy(GetComponent<DamageActionComponentBase>());
            return;
        }
        setParam();
    }

    /**
     * ダメージ量の計算処理
     */
    public int calculateDamage(int attack)
    {
        return damageCalculateObj.calculateDamage(attack);
    }

    /**
     * HPの減算処理
     */
    public int subHp(int calDamage)
    {
        return statusObj.charHp.subHp(calDamage);
    }

    /**
     * ダメージを受けた際のアクション
     */
    public void reciveDamageAction(int calHp)
    {
        damageActionObj.reciveDamageAction(calHp);
    }

    /**
     * 必要なインスタンスをセット
     */
    public virtual void setParam()
    {
        this.damageCalculateObj = new DamageCalculateBase();
    }
}