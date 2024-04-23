using Firebase.Database;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SR_SignIn: MonoBehaviour
{
    [Header("Input")]
    public TMP_InputField Txt_nickName;
    public TMP_InputField Txt_password;
    public TMP_InputField Txt_checkPassword;

    [Header("Button")]
    public Button Btn_checkNickName;
    public Button Btn_SignIn;
    public Button Btn_closeSignIn;
    public Button Btn_exit;

    [Header("Text")]
    public Text Txt_NickNameInfo;
    public Text Txt_PasswordInfo;
    string nickNameInfo;
    string passwordInfo;

    [Header("GameObject")]
    public GameObject SingupPage;
 
    DatabaseReference userData;

    bool nickNameCheck = false;
    
    bool CheckNicknameLength(){ return 2 <= Txt_nickName.text.Length && Txt_nickName.text.Length <= 10; }
  
    
    private void Awake()
    {
        userData = FirebaseDatabase.DefaultInstance.GetReference("User");
    }
    private void Start()
    {
        SingupPage.SetActive(false);

        Btn_checkNickName.onClick.AddListener(() => { CheckNickName(); });
        Btn_SignIn.onClick.AddListener(() => { SignIn(); });
        Btn_closeSignIn.onClick.AddListener(() => { closeSignIn(); });
        Btn_exit.onClick.AddListener(() => { closeSignIn(); });

        Txt_NickNameInfo.text = "";
        Txt_PasswordInfo.text = "";
        StartCoroutine(UpdateUI());
       
    }
    
    void CheckNickName()
    {
        Debug.Log("중복체크");
        if (!CheckNicknameLength()) { nickNameInfo = "2~10자 사이로 입력하세요."; return; }

        userData.Child(Txt_nickName.text).GetValueAsync().ContinueWith(task =>
        {
            if(task.IsFaulted)
            {
                Debug.Log("실패");
              
            }
            else if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (!snapshot.Exists)
                {
                    nickNameCheck = true;
                    nickNameInfo = "사용가능한 닉네임입니다.";

                }else
                {
                    nickNameInfo = "중복된 닉네임입니다.";
                }
                Debug.Log("성공");
                return;
            }
        });
    }
   
    void SignIn()
    {
        if (!nickNameCheck) { nickNameInfo = "닉네임 중복확인을 해주세요."; return; }
        if (!PasswordMatch()) { return; }
        if (!CheckPasswordLength()){ return; }

        User user = new User(Txt_password.text);
        string Juser = JsonUtility.ToJson(user);
        userData.Child(Txt_nickName.text).SetRawJsonValueAsync(Juser);
        passwordInfo = "";
        SR_SaveLoadManager.instance.SaveLoginData(Txt_nickName.text, Txt_password.text);
        DOTween.Restart("On");
    }

    void closeSignIn()
    {
        SingupPage.SetActive(false);
    }
    bool PasswordMatch()
    {
        if (Txt_password.text == Txt_checkPassword.text)
            return true;
        
        passwordInfo = "비밀번호가 일치하지 않습니다.";
        return false;
    }
    bool CheckPasswordLength()
    {
        if (Txt_password.text.Length >= 6 && Txt_password.text.Length <= 16)
            return true;

        passwordInfo = "비밀번호 길이는 6~16로 지정하셔야합니다.";
        return false;
    }

    IEnumerator UpdateUI()
    {
        while (true)
        {
            Txt_NickNameInfo.text =nickNameInfo;
            Txt_PasswordInfo.text =passwordInfo;
            yield return new WaitForSeconds(0.1f);
        }
       
    }
}
public class User
{
    public string password;

    public User(string password)
    {
        this.password = password;
    }

}
