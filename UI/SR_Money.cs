using UnityEngine;
using UnityEngine.UI;

public class SR_Money : MonoBehaviour
{

    public Text Txt_GameMoney;
    public Text Txt_Diamond;

    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        Txt_GameMoney.text = SR_GameManager.instance.GetGameMoney().ToString();
        Txt_Diamond.text = SR_GameManager.instance.GetDiamond().ToString();
    }
}

