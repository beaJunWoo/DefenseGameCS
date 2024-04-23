using UnityEngine;
using UnityEngine.UI;

public class SR_DisappearText : MonoBehaviour
{

    [SerializeField] RectTransform rect;
    [SerializeField] Text text;
    [SerializeField] Image image;
    public float TxtSpeed = 5.0f;
    public float disappearSpeed = 1.0f;
    public float DestroyTime = 1.0f;

    Vector3 newPos;
    Color newColor;
    void Start()
    {
        Invoke("Delete", DestroyTime);
    }

    void Update()
    {
        newPos = rect.transform.position;
        newPos.y += Time.deltaTime * TxtSpeed;
        rect.transform.position = newPos;

        newColor = text.color;
        newColor.a -=Time.deltaTime * disappearSpeed;
        text.color = newColor;

        if(image != null )
        {
            image.color = newColor;
        }
       
    }
    void Delete()
    {
        Destroy(gameObject);
    }
}
