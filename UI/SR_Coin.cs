using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Threading;

public class SR_Coin: MonoBehaviour
{
    public Text Txt_Coin;
    public Text Txt_Interest;
    public int coin = 0;

    private void Awake()
    {
        if (SR_GameManager.instance.challengeMode)
        {
            coin = 15000;
        }
        else
        {
            int stageDataIndex = SR_GameManager.instance.GetStageLv() - 1 + (SR_GameManager.instance.GetStage() - 1) * 3;
            coin = SR_GameManager.instance.stageSettingData.stageData[stageDataIndex].DefaultMony;
        }
        Txt_Interest.text = "";
        UpdateCoinText();
    }
    public int GetCoin() { return coin; }

    public bool DeductionCoin(int deductionPrice)
    {
        if (coin - deductionPrice < 0) {  return false; }
        else
        {
            Interlocked.Add(ref this.coin, -deductionPrice);
            UpdateCoinText();
        }
        return true;
    }

    public void AddCoin(int coin)
    {
        Interlocked.Add(ref this.coin, coin);
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        Txt_Coin.text = coin.ToString();
    }

    public void StartInterest()
    {
        StartCoroutine(IntersetCoroutin());
    }
    IEnumerator IntersetCoroutin()
    {
        while(true)
        {
            Txt_Interest.text = string.Format("+{0} (ÃÊ´ç)", coin / 100);
            AddCoin(coin / 100);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
