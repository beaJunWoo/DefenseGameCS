using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SR_ObjControl : MonoBehaviour
{
    public GameObject replaceUI;
    public GameObject sellUI;
    public GameObject CoursorindicatorParent;
    public PlacementSystem placementSystem;
    Camera Maincamera;
    GameObject spawnObj;

    SR_Coin SR_coin;

    public SR_TurnObj SR_trunLeft;
    public SR_TurnObj SR_turnRight;

    public Toggle Tg_CameraSetting;
    public float NearPosZ = -2.0f;
    public float FarPosZ = -2.0f;
    public float posZ;


    void Start()
    {
        Maincamera = Camera.main;
        SR_coin = GameObject.Find("Cvs_Coin").GetComponent<SR_Coin>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            string btnName = null;
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                btnName = EventSystem.current.currentSelectedGameObject.name;
            }
            Ray ray = Maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.Log(btnName);
            if (btnName == "Btn_Sell" || btnName == "Btn_Right"|| btnName == "Btn_Left" || btnName == "Btn_Move" || 
                btnName == "Btn_RightMove" || btnName == "Btn_LeftMove" || btnName == "Img_ObjContol")
            {
                return;
            }
            else if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("ParentObj")))
            {
                if (CoursorindicatorParent.activeSelf) {  return; }

                if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("ParentObj"))&& hit.collider.name!="MilitaryBase")
                {
                    string name = hit.collider.name;

                    ShowObjControl(hit.collider.gameObject);
                    posZ = Tg_CameraSetting.isOn ? FarPosZ:NearPosZ;

                }
            }
            else
            {
                replaceUI.SetActive(false);
                sellUI.SetActive(false);
                if(spawnObj != null)
                {
                    if (spawnObj.transform.GetChild(0).GetComponent<SR_Army>() != null)
                        spawnObj.transform.GetChild(0).GetComponent<SR_Army>().UpdateAttackRangeColor(0.2f);
                }
            }
        }
       
    }
    public void ShowObjControl(GameObject obj)
    {
        if (spawnObj != null)
        {
            if (spawnObj.transform.GetChild(0).GetComponent<SR_Army>() != null)
                spawnObj.transform.GetChild(0).GetComponent<SR_Army>().UpdateAttackRangeColor(0.2f);
        }
        spawnObj = obj;
        SR_trunLeft.spawnObj = spawnObj;
        SR_turnRight.spawnObj = spawnObj;
        replaceUI.SetActive(true);
        sellUI.SetActive(true);
        if (spawnObj != null)
        {
            if (spawnObj.transform.GetChild(0).GetComponent<SR_Army>() != null)
                spawnObj.transform.GetChild(0).GetComponent<SR_Army>().UpdateAttackRangeColor(1.0f);
        }
    }
    public void SellObj()
    {
        int price = spawnObj.transform.GetChild(0).GetComponent<SR_PlayerObj>().GetPrice();
        SR_coin.AddCoin(price);
        Invoke("UIOff", 0.1f);
    }
    public void UIOff()
    {
        placementSystem.RemoveObject(new Vector3Int((int)(spawnObj.transform.position.x * 0.5f), (int)(spawnObj.transform.position.y * 0.5f), (int)(spawnObj.transform.position.z * 0.5f + 1)), spawnObj);
        Destroy(spawnObj);
        replaceUI.SetActive(false);
        sellUI.SetActive(false);
    }

    public Transform GetSpawnObjPos() { return spawnObj.transform; }
    public GameObject GetSpawnObj() { return spawnObj; }

    
}
