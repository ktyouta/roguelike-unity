using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorComponentNext : SensorComponentBase
{
    /**
     * �Ώۂ��T�[�`����(�㉺���E����)
     */
    public override bool searchTarget(Vector2 nextSelfPosition, Vector2 nextTargetPosition)
    {
        //�ړ��悪���ꍇ
        if (nextSelfPosition == nextTargetPosition)
        {
            return true;
        }
        return false;
    }
}
