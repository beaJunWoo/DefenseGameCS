using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_ChallengeManager: MonoBehaviour
{
    public static SR_ChallengeManager instance;

    List<string>achivements = new List<string>();

    bool isLogin = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        achivements.Add(GPGSIds.achievement);
    }

    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }
    //private void Update()
    //{
    //    PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    //    Social.ReportProgress("CgkI1bOThJcHEAIQAw", 100f, (bool success) =>
    //    {
    //    });
    //    Debug.Log(achivements[0]);
    //
    //}

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            isLogin = true;
        }
    }

    public enum ChallengType
    {
        ClearFirstWar, //0
    }
    

    public void SendChalleng(int index)
    {
        if (!isLogin) { return; }
        Social.ReportProgress(achivements[index], 100f, (bool success)=> 
        {
            Debug.Log(success);
            if (!success)
            {
                StartCoroutine(CheckChallenges(index));
            }
        });
    }
    public void SendChalleng(ChallengType challengType)
    {
        SendChalleng((int)challengType);
       
    }
    IEnumerator CheckChallenges(int index)
    {
        bool isChallengeCompleted = false;

        while (true)
        {
            yield return new WaitForSeconds(10.0f);
            Social.ReportProgress(achivements[index], 100f, (bool success) =>
            {
                Debug.Log("업적다시로드중");
                isChallengeCompleted = success;
            });
            if(isChallengeCompleted) { break; }
        }
       
    }
}
