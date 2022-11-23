using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponentEnemyThrow : AttackComponentEnemyBase
{
    ThrowComponentBase throwComponentObj;
    private GameObject throwItem;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        gameObject.AddComponent<ThrowComponentEnemy>();
        throwComponentObj = GetComponent<ThrowComponentBase>();
        throwItem = Resources.Load("Prefab/Item/Food") as GameObject;
    }

    /**
    * �U���A�N�V����
    */
    public override IEnumerator attackAction(Vector2 start, Vector2 end)
    {
        //�U���̏ꍇ�̓v���C���[�̍s��������҂�
        yield return new WaitUntil(() => GManager.instance.isEndPlayerAction);
        int verticalDirection = (int)(end.y - start.y);
        int horizontalDirection = (int)(end.x - start.x);
        //�A�C�e���𓊝�����
        yield return StartCoroutine(throwComponentObj.throwObject(verticalDirection, horizontalDirection, throwItem));
        //yield return new WaitForSeconds(0.5f);
        // �s���I���̃J�E���g
        GManager.instance.enemyActionEndCount++;
    }
}
