using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SR_StageSettingDataBase : ScriptableObject
{
    public List<StageData> stageData;
}
[Serializable]
public  class StageData
{
    [field: SerializeField]
    public int Stage;
    [field: SerializeField]
    public int StageLv;

    [field: SerializeField]
    public int DefaultMony;

    [field: SerializeField]
    public float TotalDefenseTime;

    [field: SerializeField]
    public float WaveTime;

    [field: SerializeField]
    public float WaveDuration;

    [field: SerializeField]
    public float[] EnemySpawnTime;

    [field: SerializeField]
    public float[] WaveEnemySpawnTime;
}
