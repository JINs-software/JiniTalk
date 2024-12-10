
using System;
using UnityEngine;

public class ChatUI : MonoBehaviour
{
    NetworkHandler ChatSession = new NetworkHandler();
    const string serverIP = "127.0.0.1";
    const UInt16 serverPort = 12120;

    GameObject ChatServerList = null;
    GameObject ChatRoomList = null;
    GameObject ChatRoom = null;

    private void Start()
    {
        ChatServerList = Util.FindChild(gameObject, "ChatServerList", true);
        ChatRoomList = Util.FindChild(gameObject, "ChatRoomList", true);
        ChatRoom = Util.FindChild(gameObject, "ChatRoom", true);

        if(ChatSession.Connect(serverIP, serverPort))
        {
            ChatRoomList.SetActive(true);
            
        }
    }
}