using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SR_EnemySpawnManager : MonoBehaviour
{
    public Transform EnemySpawnPos;

    public float[] enemySpawnTime = new float[3] { 20.0f, 50.0f, 100.0f };
    public float[] WaveEnemySpawnTime = new float[3] { 10.0f, 30.0f, 50.0f };
    float[] nowTime = new float[3] { 0.0f, 0.0f, 0.0f };

    public float totalDefenseTime = 180.0f;
    public float WaveTime = 60.0f;
    public float WaveDuration = 20.0f;
    public float WaveCountDown = 0.0f;
    public bool isWaveTime;
    public float time = 20;

    [Header("ProgressBar")]
    public RectTransform[] WavePoint;
    public RectTransform Trf_RoundProgressbar;
    public Slider Slider_RoundProgressbar;

    public Text[] Txt_TotalTime = new Text[2];
    public Text[] Txt_NextWaveTime = new Text[2];

    int WaveCount = 0;
    float NextDebuffTime;

    public bool FinshGame = false;
    public bool startGame = false;
    SR_ObjPooling SR_objPooling;
    Color defaultColor;

    int CarPriorityNum = 0;
    int enemyPriorityNum = 50;
    private void Awake()
    {
        if (SR_GameManager.instance.challengeMode)
        { return; }

        int DataBaseIdx = SR_GameManager.instance.GetStageLv() - 1 + (SR_GameManager.instance.GetStage() - 1) * 3;
        totalDefenseTime = SR_GameManager.instance.stageSettingData.stageData[DataBaseIdx].TotalDefenseTime;
        WaveTime = SR_GameManager.instance.stageSettingData.stageData[DataBaseIdx].WaveTime;
        WaveDuration = SR_GameManager.instance.stageSettingData.stageData[DataBaseIdx].WaveDuration;

        enemySpawnTime = (float[])SR_GameManager.instance.stageSettingData.stageData[DataBaseIdx].EnemySpawnTime.Clone();
        WaveEnemySpawnTime = (float[])SR_GameManager.instance.stageSettingData.stageData[DataBaseIdx].WaveEnemySpawnTime.Clone();

    }
    private void Start()
    {
        if (SR_GameManager.instance.challengeMode)
        { return; }
        SR_objPooling =GameObject.Find("ObjPooling").GetComponent<SR_ObjPooling>();
         defaultColor = Txt_NextWaveTime[0].color;
         NextDebuffTime = WaveTime;

        //int i = 1;
        float wave = WaveTime;

        for (int i = 0; i < WavePoint.Length; i++)
        {

            if ((WaveTime + WaveDuration) * (i+1) > totalDefenseTime)
            {
                WavePoint[i].gameObject.SetActive(false);
            }
            else
            {
                float totalWaves = wave / totalDefenseTime;
                Debug.Log("totalWaves :" + totalWaves);
                Debug.Log("Trf_RoundProgressbar.sizeDelta.x :" + Trf_RoundProgressbar.sizeDelta.x);
                WavePoint[i].anchoredPosition = new Vector2(totalWaves * Trf_RoundProgressbar.sizeDelta.x, 0);
                wave += WaveTime + WaveDuration;
            }
        }
       
    }
  
    void Update()
    {
        if (SR_GameManager.instance.challengeMode)
        { return; }
        Slider_RoundProgressbar.value = time/totalDefenseTime;
        

        int resultMin = Mathf.FloorToInt(WaveCountDown / 60);
        int resultSec = Mathf.FloorToInt(WaveCountDown % 60);

        if (time > NextDebuffTime)
        {
            NextDebuffTime += WaveTime + WaveDuration;
            GameObject.Find("Cvs_RandomDebuff").GetComponent<SR_RandomDebuff>().StartDebuff();

            for (int i = 0; i < enemySpawnTime.Length; i++)
            {
                enemySpawnTime[i] *= 0.8f;
                WaveEnemySpawnTime[i] *= 0.8f;
            }

        }

        if (totalDefenseTime > time)
        {
            if (isWaveTime)
            {
                Txt_NextWaveTime[0].text = string.Format("웨이브 종료 까지 {0:D2}:{1:D2}", resultMin, resultSec);
                Txt_NextWaveTime[1].text = string.Format("웨이브 종료 까지 {0:D2}:{1:D2}", resultMin, resultSec);

            }
            else
            {
                Txt_NextWaveTime[0].text = string.Format("다음 웨이브 까지 {0:D2}:{1:D2}", resultMin, resultSec);
                Txt_NextWaveTime[1].text = string.Format("다음 웨이브 까지 {0:D2}:{1:D2}", resultMin, resultSec);
            }
            Txt_NextWaveTime[0].color = WaveCountDown <= 10 ? Color.red : defaultColor;
        }
        else
        {
            GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");

            int activeEnemyCount = 0;
            for (int i = 0; i < Enemys.Length; i++)
            {
                if (Enemys[i].activeSelf && Enemys[i].GetComponent<SR_Enemy>().GetIs_Active()) { activeEnemyCount++; }
            }

            if (activeEnemyCount > 0)
            {
                Txt_NextWaveTime[0].text = string.Format("남은 적수:" + activeEnemyCount);
                Txt_NextWaveTime[1].text = string.Format("남은 적수:" + activeEnemyCount);
            }
            else
            {
                Txt_NextWaveTime[0].text = string.Format("미션 성공!");
                Txt_NextWaveTime[1].text = string.Format("미션 성공!");
                if (!FinshGame)
                {
                    FinshGame = true;
                    DOTween.Restart("Success");
                }
            }


        }


        float TotalWaveTime = totalDefenseTime - time;
        if (!FinshGame && totalDefenseTime <= time)
        {
            Txt_TotalTime[0].text = string.Format("남은적을 모두 소탕하세요!");
            Txt_TotalTime[1].text = string.Format("남은적을 모두 소탕하세요!");
        }
        else
        {
            if (FinshGame)
            {
                Txt_TotalTime[0].text = string.Format("모든적을 제거했습니다.");
                Txt_TotalTime[1].text = string.Format("모든적을 제거했습니다.");
            }
            else
            {
                int totalMin = Mathf.FloorToInt(TotalWaveTime / 60);
                int totalSec = Mathf.FloorToInt(TotalWaveTime % 60);
                Txt_TotalTime[0].text = string.Format("남은 방어시간 : {0:D2}:{1:D2}", totalMin, totalSec);
                Txt_TotalTime[1].text = string.Format("남은 방어시간 : {0:D2}:{1:D2}", totalMin, totalSec);
            }

        }

        if (startGame)
        {
            if (totalDefenseTime >= time)
            {
                time += Time.deltaTime;
                if (!isWaveTime && time % WaveTime > 0 && time % WaveTime < WaveDuration && time > WaveTime)
                {
                    DOTween.Restart("BounceTime");
                }
                if (isWaveTime && !(time % WaveTime > 0 && time % WaveTime < WaveDuration && time > WaveTime))
                { DOTween.Pause("BounceTime"); }
                isWaveTime = time % WaveTime > 0 && time % WaveTime < WaveDuration && time > WaveTime;
                if (isWaveTime)
                {
                    WaveCountDown = WaveDuration - time % WaveTime;
                    SpwanEnemy(WaveEnemySpawnTime);
                    if (WaveDuration == 0)
                    {
                        WaveCount++;
                        WaveDuration += 5;
                        WaveTime -= 5;
                    }
                }
                else
                {
                    WaveCountDown = WaveTime - time % WaveTime;
                    SpwanEnemy(enemySpawnTime);
                }
            }
        }

    }
    void SpwanEnemy(float[] arr)
    {
        if (totalDefenseTime - time < 10.0f) { return; }
        for (int i = 0; i < arr.Length; i++)
        {
            nowTime[i] += Time.deltaTime;
            if (nowTime[i] >= arr[i])
            {
                GameObject enemy = SR_objPooling.GetObj(SR_ObjPooling.PoolingType.Enemy,i);
                if(enemy != null)
                {
                    Vector3 newPos = EnemySpawnPos.position;
                    newPos.x += Random.Range(0.0f, 15.0f);
                    newPos.z += Random.Range(-15.0f, 15.0f);
                    enemy.transform.position = newPos;
                  
                    if(i==2)
                    {
                        enemy.GetComponent<SR_Enemy>().Initialize(CarPriorityNum);
                        CarPriorityNum++;
                        if (CarPriorityNum == 49) { CarPriorityNum = 0; }
                    }
                    else
                    {
                        enemy.GetComponent<SR_Enemy>().Initialize(enemyPriorityNum);
                        enemyPriorityNum++;
                        if (enemyPriorityNum == 99) { enemyPriorityNum =50; }
                    }
                }
                nowTime[i] = 0.0f;
            }
        }
    }
    public bool GetFinshGame()
    {
        return FinshGame;
    }
    public void StartEnemySpawn()
    {
        startGame = true;
    }
}
