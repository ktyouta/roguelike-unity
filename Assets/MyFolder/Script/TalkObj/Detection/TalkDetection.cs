using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkDetection : MonoBehaviour
{
    [HideInInspector]public bool isEnterTalkArea = false;
    public GameObject npcCanvasInDetection;
    public GameObject npcObj;
    // Start is called before the first frame update
    void Start()
    {
        //NPCのゲームオブジェクトを取得
        npcObj = gameObject.transform.parent.gameObject;
        if (npcObj != null)
        {
            //NPCゲームオブジェクトの子オブジェクト(NpcCanvas)を取得
            npcCanvasInDetection = npcObj.GetComponent<TalkBase>().npcCanvas;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //NPCと位置が被る場合は非表示にする
        if (collider.tag == "Npc")
        {
            this.gameObject.SetActive(false);
        }
        if (collider.tag == "Player")
        {
            npcCanvasInDetection.SetActive(true);
            isEnterTalkArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            npcCanvasInDetection.SetActive(false);
            isEnterTalkArea = false;
        }
    }
}
