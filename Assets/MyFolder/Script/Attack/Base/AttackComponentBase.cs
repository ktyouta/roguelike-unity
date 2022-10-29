using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackComponentBase : MonoBehaviour
{
    protected Animator animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void attack(int horizontalDirection, int verticalDirection)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(horizontalDirection, verticalDirection);
        StartCoroutine(attackAction(start, end));
    }

    /**
     * çUåÇÉAÉNÉVÉáÉì
     */
    public abstract IEnumerator attackAction(Vector2 start, Vector2 end);
}
