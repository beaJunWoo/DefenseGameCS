using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Firebase.Database;
using System.Collections.Generic;

public class FB_PVP : MonoBehaviour
{
    public GameObject GO_PVP;
    public GameObject GO_CancleMatching;

    public Button btn_Exit;
    public Button btn_StartMatching;
    Button btn_PVP;
    Button btn_CancleMatching;

    DatabaseReference reference;
    public DatabaseReference roomRef;

    public Room room;
    public int PlayerNum;

    bool MatchingComplate = false;
    bool isPvpStarted = false;
    void Start()
    {

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        btn_PVP = GO_PVP.GetComponent<Button>();
        btn_PVP.onClick.AddListener(() => PVPUIOn());
        btn_Exit.onClick.AddListener(() => PVPUIOff());
        btn_StartMatching.onClick.AddListener(() => StartMatching());

        btn_CancleMatching = GO_CancleMatching.GetComponent<Button>();
        btn_CancleMatching.onClick.AddListener(() => CancleMatching());
        GO_CancleMatching.SetActive(false);
    }

    private void Update()
    {
        if (MatchingComplate)
        {
            StartPVP();
        }
    }
    void PVPUIOn()
    {
        DOTween.Restart("PVPUIOn");
    }
    void PVPUIOff()
    {
        DOTween.Restart("PVPUIOff");
    }
    void StartMatching()
    {
        GO_CancleMatching.SetActive(true);

        reference.Child("MatchingRoom").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving data");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("방만들기");
                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    room = JsonUtility.FromJson<Room>(childSnapshot.GetRawJsonValue());
                    Debug.Log($"Room: P1 = {room.p1}, P2 = {room.p2}");
                    if (room.p2 == "")
                    {
                        UpdateRoom(childSnapshot.Key, room);
                        Debug.Log("업데이트됨!");
                        return;
                    }
                  
                }
                CreateRoom();
            }
        });

    }
    void UpdateRoom(string key, Room room) //내가 빈방에 들어오면
    {

        Dictionary<string, object> updates = new Dictionary<string, object>();
        updates["p2"] = SR_GameManager.instance.nickName;

        roomRef = reference.Child("MatchingRoom").Child(key);
        roomRef.UpdateChildrenAsync(updates).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                PlayerNum = 2;   
                MatchingComplate = true;
            }
        });
    }
    void CreateRoom()
    {
        room = new Room(SR_GameManager.instance.nickName);
        string json = JsonUtility.ToJson(room);
        roomRef = reference.Child("MatchingRoom").Push();
        roomRef.SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                PlayerNum = 1;
                Debug.Log("Create Room success!");
                roomRef.Child("p2").ValueChanged += PlayerJoined;
            }

        });
    }
    void CancleMatching()
    {
        GO_CancleMatching.SetActive(false);
        if (roomRef != null)
        {
            roomRef.RemoveValueAsync().ContinueWith(task => {
                if (task.IsCompleted)
                {
                }
            });
        }

    }
    void PlayerJoined(object sender, ValueChangedEventArgs args) // 누군가 내방에 들어오면
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot.Exists && !string.IsNullOrEmpty(args.Snapshot.Value.ToString()))
        {
            Debug.Log($"Player 2 Joined: {args.Snapshot.Value.ToString()}");
           
            StartPVP();
        }
    }
    void StartPVP()
    {
        if (!isPvpStarted)
        {
            Debug.Log("PVP씬으로 이동");
            DontDestroyOnLoad(gameObject);
            SR_GameManager.instance.SceneLoad("PVP");
            isPvpStarted = true;
            PVPUIOff();
            GO_PVP.SetActive(false);
            Debug.Log(PlayerNum);
        }

    }
    void OnApplicationQuit()
    {
       if(roomRef != null)
        {
            CancleMatching();
        }
    }
}
public class Room
{
    public string p1;
    public string p2;
    public float p1Hp;
    public float p2Hp;
    public bool p1_Army;
    public bool p2_Army;
    public Room(string p1)
    {
        this.p1 = p1;
        this.p2 = "";
        p1Hp = 100f;
        p2Hp = 100f;
        p1_Army = false;
        p2_Army = false;
    }
}

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> jobs = new Queue<Action>();

    private void Update()
    {
        while (jobs.Count > 0)
        {
            jobs.Dequeue().Invoke();
        }
    }

    public static void Enqueue(Action job)
    {
        jobs.Enqueue(job);
    }
}