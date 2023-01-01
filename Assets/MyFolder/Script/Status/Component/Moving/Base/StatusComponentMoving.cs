using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ComponentSettingManager;


public class StatusComponentMoving : StatusComponentBase
{
    //攻撃力
    [HideInInspector] public AttackClass charAttack = new AttackClass();
    //防御力
    [HideInInspector] public DefenceClass charDefence = new DefenceClass();

    // Start is called before the first frame update
    protected override void Start()
    {
        //ステータス情報を取得
        CommonStatusInfoClass statusInfo = getStatusInfo(getJsonId());

        //ステータス情報をセット
        setParam(statusInfo);

        //各ステータスのMAX値の初期設定
        initializeStatus();
    }

    /**
     * Jsonからデータを取得するためのIDを取得
     */
    protected int getJsonId()
    {
        int? jsonId = GetComponent<MovingObject>()?.jsonDataId;
        if (jsonId == null)
        {
            Destroy(gameObject);
        }

        return (int)jsonId;
    }

    /**
     * ステータス情報を取得
     */
    protected virtual CommonStatusInfoClass getStatusInfo(int jsonId)
    {
        return new CommonStatusInfoClass();
    }

    /**
     * ステータス情報をセット
     */
    protected virtual void setParam(CommonStatusInfoClass statusInfo)
    {
        //名前
        charName.name = statusInfo.name;
        //HP
        charHp.hp = statusInfo.hp;
        //攻撃力
        charAttack.attack = statusInfo.attack;
        //防御力
        charDefence.defence = statusInfo.defence;
    }

    /**
     * 各ステータスのMAX値の初期設定
     */
    protected virtual void initializeStatus()
    {
        charHp.initializeMaxHp();
        charAttack.initializeTotalAttack();
        charDefence.initializeTotalDefence();
    }
}
