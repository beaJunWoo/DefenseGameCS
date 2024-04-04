using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SR_SkillsData : ScriptableObject
{
    public List<SkillsData> skillsData;
}
[Serializable]
public class SkillsData
{
    public enum SkillType {MISSILE, MINIGUN };
    [field: SerializeField]
    public SkillType skillType;

    [field: SerializeField]
    public Sprite skill_img;

    [field: SerializeField]
    public float SkillCooltime;

    [field: SerializeField]
    public int defaultSkillCost;

    [field: SerializeField]
    public int SkillCost;

}
