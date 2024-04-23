using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_DefanseStart : MonoBehaviour
{
    public GameObject DarkBackGround;
    
    public void StartWarningOn()
    {
        DarkBackGround.SetActive(true);
    }
    public void StartWarningOff()
    {
        DarkBackGround.SetActive(false);
    }
}
