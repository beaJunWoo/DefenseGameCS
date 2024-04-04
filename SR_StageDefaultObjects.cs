using UnityEngine;

public class SR_StageDefaultObjects : MonoBehaviour
{
    public GameObject[] StageDefaultObjects;
    void Awake()
    {
        int stageLv = SR_GameManager.instance.GetStageLv();
        for(int i=0; i<StageDefaultObjects.Length; i++)
        {
            if (stageLv-1 == i)
            {
                StageDefaultObjects[i].SetActive(true);
            }
            else { StageDefaultObjects[i].SetActive(false);}
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
