using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GManager.instance.enemyAppearanceFlg && GManager.instance.enemyAppearanceEventTurnNum >= 3)
        {

        }
    }

    private void appearanceEnemy()
    {
        Vector2 nowPlayerPosition = GManager.instance.playerObj.transform.position;

    }
}
