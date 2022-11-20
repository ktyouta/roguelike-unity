using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorComponentRange : SensorComponentBase
{
    //�T�[�`�͈�
    const int horizontalDirection = 2;
    const int verticalDirection = 2;

    /**
     * �Ώۂ��T�[�`����(�͈͓�)
     */
    public override bool searchTarget(Vector2 nextSelfPosition, Vector2 nextTargetPosition)
    {
        float minHorizontal = nextSelfPosition.x - horizontalDirection;
        float maxHorizontal = nextSelfPosition.x + horizontalDirection;
        float minVertical = nextSelfPosition.y - verticalDirection;
        float maxVertical = nextSelfPosition.y + verticalDirection;
        float targetHorizotal = nextTargetPosition.x;
        float targetVertical = nextTargetPosition.y;
        //�͈͓��ɂ���ꍇ
        if (targetHorizotal >= minHorizontal && targetHorizotal <= maxHorizontal && targetVertical >= minVertical && targetVertical <= maxVertical)
        {
            return true;
        }
        return false;
    }
}
