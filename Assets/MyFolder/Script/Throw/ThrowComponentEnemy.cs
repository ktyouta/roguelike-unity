using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowComponentEnemy : ThrowComponentBase
{
    protected override IEnumerator throwAction(Item item, ThrowObject throwObj)
    {
        animator.Play("EnemyAttack");
        //�A�C�e������ʊO�ɏo�邩�A��Q���ɓ�����܂ōs���s��
        yield return new WaitUntil(() => !throwObj.isThrownObj);
    }
}
