using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TalkBase : MonoBehaviour
{

    // 接触判定
    private IEnumerator coroutine;
    [Header("会話エリア(上)")] public TalkDetection topArea;
    [Header("会話エリア(下)")] public TalkDetection bottomArea;
    [Header("会話エリア(右)")] public TalkDetection rightArea;
    [Header("会話エリア(左)")] public TalkDetection leftArea;
    //プレイヤーの状態管理用
    [HideInInspector]public player playerObj;
    [Header("会話開始用UI")] public GameObject talkAreaObj;
    [HideInInspector] public GameObject npcCanvas;
    protected PanelManager pManager;

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
        pManager = GameObject.Find("GameController").GetComponent<PanelManager>();
    }

    private void Update()
    {
        if ((topArea.isEnterTalkArea || bottomArea.isEnterTalkArea || rightArea.isEnterTalkArea || leftArea.isEnterTalkArea) && coroutine == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D touchObj = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, LayerUtil.npcLayer);
                
                if (touchObj.transform == null)
                {
                    return;
                }
                //クリックしたNPCと一致した場合に会話開始
                if (this.gameObject.name == touchObj.transform.gameObject.name)
                {
                    playerObj.setPlayerState(player.playerState.Talk);
                    coroutine = CreateCoroutine();
                    // コルーチンの起動
                    StartCoroutine(coroutine);
                }
            }
        }
    }

    /**
     * リアクション用コルーチン
     */
    private IEnumerator CreateCoroutine()
    {
        // window起動
        pManager.npcWindowImage.SetActive(true);

        // 抽象メソッド呼び出し 詳細は子クラスで実装
        yield return OnAction();

        // window終了
        pManager.npcMessageText.text = "";
        pManager.npcWindowImage.SetActive(false);

        StopCoroutine(coroutine);
        coroutine = null;
        playerObj.setPlayerState(player.playerState.Normal);
    }

    protected abstract IEnumerator OnAction();

    /**
     * メッセージを表示する
     */
    protected void showMessage(string message)
    {
        pManager.npcMessageText.text = message;
    }
}
