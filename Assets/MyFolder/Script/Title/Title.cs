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
        //�{�^���ɃV�[���̑J�ڃC�x���g���Z�b�g
        startButton.onClick.AddListener(() => { PressStart(); });
    }

    /**
     * �V�[���J�ڃC�x���g
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
