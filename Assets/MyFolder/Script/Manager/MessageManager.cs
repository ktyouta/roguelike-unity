using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageManager
{
    public class MessageJsonArrayClass
    {
        public MessageInfoClass[] messageInfos;
    }

    [System.Serializable]
    public class MessageInfoClass
    {
        public string id;
        public string message;
    }
    // メッセージを格納
    [HideInInspector] public static MessageJsonArrayClass messageJsonData;

    static MessageManager()
    {
        string loadjson = Resources.Load<TextAsset>("json/Message/RoguelikeMessage").ToString();
        messageJsonData = new MessageJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadjson, messageJsonData);
    }

    /**
     * メッセージを作成
     */
    public static string createMessage(string id, params string[] args)
    {
        foreach (var messageInfo in messageJsonData.messageInfos)
        {
            // メッセージが見つかった場合
            if (messageInfo.id == id)
            {
                return string.Format(messageInfo.message, args);
            }
        }
        return "";
    }
}
