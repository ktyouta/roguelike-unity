using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    GameObject playerObj;
    player pl;
    Transform playerTransform;
    Vector2 cameraMaxPos; // カメラの右上限界点
    Vector2 cameraMinPos = new Vector2(5.6f, 2.5f); // カメラの左下限界点
    [Header("マップ生成用オブジェクト")] public BoardManager boardObj;
    private float cameraMaxPointX;
    private Camera camObj;
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        camObj = GetComponent<Camera>();
        if (playerObj != null)
        {
            pl = playerObj.GetComponent<player>();
            playerTransform = playerObj.transform;
            //横スクロールマップモード
            if (boardObj.createMapMode == 1)
            {
                cameraMaxPointX = boardObj.columns - 8.5f;
                cameraMaxPos = new Vector2(cameraMaxPointX, boardObj.rows - 4.5f);
            }
            //不思議のダンジョン系マップモード
            else
            {
                //ボス戦マップ
                if (boardObj.isBossMode)
                {
                    cameraMaxPointX = Define.MYSTERYBOSSMAP_WHITH - 5.6f;
                    cameraMaxPos = new Vector2(cameraMaxPointX, Define.MYSTERYBOSSMAP_HEIGHT - 2.5f);
                }
                //通常マップ
                else
                {
                    cameraMaxPointX = Define.MYSTERYMAP_WHITH - 5.6f;
                    cameraMaxPos = new Vector2(cameraMaxPointX, Define.MYSTERYMAP_HEIGHT - 2.5f);
                }
                camObj.orthographicSize = camObj.orthographicSize - 1.0f;

            }
        }
    }
    void LateUpdate()
    {
        MoveCamera();
    }
    void MoveCamera()
    {
        if (playerTransform == null)
        {
            return;
        }
        //横方向だけ追従
        //transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        transform.position = new Vector3(
            Mathf.Clamp(playerTransform.position.x, cameraMinPos.x, cameraMaxPos.x), // カメラの左右を制限
            Mathf.Clamp(playerTransform.position.y, cameraMinPos.y, cameraMaxPos.y), // カメラの上下を制限
            -10f); // カメラz座標は-10f
    }
}