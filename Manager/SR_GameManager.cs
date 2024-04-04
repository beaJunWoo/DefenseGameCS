using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SR_GameManager : MonoBehaviour
{

    public static SR_GameManager instance;
    public GameObject GameManager;
    public SR_StageSettingDataBase stageSettingData;
    public SR_EnemyData SR_enemyData;
    public SR_ArmyData armyData;

    int[][] StageClearData = new int[5][];
    string scene;
    public string nickName;
    public float bgmVolume = 0.7f;
    public float sfxVolume = 0.45f;

    int gameMoney = 0;
    int Diamond = 0;

    int stage = 0;
    int stageLv = 0;
    public bool challengeMode = false;
    public int score = 0;
    public bool bNewSocre = false;
    public int MaxUnLockStage = 0;
    public int GetStage() { return stage; }
    public int GetStageLv() { return stageLv; }
    List<Tuple<string, int>> ItemsList = new List<Tuple<string, int>>();
    List<Tuple<string, int>> SkillsList = new List<Tuple<string, int>>();
    List<Tuple<string, int>> SpecialList = new List<Tuple<string, int>>();
    public List<int> DamageUpgradeCount;
    public List<int> CriticalUpgradeCount;
    public List<int> ShieldUpgradeCount;
    void Awake()
    {
       if(instance == null)
            instance = this;
       else 
            Destroy(GameManager);

        DontDestroyOnLoad(gameObject);
        SettingData();
        DataLoad();
    }
    private void Start()
    {
      
    }
    public void SceneLoad(string sceneName)
    {
        if (StageClearData[1][0] > 0) { Debug.Log("sendHalleng"); SR_ChallengeManager.instance.SendChalleng(SR_ChallengeManager.ChallengType.ClearFirstWar); }
        Save();
        SceneManager.LoadScene("LoadingScene");
        scene = sceneName;
    }
    public void Save()
    {
        SR_SaveLoadManager.instance.Save(nickName, score, gameMoney, Diamond, ItemsList, SkillsList, SpecialList, StageClearData, bgmVolume, sfxVolume,
        DamageUpgradeCount, CriticalUpgradeCount, ShieldUpgradeCount);
    }
    public string GetSceneName()
    {
        return scene;
    }
    public void GetItem(int idx)
    {
        ItemsList.Insert(idx,new Tuple<string, int>(ItemsList[idx].Item1,0));
        ItemsList.RemoveAt(idx + 1);
    
    }
    public void GetSkill(int idx)
    {
        SkillsList.Insert(idx, new Tuple<string, int>(SkillsList[idx].Item1, 0));
        SkillsList.RemoveAt(idx + 1);
    }
    public void GetSpecial(int idx)
    {
        SpecialList.Insert(idx, new Tuple<string, int>(SpecialList[idx].Item1, 0));
        SpecialList.RemoveAt(idx + 1);
    }

    public List<Tuple<string, int>> GetItemList() { return ItemsList; }
    public List<Tuple<string, int>> GetSkillList() { return SkillsList; }
    public List<Tuple<string, int>> GetSpecialList() { return SpecialList; }

    public int GetGameMoney() { return gameMoney; }
    public void AddGameMoney(int money) 
    { 
        gameMoney += money;
        if(GameObject.Find("Cvs_Money")!=null)
        {
            GameObject.Find("Cvs_Money").GetComponent<SR_Money>().UpdateText();
        }
       
    }
    public void AddDiamond(int diamond)
    {
        Diamond += diamond;
        GameObject.Find("Cvs_Money").GetComponent<SR_Money>().UpdateText();
    }
    public bool DecDiamond(int diamond)
    {
        if(Diamond>=diamond)
        {
            Diamond -= diamond;
            GameObject.Find("Cvs_Money").GetComponent<SR_Money>().UpdateText();
            return true;
        }
        else { return false; }
    }
    public bool DecGameMoney(int money) 
    {
        if (gameMoney >= money)
        {
            gameMoney -= money;
            GameObject.Find("Cvs_Money").GetComponent<SR_Money>().UpdateText();
            return true;
        }
        else { return false; }
    }
    public int GetDiamond() { return Diamond; }
    public int GetScore() {  return score; }
    public bool SetScore(int score)
    {
        if(this.score<score)
        {
            this.score = score;
            return true;
        }
        else { return false; }
    }
    public void setStage(int stage)
    {
        this.stage = stage;
        Debug.Log("Stage:" + stage);
    }
    public void setStageLv(int stageLv) { this.stageLv = stageLv; Debug.Log("StageLV:" + stageLv); }
    public int getClearInfo(int stage, int stageLv) { return StageClearData[stage][stageLv]; }
    public void SetStageClearData(int Stars) 
    {
        if (StageClearData[stage][stageLv - 1] < Stars)
            StageClearData[stage][stageLv - 1] = Stars;
    }
    public int[][] GetStageClearData() { return StageClearData; }
    public void DataLoad()
    {
        SR_SaveData saveData = SR_SaveLoadManager.instance.Load();
        if(saveData == null) { return; }

        nickName = saveData.nickName;
        score = saveData.score;
        gameMoney = saveData.Coin;
        Diamond = saveData.Diamond;

        ItemsList = ConvertToTupleList(ItemsList,saveData.ItemsList);
        SkillsList = ConvertToTupleList(SkillsList, saveData.SkillsList);
        SpecialList = ConvertToTupleList(SpecialList, saveData.SpecialList);


        for (int i=0; i< saveData.StageClearData.Length; i++)
        {
            StageClearData[i] = saveData.StageClearData[i].Data;
        }
        sfxVolume = saveData.SfxVolume;
        bgmVolume = saveData.BgmVolume;

        //"DamageUpgradeCount":[28,0,0,0],"CriticalUpgradeCount":[4,0,0,0],"ShieldUpgradeCount":[0,0,0,0]}
        DamageUpgradeCount = saveData.DamageUpgradeCount;
        CriticalUpgradeCount = saveData.CriticalUpgradeCount;
        ShieldUpgradeCount = saveData.ShieldUpgradeCount;

        for(int i=0; i<3; i++)
        {
            int damageCount = DamageUpgradeCount[i];
            armyData.armyData[i].additionalDamage = Upgrade(damageCount, armyData.MaxAdditionalDamage);
            armyData.armyData[i].UpgradCost_addDamage = UpgradePrice(damageCount, armyData.armyData[i].defaultUpgradCost);

            int criticalCount = CriticalUpgradeCount[i];
            armyData.armyData[i].critical = Upgrade(criticalCount, armyData.MaxCritical);
            armyData.armyData[i].UpgradCost_critical = UpgradePrice(criticalCount, armyData.armyData[i].defaultUpgradCost);

            int shieldCount = ShieldUpgradeCount[i];
            armyData.armyData[i].shield = Upgrade(shieldCount, armyData.MaxShield);
            armyData.armyData[i].UpgradCost_shield = UpgradePrice(shieldCount, armyData.armyData[i].defaultUpgradCost);
        }
      
    }
    float Upgrade(int count,float MaxUpgrade)
    {
        float upgradeAmounth = 0f;
        for(int i=0; i<count; i++)
        {
            upgradeAmounth = Mathf.Lerp(upgradeAmounth, MaxUpgrade, 0.1f);
        }
        return upgradeAmounth;
    }
    int UpgradePrice(int count, int defaultCost)
    {
        for(int i=0; i<count ; i++)
        {
            defaultCost = (int)(defaultCost * 1.5f);
        }
        return defaultCost;
    }

    private List<Tuple<string, int>> ConvertToTupleList(List<Tuple<string, int>> List, int[] values)
    {
        List<Tuple<string, int>> newList = new List<Tuple<string, int>>();
        for (int i = 0; i < List.Count; i++)
        {
            newList.Add(new Tuple<string, int>(List[i].Item1, values[i]));
        }
        return newList;
    }
    public void SettingData()
    {
        nickName = "";
        score = 0;
        gameMoney = 100000;
        Diamond = 0;

        ItemsList.Clear();
        SkillsList.Clear();
        SpecialList.Clear();

        ItemsList.Add(new Tuple<string, int>("소총 군인", 1000));
        ItemsList.Add(new Tuple<string, int>("스나이퍼 군인", 5000));


        SkillsList.Add(new Tuple<string, int>("미사일", 12000));
        SkillsList.Add(new Tuple<string, int>("미니건", 12000));

        SpecialList.Add(new Tuple<string, int>("추가 디버프", 500));

        for (int i = 0; i < StageClearData.Length; i++)
        {
            StageClearData[i] = new int[3];
            for (int j = 0; j < StageClearData[i].Length; j++)
            {
                StageClearData[i][j] = 0;
            }
        }
        DamageUpgradeCount.Clear();
        CriticalUpgradeCount.Clear();
        ShieldUpgradeCount.Clear();

        for(int i=0; i<(int)ArmyType.minigun; i++)
        {
            DamageUpgradeCount.Add(0);
            CriticalUpgradeCount.Add(0);
            ShieldUpgradeCount.Add(0);
        }

    } 
}
