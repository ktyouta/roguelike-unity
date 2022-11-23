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
        //アイテムが画面外に出るか、障害物に当たるまで行動不可
        yield return new WaitUntil(() => !throwObj.isThrownObj);
        //インベントリーから削除
        item.deleteSelectedItem(item.id);
        GManager.instance.charsNextPosition.Add(transform.position);
        GManager.instance.playersTurn = false;
        GManager.instance.isEndPlayerAction = true;
        playerObj.setPlayerState(playerState.Normal);
    }
}
