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
            npcCanvasInDetection = npcObj.GetComponent<NpcController>().npcCanvas;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        npcCanvasInDetection.SetActive(true);
        isEnterTalkArea = collider.gameObject.tag.Equals("Player");
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        npcCanvasInDetection.SetActive(false);
        isEnterTalkArea = !collider.gameObject.tag.Equals("Player");
    }
}
