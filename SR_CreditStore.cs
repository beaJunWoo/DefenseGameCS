using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SR_CreditStore : MonoBehaviour
{
    public GameObject CompleteBuyBox;
    public Sprite diamondSprite;
    public Sprite CoinSprite;

    int[] diamondPrice = { 0, 400, 800 };
    int[] storeMoney = {0, 5000, 12000 };

    Image image;
    Text text;
    public void Start()
    {
        image = CompleteBuyBox.transform.GetChild(0).transform.GetComponent<Image>();
        text = CompleteBuyBox.transform.GetChild(0).transform.GetChild(0).transform.GetComponent<Text>();
    }
    public void BuyDiamond()
    {
        SR_GameManager.instance.AddDiamond(150);
       
        image.sprite = diamondSprite;
        text.text = "+" + 150;
        DOTween.Restart("OpenCredit");
    }
    public void BuyCredit(int index)
    {
        if (SR_GameManager.instance.GetDiamond() - diamondPrice[index] < 0) { return; }
        SR_GameManager.instance.DecDiamond(diamondPrice[index]);
        SR_GameManager.instance.AddGameMoney(storeMoney[index]);

        image.sprite = CoinSprite;
        text.text = "+"+storeMoney[index];
        DOTween.Restart("OpenCredit");
    }
    
}
