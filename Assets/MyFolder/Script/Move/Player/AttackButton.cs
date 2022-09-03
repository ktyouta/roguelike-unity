using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    private player playerObj;
    private Button attackButton;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<player>();
        attackButton = GetComponent<Button>();
        attackButton.onClick.AddListener(attackAction);
    }

    /**
     * çUåÇÉ{É^Éìâüâ∫
     */
    private void attackAction()
    {
        playerObj.Attack();
    }
}
