using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorComponentAxis : SensorComponentBase
{
    /**
     * �Ώۂ��T�[�`����(������)
     */
    public override bool searchTarget(Vector2 nextSelfPosition, Vector2 nextTargetPosition)
    {
        //x���܂���y������v����ꍇ
        if (nextSelfPosition.x == nextTargetPosition.x || nextSelfPosition.y == nextTargetPosition.y)
        {
            return true;
        }
        return false;
    }
}
