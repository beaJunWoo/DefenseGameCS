using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SR_ArmyData : ScriptableObject
{

    public List<ArmyData> armyData;
    public float MaxAdditionalDamage;
    public float MaxCritical;
    public float MaxShield;
}
public enum ArmyType { Gun = 0, Rifle = 1, Sniper = 2, Tank = 3, minigun = 4 }
[Serializable]
public class ArmyData
{
    [field: SerializeField]
    public ArmyType armyType;

    [field: SerializeField]
    public string name;

    [field: SerializeField]
    public Sprite sprite;

    [field: SerializeField]
    public float defaultDamage;

    [field: SerializeField]
    public float additionalDamage;

    [field: SerializeField]
    public float critical;

    [field: SerializeField]
    public float shield;

    [field: SerializeField]
    public int defaultUpgradCost;

    [field: SerializeField]
    public int UpgradCost_addDamage;

    [field: SerializeField]
    public int UpgradCost_critical;

    [field: SerializeField]
    public int UpgradCost_shield;

}
