using UnityEngine;

public class SR_MapDesign : MonoBehaviour
{
    public GameObject[] StageMapDesigns;
    private void Start()
    {
        int Stage =SR_GameManager.instance.GetStage() - 1;
        Instantiate(StageMapDesigns[Stage]);
    }
}
