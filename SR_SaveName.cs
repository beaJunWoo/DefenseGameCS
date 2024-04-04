using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SR_SaveName : MonoBehaviour
{
    public TMP_InputField Txt_Name;
    public Text Txt_ErrorMassage;
    [SerializeField]
    string name;
    [SerializeField]
    int namesize;

    private void Start()
    {
        Debug.Log(SR_GameManager.instance.nickName);
        name = SR_GameManager.instance.nickName;
        if (TextError())
        {

        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        name = Txt_Name.text;
        Debug.Log(name.Length);
        Debug.Log(TextError());
        if (TextError())
        {
            Txt_ErrorMassage.text = "닉네임은 2~8글자 사이를 입력하세요.";
            return;
        }
        Txt_ErrorMassage.text = "사용가능한 닉네임입니다.";
    }
    public void SaveName()
    {
        if(!TextError())
        {
            SR_GameManager.instance.nickName = Txt_Name.text;
            Destroy(gameObject);
        }
    }
    bool TextError()
    {
        return name.Length < 2 || name.Length > 8;
    }
   
}
