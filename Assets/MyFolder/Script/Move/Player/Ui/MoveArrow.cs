using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveArrow : MonoBehaviour
{
    private player playerObj;
    private bool isPushTop = false;
    private bool isPushDown = false;
    private bool isPushRight = false;
    private bool isPushLeft = false;
    [Header("方向")] public Direction direction;

    public enum Direction
    {
        Top,
        Bottom,
        Right,
        Left
    }

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        //移動用のイベント設定
        gameObject.AddComponent<EventTrigger>();
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();

        //ボタン押下
        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
        entryPointerDown.eventID = EventTriggerType.PointerDown;

        //ボタンから離れる
        EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
        entryPointerUp.eventID = EventTriggerType.PointerUp;

        switch (direction)
        {
            //上方向
            case Direction.Top:
                entryPointerDown.callback.AddListener((eventDate) => { clickTopArrowBtn(); });
                entryPointerUp.callback.AddListener((eventDate) => { buttonUp(); });
                break;
            //下方向
            case Direction.Bottom:
                entryPointerDown.callback.AddListener((eventDate) => { clickBottomArrowBtn(); });
                entryPointerUp.callback.AddListener((eventDate) => { buttonUp(); });
                break;
            //右方向
            case Direction.Right:
                entryPointerDown.callback.AddListener((eventDate) => { clickRightArrowBtn(); });
                entryPointerUp.callback.AddListener((eventDate) => { buttonUp(); });
                break;
            //左方向
            case Direction.Left:
                entryPointerDown.callback.AddListener((eventDate) => { clickLeftArrowBtn(); });
                entryPointerUp.callback.AddListener((eventDate) => { buttonUp(); });
                break;
        }
        trigger.triggers.Add(entryPointerDown);
        trigger.triggers.Add(entryPointerUp);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPushTop)
        {
            playerObj.playerMove(0, 1, false);
        }
        else if (isPushDown)
        {
            playerObj.playerMove(0, -1, false);
        }
        else if (isPushRight)
        {
            playerObj.playerMove(1, 0, false);
        }
        else if (isPushLeft)
        {
            playerObj.playerMove(-1, 0, false);
        }
    }

    /**
     * プレイヤーを上方向に移動させる
     */
    public void clickTopArrowBtn()
    {
        isPushTop = true;
    }

    /**
     * プレイヤーを下方向に移動させる
     */
    public void clickBottomArrowBtn()
    {
        isPushDown = true;
    }

    /**
     * プレイヤーを右方向に移動させる
     */
    public void clickRightArrowBtn()
    {
        isPushRight = true;
    }

    /**
     * プレイヤーを左方向に移動させる
     */
    public void clickLeftArrowBtn()
    {
        isPushLeft = true;
    }

    /**
     * ボタンのクリックを終了
     */
    public void buttonUp()
    {
        isPushTop = false;
        isPushDown = false;
        isPushRight = false;
        isPushLeft = false;
    }
}
