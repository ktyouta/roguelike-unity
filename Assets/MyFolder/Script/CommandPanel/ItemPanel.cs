using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    Transform itemPanelObj;
    public Text itemPanel;
    [Header("アイテムボタン")] public GameObject itemBtn;
    [Header("アイテム使用項目パネル")] public GameObject itemUsePanel;
    [Header("アイテム説明用パネル")]public GameObject itemDescriptionPanel;
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
            itemPanel.text = "アイテムを所持していません";
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
                        //アイテムをクリックしたときの関数を設定
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
        string description = "アイテム名：" + item.name;
        description += "\n\n";
        description += item.itemDescription;
        itemDescriptionPanel.transform.Find("Text").GetComponent<Text>().text = description;
        GManager.instance.itemUsePanel.SetActive(true);
        GManager.instance.itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.RemoveAllListeners();
        //消費系アイテム
        if (item.type == "foodRecovery" || item.type == "portion")
        {
            GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "つかう";
        }
        //装備系アイテム
        else if (item.type == "Equipment")
        {
            if (((EquipmentBase)item).isEquip)
            {
                GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "はずす";
            }
            else
            {
                GManager.instance.itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "そうび";
            }
        }
        GManager.instance.itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => { adduUseItemFunc(item, listButton); });
        GManager.instance.itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.AddListener(() => { Debug.Log("置く"); GManager.instance.itemUsePanel.SetActive(false); });
        GManager.instance.itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.AddListener(() => { Debug.Log("投げる"); GManager.instance.itemUsePanel.SetActive(false); });
        //Debug.Log(GManager.instance.itemUsePanel);
        //Debug.Log(n);
    }

    public void adduUseItemFunc(Item item,GameObject useBtnObj)
    {
        item.useItem();
        //Destroy(useBtnObj);
    }
}
