using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SensorComponentBase : MonoBehaviour
{
    /**
     * �Ώۂ��T�[�`����
     */
    public abstract bool searchTarget(Vector2 nextSelfPosition, Vector2 nextTargetPosition);
}
