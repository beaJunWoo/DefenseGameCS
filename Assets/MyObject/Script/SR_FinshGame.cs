using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SR_FinshGame : MonoBehaviour
{ 
    const int BossKillCoin = 50;
    const int SkillKillCoin = 200;
    const int SuccessBonus = 500;

    int enemyKillCount = 0;
    int bossEnemyKillCount = 0;
    int skillKillCount = 0;

    int killByCoin = 10;

    public Text[] failedText = new Text[4];
    public Text[] successText = new Text[4];
    public Text Txt_Bonus;
    public void AddEnemyKill() { enemyKillCount++; }
    public void AddBossEnemyKill() { bossEnemyKillCount++; }
    public void AddSkillKill() { skillKillCount++; }
    public int GetScore() { return enemyKillCount * killByCoin + bossEnemyKillCount * BossKillCoin + skillKillCount * SkillKillCoin; }

    public void SetStageClear() { SR_GameManager.instance.SetStageClearData(StarCount()); }

    public  void SetFailedTxt()
    {
        failedText[0].text = "총 처치한 적수: " + enemyKillCount.ToString();
        failedText[1].text = "총 처치한 보스수: " + bossEnemyKillCount.ToString();
        failedText[2].text = "스킬로 죽인 적수: " + skillKillCount.ToString();
        failedText[3].text = "총 점수: " + GetScore();
        SR_GameManager.instance.AddGameMoney(GetScore());
    }

    public void SetSuccessTxt()
    {
        int starCount = StarCount();
        int totalScore = GetScore() + SuccessBonus * starCount;
        successText[0].text = "총 처치한 적수: " + enemyKillCount.ToString();
        successText[1].text = "총 처치한 보스수: " + bossEnemyKillCount.ToString();
        successText[2].text = "스킬로 죽인 적수: " + skillKillCount.ToString();
        successText[3].text = "총 점수: " + (totalScore);
        SR_GameManager.instance.AddGameMoney(totalScore);
        SR_SoundManager.instance.PlaySfx(SR_SoundManager.Sfx.SuccessGame);

        for (int i=0; i< starCount; i++)
        {
            DOTween.Restart((string)("StarOn"+i.ToString()));
        }
        Txt_Bonus.text = string.Format("보너스 {0}X{1}  +{2}", SuccessBonus, starCount, SuccessBonus * starCount);
        SetStageClear();
    }
    int StarCount()
    {

        float hp =GameObject.Find("MilitaryBase").GetComponent<SR_MilitaryBase>().GetHp();
        if (hp > 0.8f)
        {
            return 3;
        }
        else if(hp > 0.5f)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }
}
