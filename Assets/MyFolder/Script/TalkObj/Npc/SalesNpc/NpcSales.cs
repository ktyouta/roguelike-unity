using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcSales : NpcBase
{
    [Header("���i�̃��X�g")] public List<GameObject> salesItemList;
    [Header("�V���b�v�̑I����")] public List<ShopChoiseClass> shopMenu;
    [Header("�������̑I����")] public List<ShopChoiseClass> tradeMenu;
    [Header("��b�J�n����̃��b�Z�[�W")] public string startMessage;
    [Header("��b�I�����̃��b�Z�[�W")] public string endMessage;
    [Header("���j���[�{�^��")] public GameObject menuBtn;
    [Header("�g���[�h���j���[")] public GameObject tradeBtn;
    [Header("�A�C�e���{�^��")] public GameObject itemBtn;
    [SerializeField, Header("�L�����̃X�e�[�^�X�p�R���|�[�l���g")] private StatusComponentPlayer statusComponentObj;
    private bool leaveShopFlag = false;
    //�����ɂ���:0�A����ɂ���:1�A��߂�:2
    private int shopMode;

    [System.Serializable]
    public class ShopChoiseClass
    {
        [Header("�V���b�v���j���[")]public string menu;
        [Header("���j���[�I�����Ɏ��s����֐��̃C���f�b�N�X")]public int funcNumber;
    }

    public override void Start()
    {
        base.Start();
        statusComponentObj = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();
    }

    protected override IEnumerator TalkEvent()
    {
        leaveShopFlag = false;
        showMessage(startMessage);
        //���j���[�̓W�J
        yield return startShopping();
        showMessage(endMessage);
        yield return new WaitUntil(() => Input.anyKeyDown);
    }

    /**
     * �p�l���A���j���[(�����ɂ����A����ɂ����A��߂�)�̓W�J
     */
    IEnumerator startShopping()
    {
        GManager.instance.shopPanel.SetActive(true);
        deleteChildButton(GManager.instance.shopPanel.transform);
        var parentPosition = GManager.instance.shopPanel.transform.position;
        //�I�����̐�����I�����p�l���̍��������߂�
        float panelHeight = shopMenu.Count * 40f + 10;
        RectTransform choisePanelRect = GManager.instance.shopPanel.GetComponent<RectTransform>();
        Vector2 size = choisePanelRect.sizeDelta;
        size.y = panelHeight;
        choisePanelRect.sizeDelta = size;
        //�{�^���̐ݒu���W��ݒ肷�邽�߂�pivot���擾
        float pivotY = choisePanelRect.pivot.y;
        for (int i=0;i< shopMenu.Count;i++)
        {
            //���j���[�{�^���̐���
            GameObject choiseButton = Instantiate(menuBtn, new Vector3(parentPosition.x, pivotY + panelHeight * 0.5f - 30 - i * 35, 0f), Quaternion.identity) as GameObject;
            //choisePanel�̎q�I�u�W�F�N�g�ɂ���
            choiseButton.transform.SetParent(GManager.instance.shopPanel.transform, false);
            choiseButton.transform.Find("Text").GetComponent<Text>().text = shopMenu[i].menu;
            int funcIndex = shopMenu[i].funcNumber;
            //�I�������N���b�N�����ۂ̃��\�b�h��ݒ�
            choiseButton.GetComponent<Button>().onClick.AddListener(() => selectShopChoiseFunc(funcIndex));
        }
        //��߂�{�^�������Ńg���[�h�I��
        yield return new WaitUntil(() => leaveShopFlag);
    }

    /**
     * ���j���[�N���b�N
     */
    void selectShopChoiseFunc(int index)
    {
        shopMode = index;
        switch (index)
        {
            //�A�C�e���w��
            case 0:
            //�A�C�e�����p
            case 1:
                tradeItem();
                break;
            //�g���[�h�I��
            case 2:
                leaveShop();
                break;
        }
        hideTradePanel();
    }

    /**
     * �����ɗ����܂��͔���ɗ������N���b�N
     */
    void tradeItem()
    {
        //�I�����{�^�������ׂč폜
        deleteChildButton(GManager.instance.shopItemListPanel.transform);
        GManager.instance.shopItemListPanel.SetActive(true);
        //GManager.instance.playerMoneyText.text = "�������F" + GManager.instance.playerMoney + " $";
        GManager.instance.playerMoneyText.text = "�������F" + statusComponentObj?.charWallet.money + " $";
        Vector3 parentPosition = GManager.instance.shopItemListPanel.transform.position;
        RectTransform choisePanelRect = GManager.instance.shopItemListPanel.GetComponent<RectTransform>();
        //�{�^���̐ݒu���W��ݒ肷�邽�߂�pivot���擾
        float pivotY = choisePanelRect.pivot.y;
        List<GameObject> displayItemList = new List<GameObject>();
        string npcTradeMessage = "";
        //�����ɗ���
        if (shopMode == 0)
        {
            displayItemList = salesItemList;
            npcTradeMessage = "�����w������܂����H";
        }
        //����ɗ���
        else if (shopMode == 1)
        {
            displayItemList = GManager.instance.itemList;
            npcTradeMessage = "���𔄋p����܂����H";
        }
        showMessage(npcTradeMessage);
        //�A�C�e���̃��X�g���p�l���ɓW�J
        deploymentItemList(displayItemList,parentPosition);
    }

    /**
     * �A�C�e���̃��X�g���p�l���ɓW�J����
     */
    void deploymentItemList(List<GameObject> displayItemList, Vector3 parentPosition)
    {
        for (int i = 0; i < displayItemList.Count; i++)
        {
            //���j���[�{�^���̐���
            GameObject choiseButton = Instantiate(itemBtn, new Vector3(parentPosition.x, parentPosition.y + 60 - i * 35, 0f), Quaternion.identity) as GameObject;
            //choisePanel�̎q�I�u�W�F�N�g�ɂ���
            choiseButton.transform.SetParent(GManager.instance.shopItemListPanel.transform, false);
            string tradePrice = "";
            Item item = displayItemList[i].GetComponent<Item>();
            if (shopMode == 0)
            {
                tradePrice = item.buyPrice.ToString();
            }
            else if (shopMode == 1)
            {
                tradePrice = item.sellPrice.ToString();
            }
            choiseButton.transform.Find("Text").GetComponent<Text>().text = item.name + "       " + tradePrice + "$";
            //�A�C�e�������N���b�N�����ۂ̃��\�b�h��ݒ�
            GameObject tempItem = displayItemList[i];
            choiseButton.GetComponent<Button>().onClick.AddListener(() => clickItemName(tempItem));
        }
    }

    /**
     * �A�C�e�������N���b�N
     */
    void clickItemName(GameObject argItem)
    {
        Item item = argItem.GetComponent<Item>();
        GManager.instance.itemDescriptionPanel.SetActive(true);
        string description = "�A�C�e�����F" + item.name;
        description += "\n\n";
        description += item.itemDescription;
        GManager.instance.itemDescriptionPanel.transform.Find("Text").GetComponent<Text>().text = description;
        GManager.instance.shopSelectPanel.SetActive(true);
        var parentPosition = GManager.instance.shopSelectPanel.transform.position;
        //��������{�^���̃��X�g���쐬
        List<ShopChoiseClass> tempTradeMenu = new List<ShopChoiseClass>();
        for (int i=0;i<tradeMenu.Count;i++)
        {
            //"�����ɗ���"�{�^���������͔�����"����ɗ���"�{�^�����������͔�������X�g�ɒǉ�
            //��߂�{�^���͏�ɒǉ�
            if (tradeMenu[i].funcNumber == shopMode || tradeMenu[i].funcNumber == 2)
            {
                tempTradeMenu.Add(tradeMenu[i]);
            }
        }
        //��������{�^���̔z�u
        for (int i=0;i< tempTradeMenu.Count;i++)
        {
            //���j���[�{�^���̐���
            GameObject choiseButton = Instantiate(tradeBtn, new Vector3(parentPosition.x-15, parentPosition.y + 5 - i * 35, 0f), Quaternion.identity) as GameObject;
            choiseButton.transform.SetParent(GManager.instance.shopSelectPanel.transform, false);
            choiseButton.transform.Find("Text").GetComponent<Text>().text = tempTradeMenu[i].menu;
            int funcIndex = tempTradeMenu[i].funcNumber;
            //�I�������N���b�N�����ۂ̃��\�b�h��ݒ�
            choiseButton.GetComponent<Button>().onClick.AddListener(() => clickShopTradeMenu(funcIndex, argItem));
        }
    }

    /**
     * �������莞�̊֐�
     */
    void clickShopTradeMenu(int index,GameObject item)
    {
        switch (index)
        {
            //�w������
            case 0:
                decisionBuyItem(item);
                break;
            //���p����
            case 1:
                decisionSellItem(item);
                break;
            //��߂�
            case 2:
                break;
        }
        hideTradePanel();
    }

    /**
     * ��������p�l���ƃA�C�e���̐����p�l�����\���ɂ���
     */
    void hideTradePanel()
    {
        GManager.instance.shopSelectPanel.SetActive(false);
        GManager.instance.itemDescriptionPanel.SetActive(false);
    }

    /**
     * �A�C�e���w������
     */
    void decisionBuyItem(GameObject argItem)
    {
        GameObject buyItem = Instantiate(argItem) as GameObject;
        Item item = buyItem.GetComponent<Item>();
        //�������̔���
        //if (GManager.instance.playerMoney < item.buyPrice)
        if(statusComponentObj?.charWallet.money < item.buyPrice)
        {
            showMessage("������������܂���B");
            return;
        }
        //�A�C�e���̏��������̔���
        if (!GManager.instance.addItem(buyItem))
        {
            showMessage("�A�C�e���������ς��ł��B");
            return;
        }
        //���������獷������
        //GManager.instance.playerMoney -= item.buyPrice;
        statusComponentObj?.charWallet.subMoney(item.buyPrice);
        //�w�������A�C�e����ID�����蓖�Ă�
        //item.assignItemId();
        //GManager.instance.playerMoneyText.text = "�������F" + GManager.instance.playerMoney + " $";
        GManager.instance.playerMoneyText.text = "�������F" + statusComponentObj?.charWallet.money + " $";
        hideTradePanel();
        showMessage("�������グ���肪�Ƃ��������܂��B");
    }

    /**
     * �A�C�e�����p����
     */
    void decisionSellItem(GameObject argItem)
    {
        Item item = argItem.GetComponent<Item>();
        for (int i=0;i< GManager.instance.itemList.Count;i++)
        {
            if (GManager.instance.itemList[i].GetComponent<Item>().id == item.id)
            {
                GManager.instance.itemList.RemoveAt(i);
                showMessage("�������܂����B");
                //�������𑝂₷
                //GManager.instance.playerMoney += item.sellPrice;
                statusComponentObj.charWallet.addMoney(item.sellPrice);
                //GManager.instance.playerMoneyText.text = "�������F" + GManager.instance.playerMoney + " $";
                GManager.instance.playerMoneyText.text = "�������F" + statusComponentObj?.charWallet.money + " $";
                deleteChildButton(GManager.instance.shopItemListPanel.transform);
                deploymentItemList(GManager.instance.itemList,GManager.instance.shopItemListPanel.transform.position);
                hideTradePanel();
                return;
            }
        }
        showMessage("����Ɏ��s���܂����B�A�C�e����ID�����蓖�Ă��Ă��܂���B");
    }

    /**
     * �g���[�h�I��
     */
    void leaveShop()
    {
        GManager.instance.shopPanel.SetActive(false);
        GManager.instance.shopItemListPanel.SetActive(false);
        hideTradePanel();
        leaveShopFlag = true;
    }

    /**
     * �����Ŏw�肵���p�l���̎q�I�u�W�F�N�g(�{�^��)�����ׂč폜����
     */
    void deleteChildButton(Transform childButtons)
    {
        //�I�����{�^�������ׂč폜
        foreach (Transform child in childButtons)
        {
            if (child.name != "PlayerMoneyText") Destroy(child.gameObject);
        }
    }
}