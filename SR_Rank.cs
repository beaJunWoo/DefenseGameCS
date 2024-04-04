using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SR_Rank : MonoBehaviour
{
    [SerializeField]
    Text Txt_NickName;
    [SerializeField]
    TextMeshProUGUI Score;
    [SerializeField]
    LeaderboardShowcase rankborder;
    private void Awake()
    {
        Txt_NickName.text = SR_GameManager.instance.nickName;
        Score.text = "점수 : " + SR_GameManager.instance.GetScore().ToString();
    }
    private void Start()
    {
        Invoke("Submit",0.5f);
    }
    void Submit()
    {
        rankborder.SetScore(SR_GameManager.instance.GetScore());
        Score.text = "점수 : " + SR_GameManager.instance.GetScore().ToString();
        rankborder.Submit();
        SR_GameManager.instance.bNewSocre = false;

    }

}
