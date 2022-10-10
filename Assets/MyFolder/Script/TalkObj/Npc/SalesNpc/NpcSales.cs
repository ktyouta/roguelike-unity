using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcSales : NpcBase
{
    [Header("商品のリスト")] public List<GameObject> salesItemList;
    [Header("ショップの選択肢")] public List<ShopChoiseClass> shopMenu;
    [Header("売買時の選択肢")] public List<ShopChoiseClass> tradeMenu;
    [Header("会話開始直後のメッセージ")] public string startMessage;
    [Header("会話終了時のメッセージ")] public string endMessage;
    [Header("メニューボタン")] public GameObject menuBtn;
    [Header("トレードメニュー")] public GameObject tradeBtn;
    [Header("アイテムボタン")] public GameObject itemBtn;
    [SerializeField, Header("キャラのステータス用コンポーネント")] private StatusComponentPlayer statusComponentObj;
    private bool leaveShopFlag = false;
    //買いにきた:0、売りにきた:1、やめる:2
    private int shopMode;

    [System.Serializable]
    public class ShopChoiseClass
    {
        [Header("ショップメニュー")]public string menu;
        [Header("メニュー選択時に実行する関数のインデックス")]public int funcNumber;
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
        //メニューの展開
        yield return startShopping();
        showMessage(endMessage);
        yield return new WaitUntil(() => Input.anyKeyDown);
    }

    /**
     * パネル、メニュー(買いにきた、売りにきた、やめる)の展開
     */
    IEnumerator startShopping()
    {
        GManager.instance.shopPanel.SetActive(true);
        deleteChildButton(GManager.instance.shopPanel.transform);
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
        //やめるボタン押下でトレード終了
        yield return new WaitUntil(() => leaveShopFlag);
    }

    /**
     * メニュークリック
     */
    void selectShopChoiseFunc(int index)
    {
        shopMode = index;
        switch (index)
        {
            //アイテム購入
            case 0:
            //アイテム売却
            case 1:
                tradeItem();
                break;
            //トレード終了
            case 2:
                leaveShop();
                break;
        }
        hideTradePanel();
    }

    /**
     * 買いに来たまたは売りに来たをクリック
     */
    void tradeItem()
    {
        //選択肢ボタンをすべて削除
        deleteChildButton(GManager.instance.shopItemListPanel.transform);
        GManager.instance.shopItemListPanel.SetActive(true);
        //GManager.instance.playerMoneyText.text = "所持金：" + GManager.instance.playerMoney + " $";
        GManager.instance.playerMoneyText.text = "所持金：" + statusComponentObj?.charWallet.money + " $";
        Vector3 parentPosition = GManager.instance.shopItemListPanel.transform.position;
        RectTransform choisePanelRect = GManager.instance.shopItemListPanel.GetComponent<RectTransform>();
        //ボタンの設置座標を設定するためにpivotを取得
        float pivotY = choisePanelRect.pivot.y;
        List<GameObject> displayItemList = new List<GameObject>();
        string npcTradeMessage = "";
        //買いに来た
        if (shopMode == 0)
        {
            displayItemList = salesItemList;
            npcTradeMessage = "何を購入されますか？";
        }
        //売りに来た
        else if (shopMode == 1)
        {
            displayItemList = GManager.instance.itemList;
            npcTradeMessage = "何を売却されますか？";
        }
        showMessage(npcTradeMessage);
        //アイテムのリストをパネルに展開
        deploymentItemList(displayItemList,parentPosition);
    }

    /**
     * アイテムのリストをパネルに展開する
     */
    void deploymentItemList(List<GameObject> displayItemList, Vector3 parentPosition)
    {
        for (int i = 0; i < displayItemList.Count; i++)
        {
            //メニューボタンの生成
            GameObject choiseButton = Instantiate(itemBtn, new Vector3(parentPosition.x, parentPosition.y + 60 - i * 35, 0f), Quaternion.identity) as GameObject;
            //choisePanelの子オブジェクトにする
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
            //アイテム名をクリックした際のメソッドを設定
            GameObject tempItem = displayItemList[i];
            choiseButton.GetComponent<Button>().onClick.AddListener(() => clickItemName(tempItem));
        }
    }

    /**
     * アイテム名をクリック
     */
    void clickItemName(GameObject argItem)
    {
        Item item = argItem.GetComponent<Item>();
        GManager.instance.itemDescriptionPanel.SetActive(true);
        string description = "アイテム名：" + item.name;
        description += "\n\n";
        description += item.itemDescription;
        GManager.instance.itemDescriptionPanel.transform.Find("Text").GetComponent<Text>().text = description;
        GManager.instance.shopSelectPanel.SetActive(true);
        var parentPosition = GManager.instance.shopSelectPanel.transform.position;
        //売買決定ボタンのリストを作成
        List<ShopChoiseClass> tempTradeMenu = new List<ShopChoiseClass>();
        for (int i=0;i<tradeMenu.Count;i++)
        {
            //"買いに来た"ボタン押下時は買うを"売りに来た"ボタンを押下時は売るをリストに追加
            //やめるボタンは常に追加
            if (tradeMenu[i].funcNumber == shopMode || tradeMenu[i].funcNumber == 2)
            {
                tempTradeMenu.Add(tradeMenu[i]);
            }
        }
        //売買決定ボタンの配置
        for (int i=0;i< tempTradeMenu.Count;i++)
        {
            //メニューボタンの生成
            GameObject choiseButton = Instantiate(tradeBtn, new Vector3(parentPosition.x-15, parentPosition.y + 5 - i * 35, 0f), Quaternion.identity) as GameObject;
            choiseButton.transform.SetParent(GManager.instance.shopSelectPanel.transform, false);
            choiseButton.transform.Find("Text").GetComponent<Text>().text = tempTradeMenu[i].menu;
            int funcIndex = tempTradeMenu[i].funcNumber;
            //選択肢をクリックした際のメソッドを設定
            choiseButton.GetComponent<Button>().onClick.AddListener(() => clickShopTradeMenu(funcIndex, argItem));
        }
    }

    /**
     * 売買決定時の関数
     */
    void clickShopTradeMenu(int index,GameObject item)
    {
        switch (index)
        {
            //購入決定
            case 0:
                decisionBuyItem(item);
                break;
            //売却決定
            case 1:
                decisionSellItem(item);
                break;
            //やめる
            case 2:
                break;
        }
        hideTradePanel();
    }

    /**
     * 売買決定パネルとアイテムの説明パネルを非表示にする
     */
    void hideTradePanel()
    {
        GManager.instance.shopSelectPanel.SetActive(false);
        GManager.instance.itemDescriptionPanel.SetActive(false);
    }

    /**
     * アイテム購入決定
     */
    void decisionBuyItem(GameObject argItem)
    {
        GameObject buyItem = Instantiate(argItem) as GameObject;
        Item item = buyItem.GetComponent<Item>();
        //所持金の判定
        //if (GManager.instance.playerMoney < item.buyPrice)
        if(statusComponentObj?.charWallet.money < item.buyPrice)
        {
            showMessage("所持金が足りません。");
            return;
        }
        //アイテムの所持制限の判定
        if (!GManager.instance.addItem(buyItem))
        {
            showMessage("アイテムがいっぱいです。");
            return;
        }
        //所持金から差し引く
        //GManager.instance.playerMoney -= item.buyPrice;
        statusComponentObj?.charWallet.subMoney(item.buyPrice);
        //購入したアイテムにIDを割り当てる
        //item.assignItemId();
        //GManager.instance.playerMoneyText.text = "所持金：" + GManager.instance.playerMoney + " $";
        GManager.instance.playerMoneyText.text = "所持金：" + statusComponentObj?.charWallet.money + " $";
        hideTradePanel();
        showMessage("お買い上げありがとうございます。");
    }

    /**
     * アイテム売却決定
     */
    void decisionSellItem(GameObject argItem)
    {
        Item item = argItem.GetComponent<Item>();
        for (int i=0;i< GManager.instance.itemList.Count;i++)
        {
            if (GManager.instance.itemList[i].GetComponent<Item>().id == item.id)
            {
                GManager.instance.itemList.RemoveAt(i);
                showMessage("買い取りました。");
                //所持金を増やす
                //GManager.instance.playerMoney += item.sellPrice;
                statusComponentObj.charWallet.addMoney(item.sellPrice);
                //GManager.instance.playerMoneyText.text = "所持金：" + GManager.instance.playerMoney + " $";
                GManager.instance.playerMoneyText.text = "所持金：" + statusComponentObj?.charWallet.money + " $";
                deleteChildButton(GManager.instance.shopItemListPanel.transform);
                deploymentItemList(GManager.instance.itemList,GManager.instance.shopItemListPanel.transform.position);
                hideTradePanel();
                return;
            }
        }
        showMessage("買取に失敗しました。アイテムにIDが割り当てられていません。");
    }

    /**
     * トレード終了
     */
    void leaveShop()
    {
        GManager.instance.shopPanel.SetActive(false);
        GManager.instance.shopItemListPanel.SetActive(false);
        hideTradePanel();
        leaveShopFlag = true;
    }

    /**
     * 引数で指定したパネルの子オブジェクト(ボタン)をすべて削除する
     */
    void deleteChildButton(Transform childButtons)
    {
        //選択肢ボタンをすべて削除
        foreach (Transform child in childButtons)
        {
            if (child.name != "PlayerMoneyText") Destroy(child.gameObject);
        }
    }
}