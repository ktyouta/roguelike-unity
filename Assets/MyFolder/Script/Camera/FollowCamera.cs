using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    GameObject playerObj;
    player pl;
    Transform playerTransform;
    Vector2 cameraMaxPos; // �J�����̉E����E�_
    Vector2 cameraMinPos = new Vector2(7.5f, 3.5f); // �J�����̍������E�_
    [Header("�}�b�v�����p�I�u�W�F�N�g")] public BoardManager boardObj;
    private float cameraMaxPointX;
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            pl = playerObj.GetComponent<player>();
            playerTransform = playerObj.transform;
            cameraMaxPointX = boardObj.columns - 8.5f;
            cameraMaxPos = new Vector2(cameraMaxPointX, boardObj.rows - 4.5f);
        }
    }
    void LateUpdate()
    {
        MoveCamera();
    }
    void MoveCamera()
    {
        //�����������Ǐ]
        //transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        transform.position = new Vector3(
            Mathf.Clamp(playerTransform.position.x, cameraMinPos.x, cameraMaxPos.x), // �J�����̍��E�𐧌�
            Mathf.Clamp(playerTransform.position.y, cameraMinPos.y, cameraMaxPos.y), // �J�����̏㉺�𐧌�
            -10f); // �J����z���W��-10f
    }
}