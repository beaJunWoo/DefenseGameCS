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
            Debug.Log("�α��� ����");
            Social.ReportProgress("CgkI1bOThJcHEAIQAw", 100f,null);
        }
        else
        {
            Debug.Log("�α��� ����");
        }
    }
    public void Ch()
    {
        Social.ReportProgress("CgkI1bOThJcHEAIQAw", 100f, null);
    }

}
