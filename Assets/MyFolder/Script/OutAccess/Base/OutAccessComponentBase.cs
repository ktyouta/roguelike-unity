using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutAccessComponentBase : MonoBehaviour
{
    public StatusComponentBase statusObj;
    private DamageCalculateBase damageCalculateObj;
    private DamageActionComponentBase damageActionObj;

    // Start is called before the first frame update
    public void Start()
    {
        this.statusObj = GetComponent<StatusComponentBase>();
        this.damageActionObj = GetComponent<DamageActionComponentBase>();
        if (this.statusObj == null || this.damageActionObj == null)
        {
            Destroy(gameObject);
            return;
        }
        this.damageCalculateObj = new DamageCalculateBase();
    }

    /**
     * �_���[�W����
     */
    public void callCalculateDamage(int attackValue)
    {
        callCalculateDamage(attackValue,null);
    }

    /**
     * �_���[�W����
     */
    public void callCalculateDamage(int attack,string message)
    {
        //�_���[�W�ʂ̌v�Z����
        int calDamage = damageCalculateObj.calculateDamage(attack);
        //HP�̌��Z����
        int calHp = statusObj.charHp.subHp(calDamage);
        //���O�o��
        if (!string.IsNullOrEmpty(message))
        {
            GManager.instance.wrightLog(message);
        }
        //�_���[�W���󂯂��ۂ̃A�N�V����
        damageActionObj.reciveDamageAction(calHp);
    }

    /**
     * HP�̉񕜏���
     */
    public void callCalculateRecoveryHp(int recovery)
    {
        callCalculateRecoveryHp(recovery,null);
    }

    /**
     * HP�̉񕜏���
     */
    public void callCalculateRecoveryHp(int recovery,string message)
    {
        statusObj.charHp.addHp(recovery);
        //���O�o��
        if (!string.IsNullOrEmpty(message))
        {
            GManager.instance.wrightLog(message);

        }
    }
}
