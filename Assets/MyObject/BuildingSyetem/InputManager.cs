using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnCliked, OnExit;
    public GameObject CousorindicatorParent;
    private void Update()
    { 
      
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended )//&& !IsPointerOverUI())
                OnCliked?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
            OnExit?.Invoke();
    }
    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();// && CousorindicatorParent.activeSelf;
    public Vector3 GetSelectedMapPositon()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        string btnName = null;
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            btnName = EventSystem.current.currentSelectedGameObject.name;
        }
        if (btnName == "Btn_Sell" || btnName == "Btn_Right" || btnName == "Btn_Left" || btnName == "Btn_Move" ||
               btnName == "Btn_RightMove" || btnName == "Btn_LeftMove" || btnName == "Img_ObjContol")
        {
            return lastPosition;
        }
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}