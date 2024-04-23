using System;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public GameObject[] skills;

    public void Start()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SetActive(false);
        }

    }
    public void SetActiveSkills()
    { 
        List<Tuple<string, int>> SkillsList = SR_GameManager.instance.GetSkillList();

        for (int i = 0; i < skills.Length; i++)
        {
            if (SkillsList[i].Item2 == 0)
            {
                skills[i].SetActive(true);
            }
        }
    }

}
