using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SR_EnemyData : ScriptableObject
{
    public List<EnemyData> enemyData;
}

[Serializable]
public class EnemyData
{
    public enum EnemyType { Gun, Rifle, Bomb, Car }
    [field: SerializeField]
    public EnemyType enemyType;

    [field: SerializeField]
    public float Hp;

    [field: SerializeField]
    public float bulletDamage;

    [field: SerializeField]
    public float Speed;

    [field: SerializeField]
    public int max_MagazineCapacity;

    [field: SerializeField]
    public int minRandomCoin;

    [field: SerializeField]
    public int maxRandomCoin;
}
