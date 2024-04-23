using UnityEngine;
using UnityEngine.UI;

public class SR_Stage : MonoBehaviour
{
    public Text Txt_stage;
    
    public void SetStageText(string stage)
    {
        Txt_stage.text = stage;
    }
}
