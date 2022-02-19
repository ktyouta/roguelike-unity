using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    Transform itemPanelObj;
    public Text itemPanel;
    [Header("�A�C�e���{�^��")] public GameObject itemBtn;
    [Header("�A�C�e���g�p���ڃp�l��")] public GameObject itemUsePanel;
    [Header("�A�C�e�������p�p�l��")]public GameObject itemDescriptionPanel;
    private int beforeCount = 0;
    private int nowCount = 0;
    private int buttonPosy = 110;
    // Start is called before the first frame update
    void Start()
    {
        //itemPanelObj = new GameObject("ItemPanel").transform;
        itemPanelObj = GameObject.Find("ItemPanel").transform;
        //itemPanel = GameObject.Find("ItemPanel").GetComponent<Text>();
        //itemUsePanel = GameObject.Find("ItemUseList");
        //itemUsePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        nowCount = GManager.instance.itemList.Count;
        if (nowCount == 0)
        {
            itemPanel.enabled = true;
            itemPanel.text = "�A�C�e�����������Ă��܂���";
        }
        else
        {
            itemPanel.enabled = false;
            if (nowCount > beforeCount)
            {
                buttonPosy = 110;
                Debug.Log("itemlistcount"+ GManager.instance.itemList.Count);
                for (var i = 0; i < GManager.instance.itemList.Count; i++)
                {
                    if (!GManager.instance.itemList[i].isUsedFlag)
                    {
                        buttonPosy -= 30;
                    }
                    if (i >= beforeCount)
                    {
                        //GameObject listButton = Instantiate(itemBtn) as GameObject;
                        GameObject listButton = Instantiate(itemBtn, new Vector3(-10, buttonPosy, 0f), Quaternion.identity) as GameObject;
                        listButton.transform.SetParent(itemPanelObj, false);
                        //Debug.Log("count" + GManager.instance.itemList.Count);
                        listButton.transform.Find("Text").GetComponent<Text>().text = GManager.instance.itemList[i].name;
                        int index = i;
                        listButton.GetComponent<ItemNameButton>().itemNameButtonId = index;
                        //�A�C�e�����N���b�N�����Ƃ��̊֐���ݒ�
                        Debug.Log("index" + index);
                        listButton.GetComponent<Button>().onClick.AddListener(() => clickItemButton(GManager.instance.itemList[index], listButton,index));
                        //Debug.Log(listButton.transform);
                    }
                }
                beforeCount = nowCount;
            }
        }
    }

    public  void clickItemButton(Item item,GameObject listButton,int index)
    {
        GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
        //Debug.Log("index"+index);
        for (int i = 0; i < itemBtns.Length; i++)
        {
            if (itemBtns[i].GetComponent<ItemNameButton>().itemNameButtonId == index)
            {
                itemBtns[i].GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                itemBtns[i].GetComponent<Image>().color = new Color32(140, 168, 166, 255);
            }
        }
        itemDescriptionPanel.SetActive(true);
        string description = "�A�C�e�����F" + item.name;
        description += "\n\n";
        description += item.itemDescription;
        itemDescriptionPanel.transform.Find("Text").GetComponent<Text>().text = description;
        GManager.instance.itemUsePanel.SetActive(true);
        GManager.instance.itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.RemoveAllListeners();
        //����n�A�C�e��
        if (item.type == "foodRecovery" || item.type == "portion")
        {
            GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "����";
        }
        //�����n�A�C�e��
        else if (item.type == "Equipment")
        {
            if (((EquipmentBase)item).isEquip)
            {
                GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "�͂���";
            }
            else
            {
                GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "������";
            }
        }
        GManager.instance.itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => { adduUseItemFunc(item, listButton); });
        GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.AddListener(() => { Debug.Log("�u��"); GManager.instance.itemUsePanel.SetActive(false); });
        GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.AddListener(() => { Debug.Log("������"); GManager.instance.itemUsePanel.SetActive(false); });
        //Debug.Log(GManager.instance.itemUsePanel);
        //Debug.Log(n);
    }

    public void adduUseItemFunc(Item item,GameObject useBtnObj)
    {
        item.useItem();
        //Destroy(useBtnObj);
    }
}
