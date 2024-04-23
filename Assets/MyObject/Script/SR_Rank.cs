using Firebase.Database;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SR_Rank : MonoBehaviour
{
    public Text Txt_nickName;
    public Text Txt_score;

    public GameObject Content;
    public GameObject Entry;

    string nickName;
    int score;
    DatabaseReference rankData;

    private void Awake()
    {
        rankData = FirebaseDatabase.DefaultInstance.GetReference("Rank");
    }
    private void Start()
    {
        nickName = SR_GameManager.instance.nickName;
        score = SR_GameManager.instance.score;
        Txt_nickName.text = nickName;
        Txt_score.text = "���� :"+ score;
        UpdateScore();
        LoadRankData();
    }
    void UpdateScore()
    {
        Debug.Log("���� ���ε�");
        RankScore rankNickName = new RankScore(score);
        string json =JsonUtility.ToJson(rankNickName);
        rankData.Child(nickName).SetRawJsonValueAsync(json);
    }
    async void LoadRankData()
    {
        // "score"�� �������� �����͸� �������� �����ϰ�,
        // ���� �������� ���� Firebase ���� ����
        Query query = rankData.OrderByChild("score").LimitToLast(10); // ���⼭�� ���� 10���� ������

        DataSnapshot snapshot = await query.GetValueAsync();

        int Rank = (int)snapshot.ChildrenCount;
        foreach (DataSnapshot child in snapshot.Children)
        {
            Debug.Log("����");

            int fetchedScore = Convert.ToInt32(child.Child("score").Value);
            string fetchedNickName = child.Key; // ����� Ű�� nickname�� ��Ÿ��

            GameObject entry = Instantiate(Entry, Content.transform);
            EntryDisplay entryDisplay = entry.GetComponent<EntryDisplay>();

            bool isMine = SR_GameManager.instance.nickName == fetchedNickName;
            entryDisplay.SetEntry(Rank, fetchedNickName, fetchedScore, isMine);
            Rank--;

            Debug.Log(fetchedNickName + ": " + fetchedScore);
        }
    }
}
public class RankScore
{
    public int score;
    public RankScore(int score)
    {
        this.score = score;
    }
}
