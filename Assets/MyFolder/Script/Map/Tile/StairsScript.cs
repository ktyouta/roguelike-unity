using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
        //次のシーンにデータを引き渡すメソッドをセット
        SceneManager.sceneLoaded += handoverData;
        GManager.instance.nowStairs.text = "";
        //次の階数を表示
        GManager.instance.mapLoadingText.text = ++GManager.instance.hierarchyLevel + " F";
        GManager.instance.mapLoadingImage.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    /**
     * 次のシーンにデータを引き渡す
     */
    private void handoverData(Scene next, LoadSceneMode mode)
    {
        //次のシーンのステータスのインスタンスを取得
        StatusComponentPlayer nextSceneStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusComponentPlayer>();

        var fields = previousSceneStatus.GetType().GetFields();
        // フィールド分のステータスを引き渡す
        foreach (var field in fields)
        {
            var previousSceneStatusField = previousSceneStatus.GetType().GetField(field.Name);
            var nextSceneStatusField = nextSceneStatus.GetType().GetField(field.Name);
            nextSceneStatusField?.SetValue(nextSceneStatus, previousSceneStatusField?.GetValue(previousSceneStatus));
        }

        // イベントから削除
        SceneManager.sceneLoaded -= handoverData;
    }
}
