using UnityEngine;
using UnityEngine.EventSystems;

public class SR_LeftRIghtMove : MonoBehaviour
{
    Transform TF_MainCamera;
    Camera mainCamera;
    public int LeftMax;
    public int RightMax;
    public float CameraSpeed = 1.5f;

    public bool isMoving = false;
    string OnName;
    private void Start()
    {
        TF_MainCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject)
        {
            OnName = EventSystem.current.currentSelectedGameObject.name;
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnName = null;
            if (isMoving) { Invoke("SetIsnotMoving", 0.5f); }
        }
        CameraMove();
    }
    void CameraMove()
    {
        if (OnName == "Btn_LeftMove")
        {
            float newX = Mathf.Clamp(TF_MainCamera.position.x - Time.deltaTime * CameraSpeed, LeftMax, RightMax);
            Vector3 newPos = new Vector3(newX, TF_MainCamera.position.y, TF_MainCamera.position.z);
            TF_MainCamera.transform.position = newPos;
            isMoving = true;

        }
        if (OnName == "Btn_RightMove")
        {
            float newX = Mathf.Clamp(TF_MainCamera.position.x + Time.deltaTime * CameraSpeed, LeftMax, RightMax);
            Vector3 newPos = new Vector3(newX, TF_MainCamera.position.y, TF_MainCamera.position.z);
            TF_MainCamera.transform.position = newPos;
            isMoving = true;
        }
        
    }
    void SetIsnotMoving()
    {
        isMoving = false;
    }
}
