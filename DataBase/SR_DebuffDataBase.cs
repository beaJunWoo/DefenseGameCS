using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu]
public class SR_DebuffDataBase : ScriptableObject
{
    public List<DebuffData> debuffData;
}

[Serializable]
public class DebuffData
{
    public enum DebuffType { Reloadshorten, DamageBuff, RPMBuff, ArmorBuff, MaxHpBuff, ContinuousHealing, AimAccuracy, HpBuff };
    [field: SerializeField]
    public DebuffType debuffType;

    [field: SerializeField]
    public Sprite img;

    [field: SerializeField]
    public string debuffName;

    [field: SerializeField]
    public string lastName;

    [field: SerializeField]
    public float MaxRandomRange;

    [field: SerializeField]
    public float MinRandomRange;

    [field: SerializeField]
    public float NowDebuffAmount;
}