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
        pManager.npcNameText.text = npcName + "：";
        if(npcPicture != null)
        {
            pManager.npcImage.SetActive(true);
            pManager.npcImage.GetComponent<Image>().sprite = npcPicture;
        }
        yield return TalkEvent();
        pManager.npcImage.GetComponent<Image>().sprite = null;
        pManager.npcImage.SetActive(false);
        yield break;
    }

    protected abstract IEnumerator TalkEvent();
}
