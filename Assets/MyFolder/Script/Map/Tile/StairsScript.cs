using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StairsScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            return;
        }
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
        GManager.instance.nowStairs.text = "";
        GManager.instance.mapLoadingText.text = ++GManager.instance.hierarchyLevel + " F";
        GManager.instance.mapLoadingImage.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
