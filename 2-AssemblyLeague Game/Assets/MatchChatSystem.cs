using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MatchChatSystem : MonoBehaviour
{
    public List<ChatMessage> CurrentChatMessages;

    System.DateTime nextChatMessageUpdateDate;
    // Use this for initialization
    void Start()
    {
        CurrentChatMessages = new List<ChatMessage>();
        nextChatMessageUpdateDate = System.DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextChatMessageUpdateDate <= System.DateTime.Now)
        {
            nextChatMessageUpdateDate = System.DateTime.Now.AddSeconds(5);
            RefreshMessageList();
        }
    }

    public void SendMessage(string messageText)
    {
        ChatMessage aMessge = new ChatMessage();
        aMessge.SendDateUTC = System.DateTime.UtcNow;
        aMessge.MessageText = messageText;
    }

    public void RefreshMessageList()
    {

    }

    public class ChatMessage
    {
        public string ID;
        public System.DateTime SendDateUTC;
        public string OwnerID;
        public string MessageText;

    }
}
