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
        //NPC�̃Q�[���I�u�W�F�N�g���擾
        npcObj = gameObject.transform.parent.gameObject;
        if (npcObj != null)
        {
            //NPC�Q�[���I�u�W�F�N�g�̎q�I�u�W�F�N�g(NpcCanvas)���擾
            npcCanvasInDetection = npcObj.GetComponent<TalkBase>().npcCanvas;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //NPC�ƈʒu�����ꍇ�͔�\���ɂ���
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
