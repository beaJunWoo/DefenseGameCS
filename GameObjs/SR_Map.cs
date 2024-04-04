using UnityEngine;
using UnityEngine.UI;

public class SR_Map : MonoBehaviour
{
    const int NOT_CLEAR = 0;

    [SerializeField]
    GameObject[] StageButtons;

    public Sprite Img_Unlock;
    public Sprite Img_Lock;
    void Awake()
    {
        GameObject childGameObject = gameObject.transform.GetChild(0).gameObject;
        int childGameObjectChildCount = childGameObject.transform.childCount;
        StageButtons = new GameObject[childGameObjectChildCount];
        for (int i=0; i< childGameObjectChildCount; i++)
        {
            StageButtons[i] = childGameObject.transform.GetChild(i).gameObject;
        }
    }
    void Start()
    {
        int[][] StageClearData =SR_GameManager.instance.GetStageClearData();
        int MaxUnLockStage = GetMaxUnLockStage(StageClearData);
        SR_GameManager.instance.MaxUnLockStage = MaxUnLockStage;
        for(int i=0; i<StageButtons.Length; i++)
        {
            StageButtons[i].GetComponent<Image>().sprite = i -1 < MaxUnLockStage ? Img_Unlock : Img_Lock;
        }
    }
    int GetMaxUnLockStage(int[][] StageClearData)
    {
        int unLockStage = 0;
        for (int stage = 1; stage < StageClearData.Length; stage++)
        {
            for (int stageLv = 1; stageLv < StageClearData[stage].Length; stageLv++)
            {
                if (StageClearData[stage][stageLv] == NOT_CLEAR)
                {
                    return unLockStage;
                }
   
            }
            unLockStage++;
        }
        return unLockStage;
    }
}
