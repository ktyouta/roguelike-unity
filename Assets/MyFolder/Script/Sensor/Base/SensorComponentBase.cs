using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SensorComponentBase : MonoBehaviour
{
    /**
     * ‘ÎÛ‚ğƒT[ƒ`‚·‚é
     */
    public abstract bool searchTarget(Vector2 nextSelfPosition, Vector2 nextTargetPosition);
}
