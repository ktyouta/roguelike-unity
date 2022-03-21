using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NpcBase : TalkBase
{
    [Header("NPCのメッセージ")] public List<string> messages;
    [HideInInspector] public Image npcImage;
    [Header("NPCの立ち絵")] public Sprite npcPicture;
    [Header("NPCの名前")] public string npcName;


    protected override IEnumerator OnAction()
    {
        GManager.instance.npcNameText.text = npcName + "：";
        GManager.instance.npcImage.SetActive(true);
        GManager.instance.npcImage.GetComponent<Image>().sprite = npcPicture;
        yield return TalkEvent();
        GManager.instance.npcImage.GetComponent<Image>().sprite = null;
        GManager.instance.npcImage.SetActive(false);
        yield break;
    }

    protected abstract IEnumerator TalkEvent();
    
}
