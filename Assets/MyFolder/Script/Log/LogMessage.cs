using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogMessage : MonoBehaviour
{
    [Header("ログテキスト")] public Text log;
    private string logText;
    private int logBeforeLength = 0;
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GManager.instance.logMessage);
        int logArrayNum = GManager.instance.logMessage.Count;
        if (logBeforeLength != logArrayNum)
        {
            logText = "";
            for (int i=0;i<logArrayNum;i++)
            {
                logText += GManager.instance.logMessage[i];
                logText += "\n";
            }
            log.text = logText;
            logBeforeLength = logArrayNum;
        }
    }
}
