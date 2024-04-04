using System;
using System.Collections.Generic;
using UnityEngine;

public class SR_SpawnObj : MonoBehaviour
{    
    public GameObject[] ArmyTypeBtns;
    SR_GameManager SR_gameManager;
    private void Start()
    {
        SR_gameManager = GameObject.Find("GameManager").GetComponent<SR_GameManager>();
        List<Tuple<string, int>> ItemsList = SR_gameManager.GetItemList();

        for (int i = 0; i < ArmyTypeBtns.Length; i++)
        {
            if (ItemsList[i].Item2 != 0)
            {
                ArmyTypeBtns[i].SetActive(false);
            }
        }
    }
}
