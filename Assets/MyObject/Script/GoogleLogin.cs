using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;


public class GoogleLogin : MonoBehaviour
{

    public void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("로그인 성공");
            Social.ReportProgress("CgkI1bOThJcHEAIQAw", 100f,null);
        }
        else
        {
            Debug.Log("로그인 실패");
        }
    }
    public void Ch()
    {
        Social.ReportProgress("CgkI1bOThJcHEAIQAw", 100f, null);
    }

}
