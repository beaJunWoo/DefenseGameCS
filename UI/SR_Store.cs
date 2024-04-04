using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SR_Store : MonoBehaviour
{
    public enum BuyType { GAMEMONEY,DIAMOND}

    BuyType buyType;
    
    SR_GameManager SR_gameManager;
    SR_Money SR_money;

    public Text[] Txt_ItemName = new Text[1];
    public Text[] Txt_ItemPrice = new Text[1];
    public Button[] Btn_Items = new Button[1];
    
    public Text[] Txt_SkillName = new Text[1];
    public Text[] Txt_SkillPrice = new Text[1];
    public Button[] Btn_Skills = new Button[1];

    public Text[] Txt_SpecialName;
    public Text[] Txt_SpecialPrice;
    public Button[] Btn_Specials;

    public Text Txt_ChangeMoneyInfo;
    public GameObject DarkBackGround;

    string nowClickItem;
    List<Tuple<string, int>> ItemsList;
    List<Tuple<string, int>> SkillsList;
    List<Tuple<string, int>> SpecialList;


    void Start()
    {
        SR_gameManager = GameObject.Find("GameManager").GetComponent<SR_GameManager>();
        SR_money = GameObject.Find("Cvs_Money").GetComponent<SR_Money>();

        FixedText();
    }
    public void ItemBuy(string ItemName)
    {
        buyType = BuyType.GAMEMONEY;
        int i;
        for (i= 0; i < ItemsList.Count; i++)
        {
            if (ItemsList[i].Item1 == ItemName)
            {
                break;
            }
        }
        bool canBuy = SR_gameManager.GetGameMoney() - ItemsList[i].Item2 >= 0;

        if (canBuy)
        {
            DOTween.Restart("On");
            Txt_ChangeMoneyInfo.text = string.Format("골드{0}-> {1}", SR_gameManager.GetGameMoney(), SR_gameManager.GetGameMoney() - ItemsList[i].Item2);
            nowClickItem = ItemName;
        }
        else
        {
            DOTween.Restart("NoMoeneyOn");
        }

        DarkBackGround.SetActive(true);
    }
    public void SkillBuy(string ItemName)
    {
        buyType = BuyType.GAMEMONEY;
        int i;
        for (i = 0; i < SkillsList.Count; i++)
        {
            if (SkillsList[i].Item1 == ItemName)
            {
                break;
            }
        }
        bool canBuy = SR_gameManager.GetGameMoney() - SkillsList[i].Item2 >= 0;

        if (canBuy)
        {
            DOTween.Restart("On");
            Txt_ChangeMoneyInfo.text = string.Format("골드 {0}-> {1}", SR_gameManager.GetGameMoney(), SR_gameManager.GetGameMoney() - SkillsList[i].Item2);
            nowClickItem = ItemName;
        }
        else
        {
            DOTween.Restart("NoMoeneyOn");
        }

        DarkBackGround.SetActive(true);
    }
    public void SpecialBuy(string ItemName)
    {
        buyType = BuyType.DIAMOND;
        int i;
        for (i = 0; i < SkillsList.Count; i++)
        {
            if (SpecialList[i].Item1 == ItemName)
            {
                break;
            }
        }
        int TotalDiamond = SR_gameManager.GetDiamond();
        int price = SpecialList[i].Item2;
        bool canBuy = TotalDiamond - price >= 0;
        if(canBuy)
        {

            DOTween.Restart("On");
            Txt_ChangeMoneyInfo.text = string.Format("다이아 {0}-> {1}", TotalDiamond, TotalDiamond - price);
            nowClickItem = ItemName;
        }
        else
        {
            DOTween.Restart("NoMoeneyOn");
        }
    }

    public void ItemBuyCancle()
    {
        nowClickItem = null;
        DarkBackGround.SetActive(false);
    }
    public void AddItem()
    {
        SR_SoundManager.instance.PlayBtnSfx(SR_SoundManager.BtnSfx.Buy);

        int i;
        if (buyType == BuyType.DIAMOND)
        {
            
            for (i = 0; i < SpecialList.Count; i++)
            {
                if (SpecialList[i].Item1 == nowClickItem)
                {
                    break;
                }
            }
            if (i < SpecialList.Count)
            {
                SR_gameManager.DecDiamond(SpecialList[i].Item2);
                SR_gameManager.GetSpecial(i);
                SR_money.UpdateText();
                DarkBackGround.SetActive(false);
                Btn_Specials[i].interactable = false;
                FixedText();
                return;
            }
        }

        for (i = 0; i < ItemsList.Count; i++)
        {
            if (ItemsList[i].Item1 == nowClickItem)
            {
                break;
            }
        }

        if(i<ItemsList.Count)
        {
            SR_gameManager.DecGameMoney(ItemsList[i].Item2);
            SR_gameManager.GetItem(i);
            SR_money.UpdateText();
            DarkBackGround.SetActive(false);
            Btn_Items[i].interactable = false;
            FixedText();
            return;
        }
        for (i = 0; i < SkillsList.Count; i++)
        {
            if (SkillsList[i].Item1 == nowClickItem)
            {
                break;
            }
        }
        SR_gameManager.DecGameMoney(SkillsList[i].Item2);
        SR_gameManager.GetSkill(i);
        SR_money.UpdateText();
        DarkBackGround.SetActive(false);
        Btn_Skills[i].interactable = false;
        FixedText();

    }

    void FixedText()
    {
        ItemsList = SR_gameManager.GetItemList();
        for (int i = 0; i < Txt_ItemName.Length; i++)
        {
            Txt_ItemName[i].text = ItemsList[i].Item1;
            if (ItemsList[i].Item2 != 0)
            {
                Txt_ItemPrice[i].text = ItemsList[i].Item2.ToString();
            }
            else
            {
                Txt_ItemPrice[i].text = "구매 완료";
                Btn_Items[i].interactable = false;
            }
            
        }
        SkillsList = SR_gameManager.GetSkillList();
        for (int i = 0; i < Txt_SkillName.Length; i++)
        {
            Txt_SkillName[i].text = SkillsList[i].Item1;
            if (SkillsList[i].Item2 != 0)
            {
                Txt_SkillPrice[i].text = SkillsList[i].Item2.ToString();
            }
            else
            {
                Txt_SkillPrice[i].text = "구매 완료";
                Btn_Skills[i].interactable = false;
            }

        }
        SpecialList = SR_gameManager.GetSpecialList();
        for (int i = 0; i < Txt_SpecialName.Length; i++)
        {
            Txt_SpecialName[i].text = SpecialList[i].Item1;
            if (SpecialList[i].Item2 != 0)
            {
                Txt_SpecialPrice[i].text = SpecialList[i].Item2.ToString();
            }
            else
            {
                Txt_SpecialPrice[i].text = "구매 완료";
                Btn_Specials[i].interactable = false;
            }
        }
        

    }
}

