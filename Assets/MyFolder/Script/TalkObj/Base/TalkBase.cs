using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TalkBase : MonoBehaviour
{

    // �ڐG����
    private IEnumerator coroutine;
    [Header("��b�G���A(��)")] public TalkDetection topArea;
    [Header("��b�G���A(��)")] public TalkDetection bottomArea;
    [Header("��b�G���A(�E)")] public TalkDetection rightArea;
    [Header("��b�G���A(��)")] public TalkDetection leftArea;
    //�v���C���[�̏�ԊǗ��p
    [HideInInspector]public player playerObj;
    [Header("��b�J�n�pUI")] public GameObject talkAreaObj;
    [HideInInspector] public GameObject npcCanvas;

    // Start is called before the first frame update
    public virtual void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        npcCanvas = this.gameObject.transform.Find("NpcCanvas").gameObject;
        if (npcCanvas != null)
        {
            npcCanvas.SetActive(false);
        }
        topArea = this.gameObject.transform.Find("TopTalkArea").gameObject.GetComponent<TalkDetection>();
        bottomArea = this.gameObject.transform.Find("BottomTalkArea").gameObject.GetComponent<TalkDetection>();
        rightArea = this.gameObject.transform.Find("RightTalkArea").gameObject.GetComponent<TalkDetection>();
        leftArea = this.gameObject.transform.Find("LeftTalkArea").gameObject.GetComponent<TalkDetection>();
    }

    private void Update()
    {
        if ((topArea.isEnterTalkArea || bottomArea.isEnterTalkArea || rightArea.isEnterTalkArea || leftArea.isEnterTalkArea) && coroutine == null && Input.GetKeyDown(KeyCode.Z))
        {
            playerObj.setPlayerState(player.playerState.Talk);
            coroutine = CreateCoroutine();
            // �R���[�`���̋N��
            StartCoroutine(coroutine);
        }
    }

    /**
     * ���A�N�V�����p�R���[�`��
     */
    private IEnumerator CreateCoroutine()
    {
        Debug.Log("talkstart");
        // window�N��
        GManager.instance.npcWindowImage.SetActive(true);

        // ���ۃ��\�b�h�Ăяo�� �ڍׂ͎q�N���X�Ŏ���
        yield return OnAction();

        // window�I��
        GManager.instance.npcMessageText.text = "";
        GManager.instance.npcWindowImage.SetActive(false);

        StopCoroutine(coroutine);
        coroutine = null;
        playerObj.setPlayerState(player.playerState.Normal);
    }

    protected abstract IEnumerator OnAction();

    /**
     * ���b�Z�[�W��\������
     */
    protected void showMessage(string message)
    {
        GManager.instance.npcMessageText.text = message;
    }
}
