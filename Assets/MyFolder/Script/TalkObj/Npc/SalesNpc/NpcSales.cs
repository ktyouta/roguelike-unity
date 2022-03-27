using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcSales : NpcBase
{
    [Header("���i�̃��X�g")] public List<Item> salesItemList;
    [Header("�V���b�v�̑I����")] public List<ShopChoiseClass> shopMenu;
    [Header("��b�J�n����̃��b�Z�[�W")] public string startMessage;
    [Header("��b�I�����̃��b�Z�[�W")] public string endMessage;
    [Header("���j���[�{�^��")] public GameObject menuBtn;
    [Header("�A�C�e���{�^��")] public GameObject itemBtn;
    private bool leaveShopFlag = false;

    [System.Serializable]
    public class ShopChoiseClass
    {
        [Header("�V���b�v���j���[")]public string menu;
        [Header("���j���[�I�����Ɏ��s����֐��̃C���f�b�N�X")]public int funcNumber;
    }

    protected override IEnumerator TalkEvent()
    {
        leaveShopFlag = false;
        showMessage(startMessage);
        yield return startShopping();
        showMessage(endMessage);
        yield return new WaitUntil(() => Input.anyKeyDown);
    }

    IEnumerator startShopping()
    {
        GManager.instance.shopPanel.SetActive(true);
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
        yield return new WaitUntil(() => leaveShopFlag);
    }

    /**
     * ���j���[�N���b�N
     */
    void selectShopChoiseFunc(int index)
    {
        switch (index)
        {
            //�A�C�e���w��
            case 0:
                buyItem();
                break;
            //�A�C�e�����p
            case 1:
                sellItem();
                break;
            //�������I��
            case 2:
                leaveShop();
                break;
        }
    }

    /**
     * �����ɗ������N���b�N
     */
    void buyItem()
    {
        //�I�����{�^�������ׂč폜
        deleteSelectButton();
        Debug.Log("buy");
        GManager.instance.shopItemListPanel.SetActive(true);
        GManager.instance.playerMoneyText.text = "�������F" + GManager.instance.playerMoney + " $";
        var parentPosition = GManager.instance.shopItemListPanel.transform.position;
        RectTransform choisePanelRect = GManager.instance.shopItemListPanel.GetComponent<RectTransform>();
        //�{�^���̐ݒu���W��ݒ肷�邽�߂�pivot���擾
        float pivotY = choisePanelRect.pivot.y;
        for (int i=0;i< salesItemList.Count;i++)
        {
            //���j���[�{�^���̐���
            GameObject choiseButton = Instantiate(itemBtn, new Vector3(parentPosition.x, parentPosition.y + 60 - i * 35, 0f), Quaternion.identity) as GameObject;
            //choisePanel�̎q�I�u�W�F�N�g�ɂ���
            choiseButton.transform.SetParent(GManager.instance.shopItemListPanel.transform, false);
            choiseButton.transform.Find("Text").GetComponent<Text>().text = salesItemList[i].name + "       " + salesItemList[i].buyPrice + "$";
            Item argItem = salesItemList[i];
            //�I�������N���b�N�����ۂ̃��\�b�h��ݒ�
            choiseButton.GetComponent<Button>().onClick.AddListener(() => clickItemName(argItem));
        }
    }

    /**
     * �A�C�e�������N���b�N
     */
    void clickItemName(Item item)
    {
        Debug.Log(item.name);
        GManager.instance.itemDescriptionPanel.SetActive(true);
        GManager.instance.itemDescriptionPanel.transform.Find("Text").GetComponent<Text>().text = item.itemDescription;
        GManager.instance.ShopSelectPanel.SetActive(true);
        decisionBuyItem(item);
    }

    /**
     * �A�C�e���w������
     */
    void decisionBuyItem(Item item)
    {
        //�������̔���
        if (GManager.instance.playerMoney >= item.buyPrice)
        {
            GManager.instance.playerMoney -= item.buyPrice;
            GManager.instance.addItem(item);
            int itemId = GManager.instance.itemList.Count;
            item.id = itemId;
            GManager.instance.playerMoneyText.text = "�������F" + GManager.instance.playerMoney + " $";
            showMessage("�������グ���肪�Ƃ��������܂��B");
        }
        else
        {
            showMessage("������������܂���B");
        }
    }

    /**
     * �A�C�e���̔��p
     */
    void sellItem()
    {
        //�I�����{�^�������ׂč폜
        deleteSelectButton();
        Debug.Log("sell");
    }

    /**
     * �������I��
     */
    void leaveShop()
    {
        Debug.Log("fin");
        GManager.instance.shopPanel.SetActive(false);
        GManager.instance.shopItemListPanel.SetActive(false);
        GManager.instance.itemDescriptionPanel.SetActive(false);
        GManager.instance.ShopSelectPanel.SetActive(false);
        leaveShopFlag = true;
    }

    void deleteSelectButton()
    {
        //�I�����{�^�������ׂč폜
        foreach (Transform child in GManager.instance.shopItemListPanel.transform)
        {
            if (child.name != "PlayerMoneyText") Destroy(child.gameObject);
        }
        Debug.Log("sell");
    }
}
