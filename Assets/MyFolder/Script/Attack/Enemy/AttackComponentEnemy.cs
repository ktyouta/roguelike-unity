using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponentEnemy : AttackComponentEnemyBase
{
    /**
    * �U���A�N�V����
    */
    public override IEnumerator attackAction(Vector2 start, Vector2 end)
    {
        //�U���̏ꍇ�̓v���C���[�̍s��������҂�
        yield return new WaitUntil(() => GManager.instance.isEndPlayerAction);
        RaycastHit2D hit = Physics2D.Linecast(start, end, playerLayer);
        animator.Play("EnemyAttack");

        //�ΏۃI�u�W�F�N�g�̃_���[�W�������s��
        OutAccessComponentBase outAccessObj = hit.transform?.gameObject?.GetComponent<OutAccessComponentBase>();
        //�_���[�W����
        outAccessObj?.callCalculateDamage(statusObj.charAttack.totalAttack, GManager.instance.messageManager.createMessage("1", statusObj.charName.name, outAccessObj.statusObj.charName.name, statusObj.charAttack.totalAttack.ToString()));
        yield return new WaitForSeconds(0.5f);
        // �s���I���̃J�E���g
        GManager.instance.enemyActionEndCount++;
    }
}
