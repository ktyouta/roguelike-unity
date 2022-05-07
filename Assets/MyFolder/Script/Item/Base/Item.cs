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
    [HideInInspector] public bool isEnter = false;
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
    public abstract void useItem();

    /**
     * �A�C�e�����Փ˂����ꍇ�̏���
     */
    public abstract void collisionItem(Enemy enemy);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        //�o���莞��2����s�����΍�
        isEnter = !isEnter;
        if (!isEnter)
        {
            return;
        }
        //�A�C�e���𑫌��ɒu�����Ƃ�
        if (isPut)
        {
            isEnter = !isEnter;
            isPut = !isPut;
            return;
        }
        //���������̃`�F�b�N
        if (GManager.instance.addItem(this.gameObject))
        {
            this.gameObject.SetActive(false);
        }
    }

    /**
     * �A�C�e���̃��X�g����Y������A�C�e�����폜����
     */
    public void deleteSelectedItem(int selectId)
    {
        for (int i = 0; i < GManager.instance.itemList.Count; i++)
        {
            if (GManager.instance.itemList[i].GetComponent<Item>().id == selectId)
            {
                GManager.instance.itemList.RemoveAt(i);
                this.gameObject.SetActive(false);
            }
        }
    }
}
