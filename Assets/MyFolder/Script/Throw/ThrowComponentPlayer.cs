using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static player;

public class ThrowComponentPlayer : ThrowComponentBase
{
    player playerObj;
    protected override void Start()
    {
        base.Start();
        playerObj = GetComponent<player>();
    }

    protected override IEnumerator throwAction(Item item, ThrowObject throwObj)
    {
        playerObj.isAttack = true;
        animator?.Play("PlayerAttack");
        GManager.instance.isCloseCommand = true;
        playerObj.setPlayerState(playerState.Wait);
        //�A�C�e������ʊO�ɏo�邩�A��Q���ɓ�����܂ōs���s��
        yield return new WaitUntil(() => !throwObj.isThrownObj);
        //�C���x���g���[����폜
        item.deleteSelectedItem(item.id);
        GManager.instance.charsNextPosition.Add(transform.position);
        GManager.instance.playersTurn = false;
        GManager.instance.isEndPlayerAction = true;
        playerObj.setPlayerState(playerState.Normal);
    }
}
