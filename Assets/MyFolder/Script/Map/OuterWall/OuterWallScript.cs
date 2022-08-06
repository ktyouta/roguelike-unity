using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterWallScript : MonoBehaviour
{
    [Header("破壊不可フラグ")] public bool isIndestructible;
    [Header("壁の耐久値")] public int wallHp;
    [HideInInspector] public bool isAttacked = false;
    private int beforeWallHp;

    // Start is called before the first frame update
    void Start()
    {
        wallHp = wallHp == 0 ?100:wallHp;
        beforeWallHp = wallHp;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの攻撃を検知した場合
        if (!isAttacked)
        {
            return;
        }
        isAttacked = false;
        //耐久値が減少した場合
        if (beforeWallHp != wallHp)
        {
            GManager.instance.wrightLog("外壁の残り耐久値："+wallHp);
            beforeWallHp = wallHp;
        }
        else
        {
            GManager.instance.wrightLog("破壊不能オブジェクトです。");
        }
        // 破壊可能で耐久値が0以下
        if (wallHp <= 0 )
        {
            GManager.instance.wrightLog("壁が壊れた。");
            Destroy(this.gameObject);
        }
    }
}
