using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackComponentBase : MonoBehaviour
{
    protected Animator animator;
    protected MovingObject movingObj;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        movingObj = GetComponent<MovingObject>();
    }

    public IEnumerator attack(int horizontalDirection, int verticalDirection)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(horizontalDirection, verticalDirection);
        movingObj.isAttack = true;
        yield return StartCoroutine(attackAction(start, end));
        movingObj.isAttack = false;
    }

    /**
     * çUåÇÉAÉNÉVÉáÉì
     */
    public abstract IEnumerator attackAction(Vector2 start, Vector2 end);
}
