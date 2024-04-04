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
        }
        else
        {
            Debug.Log("로그인 실패");
        }
    }


}
