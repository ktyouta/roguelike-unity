using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static player;

public class StairsScript : MonoBehaviour
{
    StatusComponentPlayer previousSceneStatus;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        //���̃V�[���p�ɃX�e�[�^�X�������n��
        previousSceneStatus = other.GetComponent<StatusComponentPlayer>();
        Invoke("Restart", 0.3f);
    }

    /**
     * �V�[�������[�h����
     */
    private void Restart()
    {
        //�Q�[���I�u�W�F�N�g�����̃V�[���Ɉ����p��
        //�A�C�e��
        for (int i=0;i<GManager.instance.itemList.Count;i++)
        {
            DontDestroyOnLoad(GManager.instance.itemList[i]);
        }
        //NPC
        for (int i = 0; i < GManager.instance.fellows.Count; i++)
        {
            DontDestroyOnLoad(GManager.instance.fellows[i]);
        }
        
        SceneManager.sceneLoaded += handoverData;
        GManager.instance.nowStairs.text = "";
        GManager.instance.mapLoadingText.text = ++GManager.instance.hierarchyLevel + " F";
        GManager.instance.mapLoadingImage.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    /**
     * ���̃V�[���Ƀf�[�^�������n��
     */
    private void handoverData(Scene next, LoadSceneMode mode)
    {
        player nextPlayerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        //�X�e�[�^�X�������n��
        nextPlayerObj.statusObj = previousSceneStatus;

        // �C�x���g����폜
        SceneManager.sceneLoaded -= handoverData;
    }
}
