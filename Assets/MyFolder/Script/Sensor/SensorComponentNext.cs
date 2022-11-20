using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorComponentNext : SensorComponentBase
{
    /**
     * 対象をサーチする(上下左右方向)
     */
    public override bool searchTarget(Vector2 nextSelfPosition, Vector2 nextTargetPosition)
    {
        //移動先が被る場合
        if (nextSelfPosition == nextTargetPosition)
        {
            return true;
        }
        return false;
    }
}
