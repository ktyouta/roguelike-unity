using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFellowTalk : NpcChoices
{
    protected override void selectMessage(int funcNumber)
    {
        //���b�Z�[�W�m�[�h�̐؂�ւ�
        nowNodeIndex = funcNumber;
        switch (funcNumber)
        {
            case 0:
                StartCoroutine(becomeFellow());
                break;
            case 1:
                break;
            default:
                break;
        }
        //�I����������Ɏ~�߂Ă�������(�R���[�`��)��i�߂�
        clickChioseButtonFlag = true;
    }

    /*
     * �v���C���[�̒��ԂɂȂ�
     */
    protected IEnumerator becomeFellow()
    {
        yield return new WaitUntil(() => isEndTalk);
        GManager.instance.fellows.Add(GetComponent<NpcFellow>());
        GetComponent<NpcFellow>().enabled = true;
        Transform gameTransform = this.gameObject.transform;
        //�q�I�u�W�F�N�g�̃R���C�_�[��j��
        foreach (Transform child in gameTransform)
        {
            Destroy(child.gameObject);
        }
        //���C���[��ύX
        this.gameObject.layer = LayerMask.NameToLayer("NpcFellow");
        GetComponent<NpcFellowTalk>().enabled = false;
    }
}
