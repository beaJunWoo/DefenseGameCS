using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using static DebuffData;

public class SR_RandomDebuff : MonoBehaviour
{
    [Header("DebuffSelectBox")]
    Image[] debuff_imgs;
    Text[] debuffNames;
    Text[] debuffExplanations;
    Text[] debuffRank;
    float[] randomAmount;
    DebuffType[] debuffType = new DebuffType[3];
    SR_Coin SR_coin;
    Image[] boxBorders;

    [Header("DebuffSelectBox")]
    RectTransform  DebuffUIBox;
    GameObject[] DebuffUIBtns = new GameObject[7];
    GameObject[] DebuffUITexts = new GameObject[7];

    public Text debuffBuyCost;
    public SR_DebuffDataBase debuffDataBase;
    public SR_Fastforward SR_fastforward;
    public GameObject[] buttons;
    public GameObject DebuffBox;
    public GameObject RefrashBtn;

    public GameObject BuyBtn;
    public Color LegendColor;
    public Color RareColor;
    public Color NormalColor;
    public Color LowColor;


    private const int RefreshCost = 50;
    int DataLen;
    int BuyDebuffCost = 500;

    void Start()
    {
        DataLen = debuffDataBase.debuffData.Count - 1;
        SR_coin = GameObject.Find("Cvs_Coin").GetComponent<SR_Coin>();

        debuff_imgs = new Image[buttons.Length];
        debuffNames =new Text[buttons.Length];
        debuffExplanations = new Text[buttons.Length];
        randomAmount = new float[buttons.Length];
        boxBorders = new Image[buttons.Length];
        debuffRank = new Text[buttons.Length];
        for (int i=0; i<buttons.Length; i++)
        {
            boxBorders[i] = buttons[i].GetComponent<Image>();
            debuff_imgs[i] = buttons[i].transform.GetChild(0).GetComponent<Image>();
            debuffNames[i] = buttons[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
            debuffExplanations[i] = buttons[i].transform.GetChild(1).GetComponent<Text>();
            debuffRank[i] = buttons[i].transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Text>();
            randomAmount[i] = 0;
        }
        DebuffUIBox = transform.GetChild(2).transform.GetComponent<RectTransform>();
        for (int i=0; i< DebuffUIBtns.Length; i++)
        {
            DebuffUIBtns[i] = transform.GetChild(2).transform.GetChild(i).gameObject;
            Debug.Log(DebuffUIBtns[i].name);
            DebuffUITexts[i] = DebuffUIBtns[i].transform.GetChild(0).transform.gameObject;
            DebuffUITexts[i].SetActive(false);
        }
      
        for (int i = 0; i < debuffDataBase.debuffData.Count; i++)
        {
            debuffDataBase.debuffData[i].NowDebuffAmount = 0;
        }
        ChangeDebuffUI();
    }

    public void StartDebuff()
    {
        BuyBtn.SetActive(false);
        DOTween.Restart("DebuffUIOn");
        for (int i = 0; i < 3; i++)
        {
            int randomIdx = Random.Range(0, debuffDataBase.debuffData.Count);
            debuffType[i] = debuffDataBase.debuffData[randomIdx].debuffType;

            debuff_imgs[i].sprite = debuffDataBase.debuffData[randomIdx].img;
            debuffNames[i].text = debuffDataBase.debuffData[randomIdx].debuffName;


            float min = debuffDataBase.debuffData[randomIdx].MinRandomRange;
            float max = debuffDataBase.debuffData[randomIdx].MaxRandomRange;
            float sub = max - min;
            float randomNum = Random.Range(0.0f, 100.0f);

            float Amount;
            if (randomNum < 10.0f) 
            {
                Amount = Random.Range(sub * 0.75f+min, max);
                boxBorders[i].color = LegendColor;
                debuffRank[i].text = "전설";
            }
            else if(randomNum < 30.0f)
            {
                Amount = Random.Range(sub * 0.5f + min, sub * 0.75f + min);
                boxBorders[i].color = RareColor;
                debuffRank[i].text = "레어";
            }
            else if (randomNum < 50.0f)
            {
                Amount = Random.Range(sub * 0.25f + min, sub * 0.5f + min);
                boxBorders[i].color = NormalColor;
                debuffRank[i].text = "보통";
            }
            else
            {
                Amount = Random.Range(min, sub * 0.25f + min);
                boxBorders[i].color = LowColor;
                debuffRank[i].text = "저급";
            }

            Amount = Mathf.Floor(Amount * 100f) / 100f;

            debuffExplanations[i].text = string.Format(debuffDataBase.debuffData[randomIdx].lastName, Amount);
            randomAmount[i] = Amount;      
        }
      
        
    }
    public void SelectDebuff(int boxNumber)
    {
        BuyBtn.SetActive(true);
        StartTime();
        int index = (int)debuffType[boxNumber];
        debuffDataBase.debuffData[index].NowDebuffAmount += randomAmount[boxNumber];


        SR_Army[] gameObjects = GameObject.FindObjectsOfType<SR_Army>();
        Debug.Log(gameObjects.Length);
        switch (debuffType[boxNumber])
        {
            case DebuffType.MaxHpBuff:
            case DebuffType.HpBuff:
                for (int i = 0; i < gameObjects.Length; i++)
                {
                    gameObjects[i].Heal((long)randomAmount[boxNumber]);
                    Debug.Log("heal");
                }
                break;
            case DebuffType.ArmorBuff:
                if (debuffDataBase.debuffData[index].NowDebuffAmount > 50.0f)
                    debuffDataBase.debuffData[index].NowDebuffAmount = 50.0f;
                break;
        }
        DOTween.Restart("DebuffUIOff");

        ChangeDebuffUI();
    }
    public void Refresh()
    {
        if (SR_coin.coin - RefreshCost< 0) { return; }
        SR_coin.DeductionCoin(50);
        StartTime();
        StartDebuff();
    }
    void ChangeDebuffUI()
    {
        int btnIdx = 0;
        for(int i=0; i < DataLen; i++)
        {
            if (debuffDataBase.debuffData[i].NowDebuffAmount>0.0f)
            {
                DebuffUIBtns[btnIdx].SetActive(true);
                DebuffUIBtns[btnIdx].GetComponent<Image>().sprite = debuffDataBase.debuffData[i].img;
                DebuffUITexts[btnIdx].GetComponent<Text>().text = 
                    debuffDataBase.debuffData[i].debuffName + "\n"+string.Format(debuffDataBase.debuffData[i].lastName, debuffDataBase.debuffData[i].NowDebuffAmount); 
                btnIdx++;
            }
        }
        DebuffUIBox.sizeDelta = new Vector2(40* btnIdx-1, 40);
        for(int i= btnIdx; i< DataLen; i++)
        {
            DebuffUIBtns[i].SetActive(false);
        }
    }
    public void CheckBuffInfo()
    {
        int index = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        DebuffUITexts[index].SetActive(true);
        StartCoroutine(HideBuffText());
    }
    public void BuyDebuff()
    {
        if (!SR_coin.DeductionCoin(BuyDebuffCost)) { return; }
        BuyDebuffCost += (int)(BuyDebuffCost * 0.1f);
        debuffBuyCost.text =string.Format("버프 구입({0})", BuyDebuffCost.ToString());
        StartDebuff();
    }
    IEnumerator HideBuffText()
    {
        yield return new WaitForSeconds(5.0f);

        for (int i = 0; i < DebuffUITexts.Length; i++)
        {
            DebuffUITexts[i].SetActive(false);
        }

    }
    public void StopTime()
    {
        Time.timeScale = 0.0f;
    }
    public void StartTime()
    {
        SR_fastforward.UpdateSpeed();
    }
    public void CheckStartBuff()
    {
        Debug.Log(SR_GameManager.instance.GetSpecialList()[0].Item1);
        if(SR_GameManager.instance.GetSpecialList()[0].Item2 == 0)
        {
            Invoke("StartDebuff",1.0f);
        }
    }
}
