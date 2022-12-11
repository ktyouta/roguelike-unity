using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
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
    [HideInInspector] public MessageJsonArrayClass messageJsonData;

    // Start is called before the first frame update
    void Awake()
    {
        string loadjson = Resources.Load<TextAsset>("json/Message/RoguelikeMessage").ToString();
        messageJsonData = new MessageJsonArrayClass();
        JsonUtility.FromJsonOverwrite(loadjson, messageJsonData);
    }

    /**
     * ���b�Z�[�W���쐬
     */
    public string createMessage(string id, params string[] args)
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
