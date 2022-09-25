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
        //次のシーン用にステータスを引き渡す
        previousSceneStatus = other.GetComponent<StatusComponentPlayer>();
        Invoke("Restart", 0.3f);
    }

    /**
     * シーンをロードする
     */
    private void Restart()
    {
        //ゲームオブジェクトを次のシーンに引き継ぐ
        //アイテム
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
     * 次のシーンにデータを引き渡す
     */
    private void handoverData(Scene next, LoadSceneMode mode)
    {
        player nextPlayerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        //ステータスを引き渡す
        nextPlayerObj.statusObj = previousSceneStatus;

        // イベントから削除
        SceneManager.sceneLoaded -= handoverData;
    }
}
