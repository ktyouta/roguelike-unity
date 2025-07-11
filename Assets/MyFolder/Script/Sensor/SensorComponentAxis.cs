using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorComponentAxis : SensorComponentBase
{
    /**
     * 対象をサーチする(軸方向)
     */
    public override bool searchTarget(Vector2 nextSelfPosition, Vector2 nextTargetPosition)
    {
        Vector2 nowPosition = transform.position;

        //x軸またはy軸が一致する場合
        if (nowPosition.x == nextTargetPosition.x || nowPosition.y == nextTargetPosition.y)
        {
            return true;
        }
        return false;
    }
}
