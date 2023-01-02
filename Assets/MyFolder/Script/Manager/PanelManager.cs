using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{

    //�p�l������n
    [Header("�C���x���g���[�ɓW�J�����A�C�e���{�^��")] public GameObject itemBtn;
    [Header("�C���x���g���[�̎q�I�u�W�F�N�g(�e�L�X�g)")] public Text itemPanelText;
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

    //�v���C���[�̃X�e�[�^�X�n
    [HideInInspector] public player playerObj;
    [HideInInspector] public StatusComponentPlayer statusComponentPlayer;

    // Update is called once per frame
    void Update()
    {
        // �Z�b�g�A�b�v��
        if (GManager.instance.doingSetup)
        {
            return;
        }
        if (playerObj.plState != player.playerState.Normal)
        {
            return;
        }
        //�R�}���h�p�l���J��
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
     * ������@���J��
     */
    public void openManual()
    {
        //�A�C�e���p�p�l�����\���ɂ���
        itemPanel.SetActive(false);
        itemUsePanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        //�X�e�[�^�X�p�l�������L���Ďg��
        statusText.SetActive(true);
        //�}�j���A����\��
        string manual;
        manual = "�ړ� : �����L�[";
        manual += "\n";
        manual += "���j���[���J�� : �X�y�[�X�L�[";
        manual += "\n";
        manual += "�U�� : �V�t�g�L�[";
        manual += "\n";
        manual += "NPC�Ƃ̉�b : ���N���b�N";
        manual += "\n";
        playerStatusPanel.text = manual;
    }

    /**
     * �X�e�[�^�X�p�l�����J��
     */
    public void openStatus()
    {
        if (statusComponentPlayer == null)
        {
            statusComponentPlayer = playerObj.GetComponent<StatusComponentPlayer>();
        }
        //�A�C�e���p�p�l�����\���ɂ���
        itemPanel.SetActive(false);
        itemUsePanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);
        statusText.SetActive(true);
        //�v���C���[�̃X�e�[�^�X��\��
        string status;
        status = "�v���C���[�� : " + statusComponentPlayer.charName.name;
        status += "\n";
        status += "���x�� : " + statusComponentPlayer.charExperience.level;
        status += "\n";
        status += "HP : " + statusComponentPlayer.charHp.hp;
        status += "\n";
        status += "�U���� : " + statusComponentPlayer.charAttack.totalAttack;
        status += "\n";
        status += "�h��� : " + statusComponentPlayer.charDefence.totalDefence;
        status += "\n";
        status += "�����x : " + statusComponentPlayer.charFood.foodPoint;
        status += "\n";
        status += "������ : " + statusComponentPlayer.charWallet.money;
        status += "\n";
        status += "���͓x : " + statusComponentPlayer.charCarm.charmPoint;
        status += "\n";
        status += "���� : " + statusComponentPlayer.weaponName;
        status += "\n";
        status += "�� : " + statusComponentPlayer.shieldName;
        playerStatusPanel.text = status;
    }

    /**
     * �A�C�e���{�^������
     */
    public void openItem()
    {
        //�{�^�������ׂč폜
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
     * �C���x���g���[���̃A�C�e����W�J����
     */
    void deploymentMyInventory(Vector3 parentPosition)
    {
        //�A�C�e�����������Ă��Ȃ�
        if (ItemManager.itemList.Count < 1 && itemPanelText != null)
        {
            itemPanelText.enabled = true;
            itemPanelText.text = "�A�C�e�����������Ă��܂���";
            return;
        }
        for (var i = 0; i < ItemManager.itemList.Count; i++)
        {
            //���j���[�{�^���̐���
            GameObject listButton = Instantiate(itemBtn) as GameObject;
            listButton.transform.SetParent(itemPanelText.transform, false);
            listButton.transform.Find("Text").GetComponent<Text>().text = ItemManager.itemList[i].GetComponent<Item>().name;
            int index = i;
            listButton.GetComponent<ItemNameButton>().itemNameButtonId = index;
            //�A�C�e�����N���b�N�����Ƃ��̊֐���ݒ�
            listButton.GetComponent<Button>().onClick.AddListener(() => clickItemButton(ItemManager.itemList[index], listButton, index));
        }
    }

    /**
     * �R�}���h���j���[�W�J
     */
    IEnumerator deploymentMyCommandPanel()
    {
        commandPanel.SetActive(true);
        yield return null;
        //����{�^������������邩�A�X�y�[�X�L�[���������ꂽ�ꍇ�Ƀ��j���[�����
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
     * �R�}���h�p�l�������
     */
    public void closeMenu()
    {
        isCloseCommand = true;
    }

    /**
     * �����Ŏw�肵���p�l���̎q�I�u�W�F�N�g(�{�^��)�����ׂč폜����
     */
    public void deleteChildButton(Transform childButtons)
    {
        //�I�����{�^�������ׂč폜
        foreach (Transform child in childButtons)
        {
            Destroy(child.gameObject);
        }
    }

    /**
     * itemPanelText(�C���x���g���[)�ɓW�J���ꂽ�A�C�e�������N���b�N
     */
    public void clickItemButton(GameObject argItem, GameObject listButton, int index)
    {
        GameObject[] itemBtns = GameObject.FindGameObjectsWithTag("ItemButton");
        Item item = argItem.GetComponent<Item>();
        for (int i = 0; i < itemBtns.Length; i++)
        {
            //�I�������A�C�e����������
            if (itemBtns[i].GetComponent<ItemNameButton>().itemNameButtonId == index)
            {
                itemBtns[i].GetComponent<Image>().color = Color.cyan;
            }
            else
            {
                itemBtns[i].GetComponent<Image>().color = new Color32(140, 168, 166, 255);
            }
        }
        //�A�C�e�������p�p�l�����\���ɂ���
        itemDescriptionPanel.SetActive(true);
        string description = "�A�C�e�����F" + item.name;
        description += "\n\n";
        description += item.itemDescription;
        itemDescriptionPanel.transform.Find("Text").GetComponent<Text>().text = description;
        itemUsePanel.SetActive(true);
        //�C�x���g���X�i�[���폜
        itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.RemoveAllListeners();
        itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.RemoveAllListeners();
        itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.RemoveAllListeners();
        //"�u��"�A"������"�{�^���������ɂ���
        itemUsePanel.transform.Find("PutButton").GetComponent<Button>().interactable = true;
        itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().interactable = true;
        //����n�A�C�e��
        if (item.type.ToString() == "Consume")
        {
            itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "����";
        }
        //�����n�A�C�e��
        else if (item.type.ToString() == "Equipment")
        {
            //�A�C�e���𑕔����Ă���ꍇ��"�u��"�A"������"�{�^����񊈐��ɂ���
            if (((EquipmentBase)item).isEquip)
            {
                itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "�͂���";
                itemUsePanel.transform.Find("PutButton").GetComponent<Button>().interactable = false;
                itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().interactable = false;
            }
            else
            {
                itemUsePanel.transform.Find("UseButton").transform.Find("Text").GetComponent<Text>().text = "������";
            }
        }
        //�A�C�e���g�p�C�x���g
        itemUsePanel.transform.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => { adduUseItemFunc(item); });
        //�A�C�e�������ɒu���C�x���g
        itemUsePanel.transform.Find("PutButton").GetComponent<Button>().onClick.AddListener(() => { addPutItemFunc(argItem); });
        //�A�C�e���𓊂���C�x���g
        itemUsePanel.transform.Find("ThrowButton").GetComponent<Button>().onClick.AddListener(() => { addThrowItem(argItem); });
    }

    /**
     * �A�C�e�����g�p
     */
    public void adduUseItemFunc(Item item)
    {
        playerObj.useItem(item);
        isCloseCommand = true;
    }

    /**
     * �����ɃA�C�e����u��
     */
    public void addPutItemFunc(GameObject item)
    {
        playerObj.putItemFloor(item);
        isCloseCommand = true;
    }

    /**
     * �A�C�e���𓊂���
     */
    public void addThrowItem(GameObject item)
    {
        playerObj.throwItem(item);
        isCloseCommand = true;
    }
}
