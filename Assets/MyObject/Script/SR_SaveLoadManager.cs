using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using Firebase.Database;
using System.Threading.Tasks;
using System.Linq;

public class SR_SaveLoadManager : MonoBehaviour
{
    public static SR_SaveLoadManager instance;
    DatabaseReference userData;

    static string SavePath => Application.persistentDataPath + "/SaveFile/";
    private void Awake()
    {
        userData = FirebaseDatabase.DefaultInstance.GetReference("UserData");
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Save(string nickName,int score,int Coin,int Diamond, List<Tuple<string, int>> ItemsList,
       List<Tuple<string, int>> SkillsList, List<Tuple<string, int>> SpecialList, int[][] StageClearData, float bgmVolume, float sfxVolume,
        List<int> DamageUpgradeCount, List<int> CriticalUpgradeCount, List<int> ShieldUpgradeCount)
    {
        //내부 데이터 저장
       

        //서버 데이터 저장
        SR_SaveData saveData = new SR_SaveData(nickName, score, Coin, Diamond, ItemsList, SkillsList, SpecialList, StageClearData, bgmVolume, sfxVolume,
         DamageUpgradeCount, CriticalUpgradeCount, ShieldUpgradeCount);
        string Json = JsonUtility.ToJson(saveData);
        userData.Child(nickName).SetRawJsonValueAsync(Json);

        Debug.Log("Save Successed!");
    }
    public void SaveLoginData(string nickName, string password)
    {
        string saveJsonPath = SavePath + "Save.json";
        if (!Directory.Exists(SavePath)) { Directory.CreateDirectory(SavePath); }
        LoginData loginData = new LoginData(nickName, password);
        string Json = JsonUtility.ToJson(loginData);
        File.WriteAllText(saveJsonPath, Json);
    }
    public LoginData LoadLoginData()
    {
        string saveJsonPath = SavePath + "Save.json";
        if(!File.Exists(saveJsonPath))
        {
            return null;
        }
        string Json = File.ReadAllText(saveJsonPath);
        LoginData saveData = JsonUtility.FromJson<LoginData>(Json);
        return saveData;
    }
  
    public async Task<SR_SaveData> Load(string Key)
    {

        try
        {
            DataSnapshot snapshot = await userData.Child(Key).GetValueAsync();
            string json =  snapshot.GetRawJsonValue();
            SR_SaveData saveData = JsonUtility.FromJson<SR_SaveData>(json);
            Debug.Log("세이브파일 가져오기 성공!");
            Debug.Log(saveData);
            return saveData;
        }
        catch (Exception ex)
        {
            Debug.Log("세이브파일 가져오기 실패: " + ex.Message);
            return null;
        }
    }
    public SR_SaveData DictionaryToJson(DataSnapshot snapshot)
    {
        string nickName = snapshot.Child("nickName").Value.ToString();
        int score = Convert.ToInt32(snapshot.Child("score").Value.ToString());
        int coin = Convert.ToInt32(snapshot.Child("Coin").Value.ToString());
        int diamond = Convert.ToInt32(snapshot.Child("Diamond").Value.ToString());

        List<Tuple<string, int>> itemsList = new List<Tuple<string, int>>();
        foreach (DataSnapshot item in snapshot.Child("ItemsList").Children)
        {
            itemsList.Add(new Tuple<string, int>(item.Key, Convert.ToInt32(item.Value)));
        }

        List<Tuple<string, int>> skillsList = new List<Tuple<string, int>>();
        foreach (DataSnapshot skill in snapshot.Child("SkillsList").Children)
        {
            skillsList.Add(new Tuple<string, int>(skill.Key, Convert.ToInt32(skill.Value)));
        }

        List<Tuple<string, int>> specialList = new List<Tuple<string, int>>();
        foreach (DataSnapshot special in snapshot.Child("SpecialList").Children)
        {
            specialList.Add(new Tuple<string, int>(special.Key, Convert.ToInt32(special.Value)));
        }

        int[][] stageClearData = snapshot.Child("StageClearData").Children
            .Select(child => child.Children.Select(c => Convert.ToInt32(c.Value)).ToArray())
            .ToArray();

        float bgmVolume = float.Parse(snapshot.Child("BgmVolume").Value.ToString());
        float sfxVolume = float.Parse(snapshot.Child("SfxVolume").Value.ToString());

        List<int> damageUpgradeCount = snapshot.Child("DamageUpgradeCount").Children
            .Select(c => Convert.ToInt32(c.Value)).ToList();
        List<int> criticalUpgradeCount = snapshot.Child("CriticalUpgradeCount").Children
            .Select(c => Convert.ToInt32(c.Value)).ToList();
        List<int> shieldUpgradeCount = snapshot.Child("ShieldUpgradeCount").Children
            .Select(c => Convert.ToInt32(c.Value)).ToList();

        return new SR_SaveData(nickName, score, coin, diamond, itemsList, skillsList, specialList, stageClearData, bgmVolume, sfxVolume,
            damageUpgradeCount, criticalUpgradeCount, shieldUpgradeCount);
    }
    public void DeleteData()
    {
        string saveJsonPath = SavePath + "Save.json";
        File.Delete(saveJsonPath);
    }
}
public class LoginData
{
    public string nickName;
    public string password;

    public LoginData(string nickName, string password)
    {
        this.nickName = nickName;
        this.password = password;
    }
}