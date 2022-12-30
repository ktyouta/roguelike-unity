using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public abstract class Item : MonoBehaviour
{
    [Header("�A�C�e���̃^�C�v")] public ItemTypeEnum type;
    [HideInInspector]public int id;
    [Header("���O")]public new string name;
    [Header("�A�C�e���̐���")] public string itemDescription;
    [Header("�X�e�[�W��Փx�ɉ�����ID")] public int diffId;
    [Header("���l")] public int buyPrice;
    [Header("���l")] public int sellPrice;
    [HideInInspector] public bool isPut = false;
    public GameObject itemPanelObj;
    public GameObject itemPanel;
    public GameObject commandPanel;
    public GameObject itemDescriptionPanel;
    

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    /**
     * �A�C�e�����g�p
     */
    public abstract void useItem(StatusComponentBase statusObj);

    /**
     * �A�C�e�����Փ˂����ꍇ�̏���
     */
    public abstract void collisionItem(GameObject targetObj);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        //�A�C�e���𑫌��ɒu�����Ƃ�
        if (isPut)
        {
            return;
        }
        //���������̃`�F�b�N
        if (ItemManager.addItem(this.gameObject))
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPut = false;
        return;   
    }

    /**
     * �A�C�e���̃��X�g����Y������A�C�e�����폜����
     */
    public void deleteSelectedItem(int selectId)
    {
        for (int i = 0; i < ItemManager.itemList.Count; i++)
        {
            if (ItemManager.itemList[i].GetComponent<Item>().id == selectId)
            {
                ItemManager.itemList.RemoveAt(i);
                this.gameObject.SetActive(false);
            }
        }
    }
}
