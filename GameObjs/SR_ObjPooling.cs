using Unity.Burst.Intrinsics;
using UnityEngine;

public class SR_ObjPooling : MonoBehaviour
{
    public enum PoolingType { Enemy, ArmyBullet, EnemyBullet };


    public GameObject[] Enemys = new GameObject[3];
    GameObject[][] PoolingEnemys = new GameObject[3][];

    public GameObject ArmyBullet;
    GameObject[] poollingArmyBullet = new GameObject[20];

    public GameObject EnemyBullet;
    GameObject[] poollingEnemyBullet = new GameObject[20];

    private void Awake()
    {
        InitObj(PoolingEnemys, Enemys);
        InitObj(poollingArmyBullet, ArmyBullet);
        InitObj(poollingEnemyBullet, EnemyBullet);
    }
    void InitObj(GameObject[][] PollingObj, GameObject[] obj)
    {
        for (int i = 0; i < PollingObj.Length; i++)
        {
            PollingObj[i] = new GameObject[40];
            for (int j = 0; j < PollingObj[i].Length; j++)
            {
                PollingObj[i][j] = Instantiate(obj[i]);
                PollingObj[i][j].SetActive(false);
            }
        }
    }
    void InitObj(GameObject[] PoolingObj, GameObject obj)
    {
        for (int i = 0; i < PoolingObj.Length; i++)
        {
            PoolingObj[i] = Instantiate(obj);
            PoolingObj[i].SetActive(false);
        }
    }

   
    public GameObject GetObj(PoolingType type, int idx)
    {
        GameObject[][] PollingObj =null;

        switch(type)
        {
            case PoolingType.Enemy:
                PollingObj = PoolingEnemys;
                break;
        }
        return FindObj(PollingObj, idx);
    }

    public GameObject GetObj(PoolingType type)
    {
        GameObject[] PollingObj = null;
        switch (type)
        {
            case PoolingType.ArmyBullet:
                PollingObj = poollingArmyBullet;
                break;
            case PoolingType.EnemyBullet:
                PollingObj = poollingEnemyBullet;
                break;
        }
        return FindObj(PollingObj);
    }
    GameObject FindObj(GameObject[][] poolingObj, int idx)
    {
        for (int i = 0; i < poolingObj[idx].Length; i++)
        {
            if (!poolingObj[idx][i].activeSelf)
            {
                poolingObj[idx][i].SetActive(true);
                poolingObj[idx][i].transform.position = new Vector3(100, 0, 0);
                
                return poolingObj[idx][i];
            }
        }
        return null;
    }
    GameObject FindObj(GameObject[] pollingObj)
    {
        for(int i=0; i< pollingObj.Length; i++)
        {
            if (!pollingObj[i].activeSelf)
            {
                pollingObj[i].SetActive(true);
                return pollingObj[i];
            }
        }
        return null;
    }
}
