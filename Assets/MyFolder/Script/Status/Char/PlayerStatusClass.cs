using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatusClass
{
    [Header("プレイヤーのHP")] public int playerHp;
    [Header("プレイヤーの現在の上限HP")] public int nowPlayerMaxHp;
    [Header("プレイヤーの名前")] public string playerName;
    [Header("プレイヤーの所持金")] public int playerMoney;
    [Header("プレイヤーの攻撃力")] public int playerAttack;
    [Header("プレイヤーの防御力")] public int playerDefence;
    [Header("プレイヤーのレベル")] public int playerLevel = 1;
    [Header("プレイヤーの魅力")] public int playerCharm = 10;
    [Header("次のレベルまでの経験値")] public int nowMaxExprience;
    [Header("アイテムの所持数制限")] public int nowMaxPosession;
    [Header("プレイヤーの満腹度")] public int playerFoodPoint;
    [Header("プレイヤーの満腹度の上限値")] public int playerMaxFoodPoint = 100;

    /**
     * ダメージ計算
     */
    public void calculateDamage(int damage)
    {
        playerHp -= damage;
    }

    /**
     * 満腹度の減算
     */
    public void consumeFoodPoint(int consume)
    {
        playerFoodPoint -= consume;
    }

    /**
     * 満腹度の回復
     */
    public void recoveryFoodPoint(int recovery)
    {
        int afterPlayerHp = playerFoodPoint + recovery;
        playerFoodPoint = afterPlayerHp > playerMaxFoodPoint ? playerMaxFoodPoint : afterPlayerHp;
    }

    /**
     * HPの回復
     */
    public void recoveryHp(int recovery)
    {
        int afterPlayerFoodPoint = playerFoodPoint + recovery;
        playerFoodPoint = afterPlayerFoodPoint > playerMaxFoodPoint ? playerMaxFoodPoint : afterPlayerFoodPoint;
    }

    /**
     * ステータスの更新
     */
    public void updateStatus()
    {
        playerAttack += 2;
        playerDefence += 2;
        playerHp += 10;
        nowPlayerMaxHp += 10;
    }
}
