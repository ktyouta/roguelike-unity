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
    // ���b�Z�[�W���i�[
    [HideInInspector] public static MessageJsonArrayClass messageJsonData;

    static MessageManager()
    {
        string loadjson = Resources.Load<TextAsset>("json/Message/RoguelikeMessage").ToString();
        messageJsonData = new MessageJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadjson, messageJsonData);
    }

    /**
     * ���b�Z�[�W���쐬
     */
    public static string createMessage(string id, params string[] args)
    {
        foreach (var messageInfo in messageJsonData.messageInfos)
        {
            // ���b�Z�[�W�����������ꍇ
            if (messageInfo.id == id)
            {
                return string.Format(messageInfo.message, args);
            }
        }
        return "";
    }
}
