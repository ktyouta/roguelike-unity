using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SensorComponentBase : MonoBehaviour
{
    /**
     * 対象をサーチする
     */
    public abstract bool searchTarget(Vector2 nextSelfPosition, Vector2 nextTargetPosition);
}
