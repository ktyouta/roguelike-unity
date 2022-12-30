using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogMessageManager
{
    public static List<string> logMessage = new List<string>();

    public static void wrightLog(string message)
    {
        deleteLog();
        logMessage.Add(message);
    }

    private static void deleteLog()
    {
        if (logMessage.Count > 5)
        {
            logMessage.Clear();
        }
    }
}
