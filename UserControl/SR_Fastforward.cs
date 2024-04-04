using UnityEngine;
using UnityEngine.UI;

public class SR_Fastforward : MonoBehaviour
{
    public enum FastType { SLOW, NORMAL, FAST }

    public FastType fastType = FastType.SLOW;
    public Sprite[] sprites;
    public Image image;
    public Text Text;
    public void Start()
    {
        UpdateSpeed();
    }

    public void ChangeFast()
    {
        int Index = ((int)fastType + 1) % 3;
        fastType = (FastType)Index;
        UpdateSpeed();


    }
    public void UpdateSpeed()
    {
        switch (fastType)
        {
            case FastType.SLOW:
                Time.timeScale = 1.0f;
                Text.text = "X 1.0";
                image.sprite = sprites[0];
                break;
            case FastType.NORMAL:
                Time.timeScale = 1.25f;
                Text.text = "X 1.25";
                image.sprite = sprites[1];
                break;
            case FastType.FAST:
                Time.timeScale = 1.5f;
                Text.text = "X 1.5";
                image.sprite = sprites[1];
                break;
        }
    }
}
