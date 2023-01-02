using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{

    //パネル制御系
    [Header("インベントリーに展開されるアイテムボタン")] public GameObject itemBtn;
    [Header("インベントリーの子オブジェクト(テキスト)")] public Text itemPanelText;
    public GameObject levelText;
    public GameObject levelImage;
    public GameObject commandPanel;
    public GameObject statusText;
    public GameObject itemPanel;
    public GameObject itemUsePanel;
    public GameObject npcWindowImage;
    public GameObject npcImage;
    public GameObject choisePanel;
    public GameObject shopPanel;
    public GameObject shopItemListPanel;
    public GameObject shopSelectPanel;
    public GameObject itemDescriptionPanel;
    public GameObject grayImage;
    public Text playerStatusPanel;
    public Text npcMessageText;
    public Text npcNameText;
    public Text playerMoneyText;
    public Text loadingText;
    public Text mapLoadingText;
    private Button statusButton;
    private Button itemButton;
    private Button closeButton;
    private Button manualButton;
    [HideInInspector] public bool isCloseCommand = true;
    private IEnumerator coroutine;

    //プレイヤーのステータス系
    [HideInInspector] public player playerObj;
    [HideInInspector] public StatusComponentPlayer statusComponentPlayer;

    // Update is called once per frame
    void Update()
    {
        // セットアップ中
        if (GManager.instance.doingSetup)
        {
            return;
        }
        if (playerObj.plState != player.playerState.Normal)
        {
            return;
        }
        //コマンドパネル開閉
        if (!isCloseCommand || Input.GetKeyDown("space"))
        {
            playerObj.setPlayerState(player.playerState.Command);
            isCloseCommand = false;
            coroutine = null;
            coroutine = deploymentMyCommandPanel();
            StartCoroutine(coroutine);
        }
    }

    public void Init()
    {
        levelText = GameObject.Find("LevelText");
        commandPanel = GameObject.Find("CommandPanel");
        statusText = GameObject.Find("StatusPanel");
        itemPanel = GameObject.Find("ItemPanel");
        statusButton = GameObject.Find("StatusButton").GetComponent<Button>();
        statusButton.onClick.AddListener(() => openStatus());
        itemButton = GameObject.Find("ItemButton").GetComponent<Button>();
        itemButton.onClick.AddListener(() => openItem());
        closeButton = GameObject.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(() => closeMenu());
        itemUsePanel = GameObject.Find("ItemUseList");
        itemDescriptionPanel = GameObject.Find("ItemDescriptionPanel");
        npcWindowImage = GameObject.FindWithTag("NpcTalkPanel");
        npcImage = GameObject.FindWithTag("NpcImage");
        choisePanel = GameObject.FindWithTag("ChioseMessagePanel");
        shopPanel = GameObject.FindWithTag("ShopPanelTag");
        shopItemListPanel = GameObject.FindWithTag("ShopItemPanelTag");
        shopSelectPanel = GameObject.FindWithTag("ShopSelectPanelTag");
        grayImage = GameObject.FindWithTag("GrayImageTag");
        manualButton = GameObject.Find("ManualButton").GetComponent<Button>();
        manualButton.onClick.AddListener(() => openManual());
        if (commandPanel != null)
        {
            commandPanel.SetActive(false);
        }
        if (statusText != null)
        {
            statusText.SetActive(false);
            playerStatusPanel = statusText.transform.Find("PlayerStatusText").GetComponent<Text>();
        }
        if (itemPanel != null)
        {
            itemPanel.SetActive(false);
            itemPanelText = itemPanel.transform.Find("ItemPanelText").GetComponent<Text>();
        }
        if (itemUsePanel != null)
        {
            itemUsePanel.SetActive(false);
        }
        if (itemDescriptionPanel != null)
        {
            itemDescriptionPanel.SetActive(false);
        }
        if (npcWindowImage != null)
        {
            npcMessageText = npcWindowImage.transform.Find("TalkText").gameObject.GetComponent<Text>();
            npcNameText = npcWindowImage.transform.Find("NpcNameText").gameObject.GetComponent<Text>();
            npcWindowImage.SetActive(false);
        }
        if (npcImage != null)
        {
            npcImage.SetActive(false);
        }
        if (choisePanel != null)
        {
            choisePanel.SetActive(false);
        }
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
        if (shopItemListPanel != null)
        {
            shopItemListPanel.SetActive(false);
            playerMoneyText = shopItemListPanel.transform.Find("PlayerMoneyText").gameObject.GetComponent<Text>();
        }
        if (shopSelectPanel != null)
        {
            shopSelectPanel.SetActive(false);
        }
        if (grayImage != null)
        {
            grayImage.SetActive(false);
            loadingText = grayImage.transform.Find("LoadingText").gameObject.GetComponent<Text>();
        }
        if (levelText != null)
        {
            levelText.SetActive(false);
        }

        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        statusComponentPlayer = playerObj.GetComponent<StatusComponentPlayer>();
    }

    /**
     * 操作方法を開く
     */
    public void openManual()
    {
        //アイテム用パネルを非表示にする
        itemPanel.SetActive(false);
        itemUsePanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        //ステータスパネルを共有して使う
        statusText.SetActive(true);
        //マニュアルを表示
        string manual;
        manual = "移動 : 方向キー";
        manual += "\n";
        manual += "メニューを開く : スペースキー";
        manual += "\n";
        manual += "攻撃 : シフトキー";
        manual += "\n";
        manual += "NPCとの会話 : 左クリック";
        manual += "\n";
        playerStatusPanel.text = manual;
    }

    /**
     * ステータスパネルを開く
     */
    public void openStatus()
    {
        if (statusComponentPlayer == null)
        {
            statusComponentPlayer = playerObj.GetComponent<StatusComponentPlayer>();
        }
        //アイテム用パネルを非表示にする
        itemPanel.SetActive(false);
        itemUsePanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        statusText.SetActive(true);
        //プレイヤーのステータスを表示
        string status;
        status = "プレイヤー名 : " + statusComponentPlayer.charName.name;
        status += "\n";
        status += "レベル : " + statusComponentPlayer.charExperience.level;
        status += "\n";
        status += "HP : " + statusComponentPlayer.charHp.hp;
        status += "\n";
        status += "攻撃力 : " + statusComponentPlayer.charAttack.totalAttack;
        status += "\n";
        status += "防御力 : " + statusComponentPlayer.charDefence.totalDefence;
        status += "\n";
        status += "満腹度 : " + statusComponentPlayer.charFood.foodPoint;
        status += "\n";
        status += "所持金 : " + statusComponentPlayer.charWallet.money;
        status += "\n";
        status += "魅力度 : " + statusComponentPlayer.charCarm.charmPoint;
        status += "\n";
        status += "武器 : " + statusComponentPlayer.weaponName;
        status += "\n";
        status += "盾 : " + statusComponentPlayer.shieldName;
        playerStatusPanel.text = status;
    }

    /**
     * アイテムボタン押下
     */
    public void openItem()
    {
        //ボタンをすべて削除
        deleteChildButton(itemPanelText.transform);
        statusText.SetActive(false);
        itemPanel.SetActive(true);
        //GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
        //for (int i = 0; i < itemBtns.Length; i++)
        //{
        //    itemBtns[i].GetComponent<Image>().color = new Color32(140, 168, 166, 255);
        //}
        Vector3 parentPosition = itemPanel.transform.position;
        deploymentMyInventory(parentPosition);
    }

    /**
     * インベントリー内のアイテムを展開する
     */
    void deploymentMyInventory(Vector3 parentPosition)
    {
        //アイテムを所持していない
        if (ItemManager.itemList.Count < 1 && itemPanelText != null)
        {
            itemPanelText.enabled = true;
            itemPanelText.text = "アイテムを所持していません";
            return;
        }
        for (var i = 0; i < ItemManager.itemList.Count; i++)
        {
            //メニューボタンの生成
            GameObject listButton = Instantiate(itemBtn) as GameObject;
            listButton.transform.SetParent(itemPanelText.transform, false);
            listButton.transform.Find("Text").GetComponent<Text>().text = ItemManager.itemList[i].GetComponent<Item>().name;
            int index = i;
            listButton.GetComponent<ItemNameButton>().itemNameButtonId = index;
            //アイテムをクリックしたときの関数を設定
            listButton.GetComponent<Button>().onClick.AddListener(() => clickItemButton(ItemManager.itemList[index], listButton, index));
        }
    }

    /**
     * コマンドメニュー展開
     */
    IEnumerator deploymentMyCommandPanel()
    {
        commandPanel.SetActive(true);
        yield return null;
        //閉じるボタンが押下されるか、スペースキーが押下された場合にメニューを閉じる
        yield return new WaitUntil(() => isCloseCommand || Input.GetKeyDown("space"));
        isCloseCommand = true;
        StopCoroutine(coroutine);
        coroutine = null;
        commandPanel.SetActive(false);
        statusText.SetActive(false);
        itemPanel.SetActive(false);
        itemUsePanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        itemPanelText.enabled = false;
        playerObj.setPlayerState(player.playerState.Normal);
    }

    /**
     * コマンドパネルを閉じる
     */
    public void closeMenu()
    {
        isCloseCommand = true;
    }

    /**
     * 引数で指定したパネルの子オブジェクト(ボタン)をすべて削除する
     */
    public void deleteChildButton(Transform childButtons)
    {
        //選択肢ボタンをすべて削除
        foreach (Transform child in childButtons)
        {
            Destroy(child.gameObject);
        }
    }

    /**
     * itemPanelText(インベントリー)に展開されたアイテム名をクリック
     */
    public void clickItemButton(GameObject argItem, GameObject listButton, int index)
    {
        GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
        Item item = argItem.GetComponent<Item>();
        for (int i = 0; i < itemBtns.Length; i++)
        {
            //選択したアイテム名を強調
            if (itemBtns[i].GetComponent<ItemNameButton>().itemNameButtonId == index)
            {
                itemBtns[i].GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                itemBtns[i].GetComponent<Image>().color = new Color32(140, 168, 166, 255);
            }
        }
        //アイテム説明用パネルを非表示にする
        itemDescriptionPanel.SetActive(true);
        string description = "アイテム名：" + item.name;
        description += "\n\n";
        description += item.itemDescription;
        itemDescriptionPanel.transform.Find("Text").GetComponent<Text>().text = description;
        itemUsePanel.SetActive(true);
        //イベントリスナーを削除
        itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.RemoveAllListeners();
        itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.RemoveAllListeners();
        itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.RemoveAllListeners();
        //"置く"、"投げる"ボタンを活性にする
        itemUsePanel.transform.Find("PutButton").GetComponent<Button>().interactable = true;
        itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().interactable = true;
        //消費系アイテム
        if (item.type.ToString() == "Consume")
        {
            itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "つかう";
        }
        //装備系アイテム
        else if (item.type.ToString() == "Equipment")
        {
            //アイテムを装備している場合は"置く"、"投げる"ボタンを非活性にする
            if (((EquipmentBase)item).isEquip)
            {
                itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "はずす";
                itemUsePanel.transform.Find("PutButton").GetComponent<Button>().interactable = false;
                itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().interactable = false;
            }
            else
            {
                itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "そうび";
            }
        }
        //アイテム使用イベント
        itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => { adduUseItemFunc(item); });
        //アイテムを床に置くイベント
        itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.AddListener(() => { addPutItemFunc(argItem); });
        //アイテムを投げるイベント
        itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.AddListener(() => { addThrowItem(argItem); });
    }

    /**
     * アイテムを使用
     */
    public void adduUseItemFunc(Item item)
    {
        playerObj.useItem(item);
        isCloseCommand = true;
    }

    /**
     * 足元にアイテムを置く
     */
    public void addPutItemFunc(GameObject item)
    {
        playerObj.putItemFloor(item);
        isCloseCommand = true;
    }

    /**
     * アイテムを投げる
     */
    public void addThrowItem(GameObject item)
    {
        playerObj.throwItem(item);
        isCloseCommand = true;
    }
}
