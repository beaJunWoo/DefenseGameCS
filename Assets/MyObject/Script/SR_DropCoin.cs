using UnityEngine;
using UnityEngine.UI;

public class SR_DropCoin : MonoBehaviour
{
    RectTransform rect;
    Text Txt_damage;
    Image image;
    public float CoinTxtSpeed = 5.0f;
    public float disappearSpeed = 1.0f;

    void Start()
    {
        rect = transform.GetChild(0).GetComponent<RectTransform>();
        Txt_damage = transform.GetChild(0).GetComponent<Text>();
        image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        Invoke("Delete", 1);
    }

    // Update is called once per frame
    void Update()
    {
        {
            Vector3 newPos = rect.transform.position;
            newPos.y += Time.deltaTime * CoinTxtSpeed;
            rect.transform.position = newPos;

            Color newColor = Txt_damage.color;
            newColor.a -= Time.deltaTime * disappearSpeed;
            Txt_damage.color = newColor;

            image.color = newColor;
        }
    }

    void Delete()
    {
        Destroy(gameObject);
    }
}
