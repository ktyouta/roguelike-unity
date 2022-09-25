using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�_���[�W�v�Z�A�_���[�W���󂯂���̍s��
public class DamageActionComponentBase : MonoBehaviour
{
    [HideInInspector] public StatusComponentBase statusObj;
    [HideInInspector] public DamageCalculateBase damageCalculateObj;
    [HideInInspector] public DamageActionBase damageActionObj;

    // Start is called before the first frame update
    public void Start()
    {
        this.statusObj = GetComponent<StatusComponentBase>();
        this.damageActionObj = GetComponent<DamageActionBase>();
        if (this.statusObj == null || this.damageActionObj == null)
        {
            Destroy(GetComponent<DamageActionComponentBase>());
            return;
        }
        setParam();
    }

    /**
     * �_���[�W�ʂ̌v�Z����
     */
    public int calculateDamage(int attack)
    {
        return damageCalculateObj.calculateDamage(attack);
    }

    /**
     * HP�̌��Z����
     */
    public int subHp(int calDamage)
    {
        return statusObj.charHp.subHp(calDamage);
    }

    /**
     * �_���[�W���󂯂��ۂ̃A�N�V����
     */
    public void reciveDamageAction(int calHp)
    {
        damageActionObj.reciveDamageAction(calHp);
    }

    /**
     * �K�v�ȃC���X�^���X���Z�b�g
     */
    public virtual void setParam()
    {
        this.damageCalculateObj = new DamageCalculateBase();
    }
}