using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NpcBase : TalkBase
{
    [Header("NPC�̃��b�Z�[�W")] public List<string> messages;
    [HideInInspector] public Image npcImage;
    [Header("NPC�̗����G")] public Sprite npcPicture;
    [Header("NPC�̖��O")] public string npcName;


    protected override IEnumerator OnAction()
    {
        GManager.instance.npcNameText.text = npcName + "�F";
        GManager.instance.npcImage.SetActive(true);
        GManager.instance.npcImage.GetComponent<Image>().sprite = npcPicture;
        yield return TalkEvent();
        GManager.instance.npcImage.GetComponent<Image>().sprite = null;
        GManager.instance.npcImage.SetActive(false);
        yield break;
    }

    protected abstract IEnumerator TalkEvent();
    
}
