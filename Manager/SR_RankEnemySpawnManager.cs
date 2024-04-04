using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SR_RankEnemySpawnManager : MonoBehaviour
{
    public Transform EnemySpawnPos;
    public SR_ObjPooling SR_objPooling;
    public float[] enemySpawnTime = new float[3] { 5.0f, 10.0f, 50.0f };
    float[] nowTime = new float[3] { 0.0f, 0.0f, 0.0f };
    float NextWaveTime = 20.0f;

    public float time = 0;
    public int Wave = 1;
    public GameObject ProgressSlider;
    public Text[] Txt_TotalTime = new Text[2];
    public Text[] Txt_Score = new Text[2];

    public int Score;
    int CarPriorityNum;
    int enemyPriorityNum;
    bool startGame = false;
    void Start()
    {
        if (SR_GameManager.instance.challengeMode) { ProgressSlider.SetActive(false); }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!startGame) { return; }
        if (!SR_GameManager.instance.challengeMode) { return; }
        time += Time.deltaTime;
        int resultMin = Mathf.FloorToInt(time / 60);
        int resultSec = Mathf.FloorToInt(time % 60);
        Txt_TotalTime[0].text = string.Format("생존시간 {0:D2}:{1:D2}", resultMin, resultSec);
        Txt_TotalTime[1].text = string.Format("생존시간 {0:D2}:{1:D2}", resultMin, resultSec);

        Txt_Score[0].text = string.Format("점수 : {0}", Score);
        Txt_Score[1].text = string.Format("점수 : {0}", Score);
        SpwanEnemy(enemySpawnTime);
    }
    void SpwanEnemy(float[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            nowTime[i] += Time.deltaTime;
            if (nowTime[i] >= arr[i])
            {
                GameObject enemy = SR_objPooling.GetObj(SR_ObjPooling.PoolingType.Enemy, i);
                if (enemy != null)
                {
                    Vector3 newPos = EnemySpawnPos.position;
                    newPos.x += Random.Range(0.0f, 15.0f);
                    newPos.z += Random.Range(-15.0f, 15.0f);
                    enemy.transform.position = newPos;

                    if (i == 2)
                    {
                        enemy.GetComponent<SR_Enemy>().Initialize(CarPriorityNum);
                        CarPriorityNum++;
                        if (CarPriorityNum == 49) { CarPriorityNum = 0; }
                    }
                    else
                    {
                        enemy.GetComponent<SR_Enemy>().Initialize(enemyPriorityNum);
                        enemyPriorityNum++;
                        if (enemyPriorityNum == 99) { enemyPriorityNum = 50; }
                    }
                }
                nowTime[i] = 0.0f;
            }
        }
    }
    public void GameStart()
    {
        startGame = true;
        if (!SR_GameManager.instance.challengeMode) { return; }
        StartCoroutine(ScoreCorutine());
        StartCoroutine(NextWaveCorutine());
    }
    IEnumerator ScoreCorutine()
    {
        int i = 0;
        while (true)
        {
            Score +=  Wave*(i);
            i++;
            yield return new WaitForSeconds(1.0f);
        }
    }
    IEnumerator NextWaveCorutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(NextWaveTime);
            Wave++;
            for (int i=0; i< enemySpawnTime.Length; i++)
            {
                enemySpawnTime[i] *= 0.75f;
            }
        }
    }
}
