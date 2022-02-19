using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [HideInInspector]public int id;
    [Header("名前")]public string name;
    [Header("アイテムのタイプ")]public string type;
    [Header("アイテムの説明")] public string itemDescription;
    public bool isUsedFlag;
    public GameObject itemObj;
    public GameObject itemPanelObj;
    public GameObject itemPanel;
    public GameObject commandPanel;
    public GameObject itemDescriptionPanel;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        itemObj = this.gameObject;
        if (itemObj == null)
        {
            Debug.Log("nulnll");
        }
    }

    public virtual void useItem()
    {
        //changeListPos(id);
        itemPanelObj = GameObject.Find("ItemUseList");
        if (itemPanelObj == null)
        {
            Debug.Log(itemPanelObj);
        }
        commandPanel = GameObject.Find("CommandPanel");
        itemPanel = GameObject.Find("ItemPanel");
        itemDescriptionPanel = GameObject.Find("ItemDescriptionPanel");
        itemPanelObj.SetActive(false);
        itemPanel.SetActive(false);
        commandPanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        GManager.instance.isMenuOpen = !GManager.instance.isMenuOpen;
        //GManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //衝突したトリガーのタグがFoodであるか確認してください。
        if (other.tag == "Player")
        {
            //アイテム取得後、非表示
            GManager.instance.addItem(itemObj.GetComponent<Item>());
            int itemId = GManager.instance.itemList.Count;
            itemObj.GetComponent<Item>().id = itemId;
            //Debug.Log("reid" + itemObj.GetComponent<RecoveryItem>().id);
            itemObj.SetActive(false);
        }
    }

    //public void changeListPos(int index)
    //{
    //    GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
    //    float beforePos = itemBtns[0].transform.position.y;
    //    for (int i=0;i<itemBtns.Length;i++)
    //    {
    //        if (itemBtns[i].GetComponent<ItemNameButton>().itemNameButtonId + 1 > index)
    //        {
    //            Vector3 pos = itemBtns[i].transform.position;
    //            pos.y = beforePos;
    //            beforePos = itemBtns[i].transform.position.y;
    //            itemBtns[i].transform.position = pos;
    //        }
    //        else
    //        {
    //            beforePos = itemBtns[i].transform.position.y;
    //        }
    //    }
    //}
}
