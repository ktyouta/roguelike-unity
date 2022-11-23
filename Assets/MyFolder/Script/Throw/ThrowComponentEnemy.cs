using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowComponentEnemy : ThrowComponentBase
{
    protected override IEnumerator throwAction(Item item, ThrowObject throwObj)
    {
        animator.Play("EnemyAttack");
        //アイテムが画面外に出るか、障害物に当たるまで行動不可
        yield return new WaitUntil(() => !throwObj.isThrownObj);
    }
}
