using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    private bool firstPush = false;

    Button startButton;

    void Start()
    {
        startButton = GameObject.Find("StartButton").GetComponent<Button>();
        //ボタンにシーンの遷移イベントをセット
        startButton.onClick.AddListener(() => { PressStart(); });
    }

    /**
     * シーン遷移イベント
     */
    public void PressStart()
    {
        if (!firstPush)
        {
            SceneManager.LoadScene("Main");
            firstPush = true;
        }
    }
}
