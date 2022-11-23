using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorComponentAxis : SensorComponentBase
{
    /**
     * ‘ÎÛ‚ğƒT[ƒ`‚·‚é(²•ûŒü)
     */
    public override bool searchTarget(Vector2 nextSelfPosition, Vector2 nextTargetPosition)
    {
        Vector2 nowPosition = transform.position;

        //x²‚Ü‚½‚Íy²‚ªˆê’v‚·‚éê‡
        if (nowPosition.x == nextTargetPosition.x || nowPosition.y == nextTargetPosition.y)
        {
            return true;
        }
        return false;
    }
}
