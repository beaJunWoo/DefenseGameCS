using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class SR_MilitaryBase : MonoBehaviour
{
    
    public float maxHp = 100;
    public float nowHp;
    float hp;

    public Slider hpBar;

    bool is_Active = true;
    void Start()
    {
        nowHp = maxHp;
        hpBar.value = nowHp/maxHp;
    }
    void Update()
    {
        hp = nowHp/ maxHp;
        HpPercent(); 
        if (nowHp <= 0 && is_Active) 
        {
            if (SR_GameManager.instance.challengeMode)
            {
                int Score =GameObject.Find("EnemySpawnManager").GetComponent<SR_RankEnemySpawnManager>().Score;
                SR_GameManager.instance.bNewSocre = true;
                SR_GameManager.instance.SetScore(Score);
            }
            DOTween.Restart("FailedAni");
            is_Active = false; 
        }
    }
    private void HpPercent()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, hp, Time.deltaTime * 10);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Attacked(1);
        }
    }
    public void Attacked(int damage)
    {
        nowHp -= damage;
    }
    public void StopTime()
    {
        Time.timeScale = 0f;
    }
    public void StartTime()
    {
        Time.timeScale = 1f;
    }
    public float GetHp() { return hp; }
}
