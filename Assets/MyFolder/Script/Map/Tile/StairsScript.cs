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
        GManager.instance.nowStairs.text = "";
        GManager.instance.mapLoadingText.text = ++GManager.instance.level + " F";
        GManager.instance.mapLoadingImage.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
