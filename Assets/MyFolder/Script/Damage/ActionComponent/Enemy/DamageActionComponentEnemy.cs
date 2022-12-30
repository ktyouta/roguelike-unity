using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageActionComponentEnemy : DamageActionComponentBase
{
    protected bool isDefeat = false;
    private StatusComponentEnemy statusComponentEnemyObj;
    [SerializeField, HideInInspector] public StatusComponentPlayer statusComponentObj;

    protected override void Start()
    {
        base.Start();
        statusComponentEnemyObj = (StatusComponentEnemy)this.statusObj;
        statusComponentObj = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();
    }

    /**
     * �_���[�W���󂯂��ۂ̋��ʃA�N�V����
     */
    public override void reciveDamageAction(int hp)
    {
        if (isDefeat)
        {
            return;
        }
        if (hp <= 0)
        {
            isDefeat = true;
            //�v���C���[�̏�������ǉ�
            statusComponentObj?.charWallet.addMoney(statusComponentEnemyObj.charWallet.money);
            //�o���l��ǉ�
            statusComponentObj?.charExperience.addExperience(statusComponentEnemyObj.experience);
            //GManager.instance.wrightDeadLog(statusComponentEnemyObj.charName.name);
            LogMessageManager.wrightLog(MessageManager.createMessage("6", statusComponentEnemyObj.charName.name));
            GManager.instance.removeEnemyToList(GetComponent<Enemy>());
            //�G���Ƃ̌ŗL�A�N�V����
            specialAction();
            Destroy(gameObject, 0.5f);
        }
    }

    /**
     * �G���Ƃ̌ŗL�A�N�V����
     */
    protected virtual void specialAction() { }
}
