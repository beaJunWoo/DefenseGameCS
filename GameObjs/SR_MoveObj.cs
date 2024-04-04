using UnityEngine;
using UnityEngine.EventSystems;
public class SR_MoveObj : MonoBehaviour, IDragHandler,IEndDragHandler,IBeginDragHandler
{
    public SR_ObjControl SR_objControl;
    public GameObject ControlImage;
    public float z = 0.0f;
    
    public float MaxZ;
    public float MinZ;
    NavigationBaker navigation;

    Vector3 savePos;
    private void Start()
    {
        navigation = GameObject.Find("Navigation").GetComponent<NavigationBaker>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("옮기는중");
        Transform spawnObjPos = SR_objControl.GetSpawnObjPos();
        if (spawnObjPos != null)
        {
            Debug.Log("있음");
            Ray CuserPos = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(CuserPos, out hit, 200f))
            {
                Vector3 hitPos = hit.point;
                hitPos.y -= SR_objControl.posZ + z;
                hitPos.y = 0.1f;
                hitPos.z -= SR_objControl.posZ;
            
                if (MaxZ <= hitPos.z)
                {
                    hitPos.z = MaxZ;
                }
                if (MinZ >= hitPos.z)
                {
                    hitPos.z = MinZ;
                }
            
                spawnObjPos.transform.position = hitPos;
                ControlImage.transform.position = Input.mousePosition;
            }
          
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        savePos = SR_objControl.GetSpawnObjPos().position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        bool canMove = navigation.CheckNavigation();
        if (!canMove)
        {
            Transform spawnObjPos = SR_objControl.GetSpawnObjPos();
            spawnObjPos.transform.position = savePos;
        }

    }


}
