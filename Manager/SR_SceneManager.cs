using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SR_SceneManager : MonoBehaviour
{
    public static SR_SceneManager instance;

    string[] stageNames = { "", "제1 방어기지", "제2 방어기지", "군수물사수송구역", "제3 방어기지" };

    public Text Txt_StageInfo;

    public GameObject[] StageBtns;
    Toggle[][] Tg_Stars = new Toggle[5][];
    public Text[] Txt_StageLv = new Text[3];

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < StageBtns.Length; i++)
        {
            Tg_Stars[i] = new Toggle[3];
            for (int j = 0; j < 3; j++)
            {
                Tg_Stars[i][j] = StageBtns[i].transform.GetChild(j).transform.GetComponent<Toggle>();
            }
        }
    }
    public void LoadScene_StartGame()
    {
        SR_GameManager.instance.SceneLoad("StartGame");
    }
    public void LoadScene_MainMenu()
    {
        if(SR_GameManager.instance.challengeMode)
        {
            LoadScene_RankMenu();
            SR_GameManager.instance.challengeMode = false;
        }else
        {
            SR_GameManager.instance.SceneLoad("MainMenu");
        }
    }
    public void LoadScene_RankMenu()
    {
        SR_GameManager.instance.SceneLoad("Rank");
    }
    public void LoadScene_RankMap()
    {
        SR_GameManager.instance.challengeMode = true;
        SR_GameManager.instance.setStageLv(0);
        SR_GameManager.instance.setStage(4);
        LoadScene_Stage(4);
    }
    public void LoadScene_Stage(int stageLv)
    {
       
        SR_GameManager.instance.setStageLv(stageLv);
        int Stage = SR_GameManager.instance.GetStage();
        SR_GameManager.instance.SceneLoad("Stage");
      
    }
    public void RestartStage()
    {
        int RestartCost = 15;
        if (!SR_GameManager.instance.DecDiamond(RestartCost)) 
        {
            //돈이 부족할때 안내메시지
            return; 
        }

        int Stage = SR_GameManager.instance.GetStage();
        string stageName = "Stage";

        SceneManager.LoadScene(stageName);
    }
    public void setStage(int stage)
    {

        if(stage-1> SR_GameManager.instance.MaxUnLockStage) 
        {
            SR_SoundManager.instance.PlayBtnSfx(SR_SoundManager.BtnSfx.Error);
            return; 
        }
        SR_SoundManager.instance.PlayBtnSfx(SR_SoundManager.BtnSfx.StageOpen);
        SR_GameManager.instance.setStage(stage);
        Txt_StageInfo.text = stageNames[stage];
        DOTween.Restart("StageUIOn");

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (SR_GameManager.instance.getClearInfo(stage, i) > j)
                {
                    Tg_Stars[i][j].isOn = true;
                }
                else
                {
                    Tg_Stars[i][j].isOn = false;
                }
            }
        }

        for(int i=1; i<=3; i++)
        {
            Txt_StageLv[i-1].text = stage + "-" + (i);
        }
    }
    public void SaveData()
    {
        SR_GameManager.instance.Save();
    }
    public void DeleteData()
    {
        SR_SaveLoadManager.instance.DeleteData();
        SR_GameManager.instance.SettingData();
        LoadScene_StartGame();
    }
}
