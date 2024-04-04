using UnityEngine;
using UnityEngine.EventSystems;

public class SR_TurnObj : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;

    public GameObject spawnObj;

    [SerializeField]
    float turnSpeed = 25f;
    void Update()
    {
        if (isRotatingLeft)
        {
            spawnObj.transform.GetChild(0).transform.Rotate(0f, -turnSpeed * Time.deltaTime, 0f);
            spawnObj.transform.GetChild(0).transform.GetChild(0).transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
            Debug.Log("LeftTurn");
        }
        else if (isRotatingRight)
        {
            spawnObj.transform.GetChild(0).transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
            spawnObj.transform.GetChild(0).transform.GetChild(0).transform.Rotate(0f, -turnSpeed * Time.deltaTime, 0f);
            Debug.Log("RightTurn");
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);

        if (eventData.pointerCurrentRaycast.gameObject.name == "Btn_Left")
        {
            isRotatingLeft = true;
            Debug.Log("Left");
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "Btn_Right")
        {
            isRotatingRight = true;
            Debug.Log("Right");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isRotatingLeft = false;
        isRotatingRight = false;
    }

}
