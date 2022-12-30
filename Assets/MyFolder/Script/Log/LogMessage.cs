using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogMessage : MonoBehaviour
{
    [Header("ログテキスト")] public Text log;
    private string logText;
    private int logBeforeLength = 0;
    
    // Update is called once per frame
    void Update()
    {
        int logArrayNum = LogMessageManager.logMessage.Count;
        if (logBeforeLength != logArrayNum)
        {
            logText = string.Empty;
            for (int i=0;i<logArrayNum;i++)
            {
                logText += LogMessageManager.logMessage[i];
                logText += "\n";
            }
            log.text = logText;
            logBeforeLength = logArrayNum;
        }
    }
}
