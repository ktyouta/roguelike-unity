using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterWallScript : MonoBehaviour
{
    [Header("破壊不可フラグ")] public bool isIndestructible;
    [Header("壁の耐久値")] public int wallHp;

    // Start is called before the first frame update
    void Start()
    {
        wallHp = wallHp == 0 ?100:wallHp;
    }

    /**
     * 外壁のダメージ計算
     */
    public void calculateWallDamage(int attackValue)
    {
        //破壊不可
        if (isIndestructible)
        {
            GManager.instance.wrightLog("破壊不能オブジェクトです。");
            return;
        }
        wallHp -= attackValue;
        if (wallHp <= 0)
        {
            GManager.instance.wrightLog("壁が壊れた。");
            Destroy(this.gameObject);
        }
        else
        {
            GManager.instance.wrightLog("外壁の残り耐久値：" + wallHp);
        }
    }
}
