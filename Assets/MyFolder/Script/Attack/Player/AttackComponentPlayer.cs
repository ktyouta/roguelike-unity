using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponentPlayer : AttackComponentPlayerBase
{
    /**
    * �U���A�N�V����
    */
    public override IEnumerator attackAction(Vector2 start, Vector2 end)
    {
        animator.Play("PlayerAttack");
        RaycastHit2D hit = Physics2D.Linecast(start, end, enemyLayer | treasureLayer | blockingLayer);
        //�U���̏ꍇ�͌��ݒn��ǉ�
        GManager.instance.enemyNextPosition.Add(transform.position);
        //���ԍ��Ŏ��s�����t���O���I���ɂ��鏈��
        yield return new WaitForSeconds(0.3f);
        GManager.instance.isEndPlayerAction = true;
        GManager.instance.playersTurn = false;

        //�ΏۃI�u�W�F�N�g�̃_���[�W�������s��
        OutAccessComponentBase outAccessObj = hit.transform?.gameObject?.GetComponent<OutAccessComponentBase>();
        //�_���[�W����
        outAccessObj?.callCalculateDamage(statusObj.charAttack.totalAttack,
            GManager.instance.messageManager.createMessage("1", statusObj.charName.name, outAccessObj.statusObj.charName.name, statusObj.charAttack.totalAttack.ToString()));
    }
}
