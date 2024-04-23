using System.Collections;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class FB_Chet : MonoBehaviour
{
    public SR_ChatMessage SR_chatMessage;
    public TMP_InputField Txt_Chat;
    public Button btn_Send;


    DatabaseReference reference;
    List<DatabaseReference> chatRefs = new List<DatabaseReference>();
    
    bool chattingOn = false;
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Chetting").ChildAdded += ChatUpdate;
        reference.Child("Chetting").ChildRemoved += ChatRemoved;
        btn_Send.onClick.AddListener(() => SendChatting());
    }

    public void Update()
    {
        if (chattingOn)
        {
            chattingOn = false;
            StartCoroutine(RemoveChat());
        }
    }
    public void SendChatting()
    {
        string nickName = SR_GameManager.instance.nickName;
        string message = Txt_Chat.text;
        Txt_Chat.text = "";

        Chat newChat = new Chat(nickName, message);
        string json = JsonUtility.ToJson(newChat);
        var chatRef = reference.Child("Chetting").Push();
        chatRefs.Add(chatRef);
        chatRef.SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("message send success!");
                chattingOn = true;
            }
        });
    }

    private void ChatUpdate(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        Chat newChat = JsonUtility.FromJson<Chat>(args.Snapshot.GetRawJsonValue());


        Debug.Log($"Received new chat from {newChat.nickName}: {newChat.message}");
        string nickName = $"{newChat.nickName}";
        string message = $"{newChat.message}";

        SR_chatMessage.SpawnChat(nickName, message);
    }

    private void ChatRemoved(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
    }

    IEnumerator RemoveChat()
    {
        yield return new WaitForSeconds(0.1f);
        reference.Child("Chetting").Child(chatRefs[0].Key).RemoveValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                Debug.Log($"Chat {chatRefs[0].Key} removed successfully.");
                chatRefs.Remove(chatRefs[0]);
            }
        });
    }
}

public class Chat
{
    public string nickName;
    public string message;
    public long Timestamp;

    public Chat(string nickName, string message)
    {
        this.nickName = nickName;
        this.message = message;
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}