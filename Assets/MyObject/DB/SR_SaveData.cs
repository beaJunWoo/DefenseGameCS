using System;
using System.Collections.Generic;

[System.Serializable]
public class StageClearData
{
    public int[] Data;
}

[System.Serializable]
public class SR_SaveData
{
   public SR_SaveData(string nickName,int score, int Coin,int Diamond, List<Tuple<string, int>> ItemsList, 
       List<Tuple<string, int>> SkillsList, List<Tuple<string, int>> SpecialList, int[][] StageClearData, float bgmVolume, float sfxVolume,
       List<int> DamageUpgradeCount, List<int> CriticalUpgradeCount, List<int> ShieldUpgradeCount)
    {
        this.nickName = nickName;
        this.score = score;
        this.Coin = Coin;
        this.Diamond = Diamond;
        this.ItemsList = new int[ItemsList.Count];
        for(int i = 0; i < ItemsList.Count; i++)
        {
            this.ItemsList[i] = ItemsList[i].Item2;
        }

        this.SkillsList = new int[SkillsList.Count];
        for (int i = 0; i < SkillsList.Count; i++)
        {
            this.SkillsList[i] = SkillsList[i].Item2;
        }

        this.SpecialList = new int[SpecialList.Count];
        for (int i = 0; i < SpecialList.Count; i++)
        {
            this.SpecialList[i] = SpecialList[i].Item2;
        }


        this.StageClearData = new StageClearData[StageClearData.Length];

        for (int i = 0; i < StageClearData.Length; i++)
        {
            this.StageClearData[i] = new StageClearData { Data = StageClearData[i] };
        }
        BgmVolume = bgmVolume;
        SfxVolume = sfxVolume;

        this.DamageUpgradeCount = DamageUpgradeCount;
        this.CriticalUpgradeCount = CriticalUpgradeCount;
        this.ShieldUpgradeCount = ShieldUpgradeCount;
    }
    public string nickName;
    public int score;
    public int Coin;
    public int Diamond;
    public int[] ItemsList;
    public int[] SkillsList;
    public int[] SpecialList;
    public StageClearData[] StageClearData;
    public float BgmVolume;
    public float SfxVolume;
    public List<int> DamageUpgradeCount;
    public List<int> CriticalUpgradeCount;
    public List<int> ShieldUpgradeCount;
}
