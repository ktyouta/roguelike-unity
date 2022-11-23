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
        Vector2 nowPosition = transform.position;

        //x���܂���y������v����ꍇ
        if (nowPosition.x == nextTargetPosition.x || nowPosition.y == nextTargetPosition.y)
        {
            return true;
        }
        return false;
    }
}
