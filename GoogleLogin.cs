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
        }
        else
        {
            Debug.Log("�α��� ����");
        }
    }


}
