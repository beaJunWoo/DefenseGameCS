using Firebase.Database;
using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SR_Login: MonoBehaviour
{
    [Header("Input")]
    public TMP_InputField Txt_nickName;
    public TMP_InputField Txt_password;

    [Header("Button")]
    public Button Btn_Login;
    public Button Btn_SignIn;

    [Header("Text")]
    public Text Txt_LoginInfo;

    [Header("GameObject")]
    public GameObject LoginPage;
    public GameObject SignUpPage;

    DatabaseReference userData;

    private void Awake()
    {
        userData = FirebaseDatabase.DefaultInstance.GetReference("User");
        Txt_LoginInfo.text = "";
    }
    private void Start()
    {
        Btn_Login.onClick.AddListener(() => { Login(); });
        Btn_SignIn.onClick.AddListener(() => { SingUpPageOn(); });

        LoginData loginData = SR_SaveLoadManager.instance.LoadLoginData();
        if (loginData != null)
        {
            Txt_nickName.text = loginData.nickName;
            Txt_password.text = loginData.password;
            Login();
        }
       
    }
    async void Login()
    {
        bool loginSuccess = await CheckUser();
        if(loginSuccess)
        {
            Txt_LoginInfo.text = "로그인 성공!";
            SR_GameManager.instance.nickName = Txt_nickName.text;
            SR_GameManager.instance.DataLoad();
            SR_SaveLoadManager.instance.SaveLoginData(Txt_nickName.text, Txt_password.text);
            LoginPage.SetActive(false);
        }
        else
        {
            Txt_LoginInfo.text = "아이디 또는 비밀번호가 일치하지 않습니다.";
        }
    }
    async Task<bool> CheckUser()
    {
        try
        {
            DataSnapshot snapshot = await userData.Child(Txt_nickName.text).GetValueAsync();

            IDictionary user = (IDictionary)snapshot.Value;
            string password = (string)user["password"];
            if(password == Txt_password.text)
            { return true; }

            return false;  
        }
        catch(Exception ex)
        {
            Debug.Log("로그인 실패: " +ex.Message);
            return false;
        }
    }
    void SingUpPageOn()
    {
        SignUpPage.SetActive(true);
    }
}
