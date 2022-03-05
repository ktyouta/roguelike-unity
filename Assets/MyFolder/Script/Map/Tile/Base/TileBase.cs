using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    [Header("マップオブジェクト設置用ルール")] public int settingFuncRule;
    [Header("マップ設置時の密度パラメータ")] public int denceParam;
    //settingFuncRuleで1をセットした際に用いるパラメータ
    [Header("集合体の構成要素数の最小値")] public int minSettingNum;
    [Header("集合体の構成要素数の最大値")] public int maxSettingNum;
    //settingFuncRuleで2をセットした際に用いるパラメータ
    [Header("マップ一区画の行数とこのパラメータの積が縦方向の長さになる(0～1の範囲で設定)")] public float verticalParam;
    [Header("マップ一区画の列数とこのパラメータの積が横方向の長さになる(0～1の範囲で設定)")] public float horizontalParam;

    [Header("マップの１区画分の横幅をこのパラメータで割った値が(集合体の)設置個数になる")] public int objectSettingNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
