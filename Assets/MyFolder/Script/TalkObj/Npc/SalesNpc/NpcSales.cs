using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcSales : NpcBase
{
    [Header("商品のリスト")] public List<Item> salesItemList;
    [Header("ショップの選択肢")] public List<ShopChoiseClass> shopMenu;
    [Header("会話開始直後のメッセージ")] public string startMessage;
    [Header("会話終了時のメッセージ")] public string endMessage;
    [Header("メニューボタン")] public GameObject menuBtn;
    [Header("アイテムボタン")] public GameObject itemBtn;
    private bool leaveShopFlag = false;

    [System.Serializable]
    public class ShopChoiseClass
    {
        [Header("ショップメニュー")]public string menu;
        [Header("メニュー選択時に実行する関数のインデックス")]public int funcNumber;
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
        //選択肢の数から選択肢パネルの高さを求める
        float panelHeight = shopMenu.Count * 40f + 10;
        RectTransform choisePanelRect = GManager.instance.shopPanel.GetComponent<RectTransform>();
        Vector2 size = choisePanelRect.sizeDelta;
        size.y = panelHeight;
        choisePanelRect.sizeDelta = size;
        //ボタンの設置座標を設定するためにpivotを取得
        float pivotY = choisePanelRect.pivot.y;
        for (int i=0;i< shopMenu.Count;i++)
        {
            //メニューボタンの生成
            GameObject choiseButton = Instantiate(menuBtn, new Vector3(parentPosition.x, pivotY + panelHeight * 0.5f - 30 - i * 35, 0f), Quaternion.identity) as GameObject;
            //choisePanelの子オブジェクトにする
            choiseButton.transform.SetParent(GManager.instance.shopPanel.transform, false);
            choiseButton.transform.Find("Text").GetComponent<Text>().text = shopMenu[i].menu;
            int funcIndex = shopMenu[i].funcNumber;
            //選択肢をクリックした際のメソッドを設定
            choiseButton.GetComponent<Button>().onClick.AddListener(() => selectShopChoiseFunc(funcIndex));
        }
        yield return new WaitUntil(() => leaveShopFlag);
    }

    /**
     * メニュークリック
     */
    void selectShopChoiseFunc(int index)
    {
        switch (index)
        {
            //アイテム購入
            case 0:
                buyItem();
                break;
            //アイテム売却
            case 1:
                sellItem();
                break;
            //買い物終了
            case 2:
                leaveShop();
                break;
        }
    }

    /**
     * 買いに来たをクリック
     */
    void buyItem()
    {
        //選択肢ボタンをすべて削除
        deleteSelectButton();
        Debug.Log("buy");
        GManager.instance.shopItemListPanel.SetActive(true);
        GManager.instance.playerMoneyText.text = "所持金：" + GManager.instance.playerMoney + " $";
        var parentPosition = GManager.instance.shopItemListPanel.transform.position;
        RectTransform choisePanelRect = GManager.instance.shopItemListPanel.GetComponent<RectTransform>();
        //ボタンの設置座標を設定するためにpivotを取得
        float pivotY = choisePanelRect.pivot.y;
        for (int i=0;i< salesItemList.Count;i++)
        {
            //メニューボタンの生成
            GameObject choiseButton = Instantiate(itemBtn, new Vector3(parentPosition.x, parentPosition.y + 60 - i * 35, 0f), Quaternion.identity) as GameObject;
            //choisePanelの子オブジェクトにする
            choiseButton.transform.SetParent(GManager.instance.shopItemListPanel.transform, false);
            choiseButton.transform.Find("Text").GetComponent<Text>().text = salesItemList[i].name + "       " + salesItemList[i].buyPrice + "$";
            Item argItem = salesItemList[i];
            //選択肢をクリックした際のメソッドを設定
            choiseButton.GetComponent<Button>().onClick.AddListener(() => clickItemName(argItem));
        }
    }

    /**
     * アイテム名をクリック
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
     * アイテム購入決定
     */
    void decisionBuyItem(Item item)
    {
        //所持金の判定
        if (GManager.instance.playerMoney >= item.buyPrice)
        {
            GManager.instance.playerMoney -= item.buyPrice;
            GManager.instance.addItem(item);
            int itemId = GManager.instance.itemList.Count;
            item.id = itemId;
            GManager.instance.playerMoneyText.text = "所持金：" + GManager.instance.playerMoney + " $";
            showMessage("お買い上げありがとうございます。");
        }
        else
        {
            showMessage("所持金が足りません。");
        }
    }

    /**
     * アイテムの売却
     */
    void sellItem()
    {
        //選択肢ボタンをすべて削除
        deleteSelectButton();
        Debug.Log("sell");
    }

    /**
     * 買い物終了
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
        //選択肢ボタンをすべて削除
        foreach (Transform child in GManager.instance.shopItemListPanel.transform)
        {
            if (child.name != "PlayerMoneyText") Destroy(child.gameObject);
        }
        Debug.Log("sell");
    }
}
