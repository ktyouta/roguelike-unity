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
        //x²‚Ü‚½‚Íy²‚ªˆê’v‚·‚éê‡
        if (nextSelfPosition.x == nextTargetPosition.x || nextSelfPosition.y == nextTargetPosition.y)
        {
            return true;
        }
        return false;
    }
}
