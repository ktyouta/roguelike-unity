using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NameClass
{
    [Header("キャラの名前")] public string name;

    /**
     * 仮名をセット
     */
    public void initializeName()
    {
        name = "オブジェクト";
    }
}
