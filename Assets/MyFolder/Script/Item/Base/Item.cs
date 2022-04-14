using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector]public int id;
    [Header("���O")]public string name;
    [Header("�A�C�e���̃^�C�v")]public string type;
    [Header("�A�C�e���̐���")] public string itemDescription;
    [Header("�X�e�[�W��Փx�ɉ�����ID")] public int diffId;
    [Header("���l")] public int buyPrice;
    [Header("���l")] public int sellPrice;
    [HideInInspector] public bool isUsedFlag = false;
    public GameObject itemPanelObj;
    public GameObject itemPanel;
    public GameObject commandPanel;
    public GameObject itemDescriptionPanel;
    [HideInInspector] public bool isEnter = false;
    [HideInInspector] public bool isPut = false;
    private player playerObj = null;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //itemObj = this.gameObject;
    }

    /**
     * �A�C�e�����g�p
     */
    public virtual void useItem()
    {
        itemPanelObj = GameObject.Find("ItemUseList");
        commandPanel = GameObject.Find("CommandPanel");
        itemPanel = GameObject.Find("ItemPanel");
        itemDescriptionPanel = GameObject.Find("ItemDescriptionPanel");
        itemPanelObj.SetActive(false);
        itemPanel.SetActive(false);
        commandPanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        GManager.instance.isMenuOpen = !GManager.instance.isMenuOpen;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //�Փ˂����g���K�[�̃^�O��Food�ł��邩�m�F���Ă��������B
        if (other.tag == "Player")
        {
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
            //�A�C�e���̏��������𒴂��Ă���ꍇ
            if (GManager.instance.itemList.Count > GManager.instance.nowMaxPosession)
            {
                GManager.instance.wrightInventoryFullLog();
                return;
            }
            //�A�C�e���擾��A��\��
            addItemInventory();
        }
    }

    /**
     * �v���C���[�̃C���x���g���ɃA�C�e����ǉ�����
     */
    public void addItemInventory()
    {
        int itemId = GManager.instance.itemList.Count;
        GetComponent<Item>().id = itemId;
        GManager.instance.addItem(this.gameObject);
        this.gameObject.SetActive(false);
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
