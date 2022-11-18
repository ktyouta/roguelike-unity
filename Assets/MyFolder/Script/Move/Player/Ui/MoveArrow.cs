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
    [Header("����")] public Direction direction;

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
        //�ړ��p�̃C�x���g�ݒ�
        gameObject.AddComponent<EventTrigger>();
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();

        //�{�^������
        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
        entryPointerDown.eventID = EventTriggerType.PointerDown;

        //�{�^�����痣���
        EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
        entryPointerUp.eventID = EventTriggerType.PointerUp;

        switch (direction)
        {
            //�����
            case Direction.Top:
                entryPointerDown.callback.AddListener((eventDate) => { clickTopArrowBtn(); });
                entryPointerUp.callback.AddListener((eventDate) => { buttonUp(); });
                break;
            //������
            case Direction.Bottom:
                entryPointerDown.callback.AddListener((eventDate) => { clickBottomArrowBtn(); });
                entryPointerUp.callback.AddListener((eventDate) => { buttonUp(); });
                break;
            //�E����
            case Direction.Right:
                entryPointerDown.callback.AddListener((eventDate) => { clickRightArrowBtn(); });
                entryPointerUp.callback.AddListener((eventDate) => { buttonUp(); });
                break;
            //������
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
     * �v���C���[��������Ɉړ�������
     */
    public void clickTopArrowBtn()
    {
        isPushTop = true;
    }

    /**
     * �v���C���[���������Ɉړ�������
     */
    public void clickBottomArrowBtn()
    {
        isPushDown = true;
    }

    /**
     * �v���C���[���E�����Ɉړ�������
     */
    public void clickRightArrowBtn()
    {
        isPushRight = true;
    }

    /**
     * �v���C���[���������Ɉړ�������
     */
    public void clickLeftArrowBtn()
    {
        isPushLeft = true;
    }

    /**
     * �{�^���̃N���b�N���I��
     */
    public void buttonUp()
    {
        isPushTop = false;
        isPushDown = false;
        isPushRight = false;
        isPushLeft = false;
    }
}
