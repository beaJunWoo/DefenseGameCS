using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class SR_SaveLoadManager : MonoBehaviour
{
    public static SR_SaveLoadManager instance;

    static string SavePath => Application.persistentDataPath + "/SaveFile/";
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Save(string nickName,int score,int Coin,int Diamond, List<Tuple<string, int>> ItemsList,
       List<Tuple<string, int>> SkillsList, List<Tuple<string, int>> SpecialList, int[][] StageClearData, float bgmVolume, float sfxVolume,
        List<int> DamageUpgradeCount, List<int> CriticalUpgradeCount, List<int> ShieldUpgradeCount)
    {
        string saveJsonPath = SavePath + "Save.json";

       
        SR_SaveData saveData = new SR_SaveData(nickName, score,Coin, Diamond,ItemsList,SkillsList,SpecialList, StageClearData, bgmVolume, sfxVolume,
            DamageUpgradeCount, CriticalUpgradeCount, ShieldUpgradeCount);
        if(!Directory.Exists(SavePath)) { Directory.CreateDirectory(SavePath); }
        string Json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveJsonPath, Json);
        Debug.Log("Save Successed!");
    }
    public SR_SaveData Load()
    {
        string saveJsonPath = SavePath + "Save.json";
        if(!File.Exists(saveJsonPath))
        {
            return null;
        }
        string Json = File.ReadAllText(saveJsonPath);
        SR_SaveData saveData = JsonUtility.FromJson<SR_SaveData>(Json);
        return saveData;
    }
    public void DeleteData()
    {
        string saveJsonPath = SavePath + "Save.json";
        File.Delete(saveJsonPath);
    }


}
