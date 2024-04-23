using System.Collections.Generic;
using UnityEngine;
using static SR_SoundManager;


public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    Grid grid;

    [SerializeField]
    private ObjectDataBase dataBase;
    private int seletedObjectsIndex = -1;

    [SerializeField]
    private SR_ObjPooling SR_objPooling;
    [SerializeField]
    private SR_ObjControl SR_objControl;
    [SerializeField]
    private GameObject gridVisualizetion;

    private GridData floorData, furnitureData;

    private Renderer PreviewRenderer;

    private List<GameObject> placedGameObject = new List<GameObject>();

    [SerializeField]
    GameObject DisappearText;

    NavigationBaker navigation;
    public SR_Coin SR_coin;
    [SerializeField]
    private PreviewSystem preview;


    public GameObject[] StageDefaultObjects;
    public SR_LeftRIghtMove SR_leftRIghtMove;

    bool UiClick = false;

    private Vector3Int lastDetectedPostion = Vector3Int.zero;
    private void Start()
    {
        //StopPlacement();
        floorData = new();
        furnitureData = new();
        navigation = GameObject.Find("Navigation").GetComponent<NavigationBaker>();

        if (SR_GameManager.instance.challengeMode) { return; }
        int StageLv = SR_GameManager.instance.GetStageLv() - 1;
        GameObject[] gameObjects = new GameObject[StageDefaultObjects[StageLv].transform.childCount];

        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i] = StageDefaultObjects[StageLv].transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < gameObjects.Length; i++)
        {
            int Placement = 0;
            Debug.Log(gameObjects[i].name);
            switch (gameObjects[i].name)
            {
                case "ArmyParent":
                    Placement = 0;
                    break;
                case "Army_RifleParent":
                    Placement = 1;
                    break;
                case "Army_SniperParent":
                    Placement = 2;
                    break;
                case "Sandbag_Parent":
                    Placement = 3;
                    break;
                case "Wall_Parent":
                    Placement = 4;
                    break;
            }
            seletedObjectsIndex = Placement;
            Vector3 newPos = new Vector3(gameObjects[i].transform.position.x, 1, gameObjects[i].transform.position.z);

            Vector3Int gridPosition = grid.WorldToCell(newPos);

            gameObjects[i].transform.position = grid.CellToWorld(gridPosition);
            placedGameObject.Add(gameObjects[i]);

            GridData selectedData = dataBase.objectsData[seletedObjectsIndex].iD == -1 ?
            floorData :
            furnitureData;

            selectedData.AddObjectAt(gridPosition,
                dataBase.objectsData[seletedObjectsIndex].Size,
                dataBase.objectsData[seletedObjectsIndex].iD,
                placedGameObject.Count - 1);
            StopPlacement();
        }
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();

        seletedObjectsIndex = dataBase.objectsData.FindIndex(data => data.iD == ID);
        if (seletedObjectsIndex < 0)
        {
            Debug.LogError($"No ID Found{ID}");
            return;
        }
        gridVisualizetion.SetActive(true);
        preview.StartShowingPlacementPreview(
            dataBase.objectsData[seletedObjectsIndex].Prefab,
            dataBase.objectsData[seletedObjectsIndex].Size);
        Invoke("StartEvent", 0.1f);
    }
    public void StartEvent()
    {
        inputManager.OnCliked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    private void PlaceStructure()
    {
      
        if (SR_coin.GetCoin() - dataBase.objectsData[seletedObjectsIndex].Price < 0)    
        { StopPlacement();
            GameObject Text=Instantiate(DisappearText);
            Text.transform.GetChild(0).GetComponent<RectTransform>().transform.position = Camera.main.WorldToScreenPoint(inputManager.GetSelectedMapPositon());
            Debug.Log("설치 중지1");
            return; 
        }
       
       //자금이 부족하면 중단
       //if (inputManager.IsPointerOverUI())
       //{
       //    if(inputManager.IsPointerOverUI())
       //    { Debug.Log("1번문제"); }
       // 
       //    Debug.Log("설치 중지2");
       //    return;
       //}
          
        //움직이거나 UI를 건들이면 멈춤
        Vector3 mousePosition = inputManager.GetSelectedMapPositon();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);


        bool placementValidity = CheckPlacementValidity(gridPosition, seletedObjectsIndex);

        if(placementValidity == false)
        {
            StopPlacement();
            Debug.Log("설치 중지3");
            return;
        }
        //놓을수 없는 공간이면 멈춤

        GameObject newObject = Instantiate(dataBase.objectsData[seletedObjectsIndex].Prefab);
        bool canMove = navigation.CheckNavigation();
        if (!canMove)
        {
            Destroy(newObject);
            StopPlacement();
            Debug.Log("설치 중지4");
            return;
        }

        //목적지 까지 이동이 불가능하면 중단
        newObject.GetComponentInChildren<SR_PlayerObj>().Initialize();
        newObject.GetComponentInChildren<SR_PlayerObj>().SetPrice(dataBase.objectsData[seletedObjectsIndex].Price);
        SR_coin.DeductionCoin(dataBase.objectsData[seletedObjectsIndex].Price);

        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObject.Add(newObject);

        GridData selectedData = dataBase.objectsData[seletedObjectsIndex].iD == -1 ?
        floorData :
        furnitureData;

        selectedData.AddObjectAt(gridPosition,
            dataBase.objectsData[seletedObjectsIndex].Size,
            dataBase.objectsData[seletedObjectsIndex].iD,
            placedGameObject.Count - 1);
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
        SR_objControl.ShowObjControl(newObject);

        SR_SoundManager.instance.PlaySfx((SR_SoundManager.Sfx)Random.Range((int)Sfx.Placement1, (int)Sfx.Placement4));
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int seletedObjectsIndex)
    {
        if (seletedObjectsIndex < 0) { return false; }
        if (gridPosition.z<4) { return false; }
        GridData selectedData = dataBase.objectsData[seletedObjectsIndex].iD == -1 ?
        floorData :
        furnitureData;

        return selectedData.CanPlaceObejctAt(gridPosition, dataBase.objectsData[seletedObjectsIndex].Size);
    }

    public void StopPlacement()
    {
        seletedObjectsIndex = -1;
        gridVisualizetion.SetActive(false);
        preview.StopShowingPerview();
        inputManager.OnCliked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPostion = Vector3Int.one;

    }
    public void RemoveObject(Vector3Int ObjectPos,GameObject ArmyObj)
    {
        int Placement=0;
        Debug.Log(ArmyObj.name);
        switch (ArmyObj.name)
        {
            case "ArmyParent(Clone)":
                Placement = 0;
                break;
            case "Army_RifleParent(Clone)":
                Placement = 1;
                break;
            case "Army_SniperParent(Clone)":
                Placement = 2;
                break;
            case "Sandbag_Parent(Clone)":
                Placement = 3;
                break;
            case "Wall_Parent(Clone)":
                Placement = 4;
                break;
        }
        
        furnitureData.RemoveObjectAt(ObjectPos, dataBase.objectsData[Placement].Size);
    }

    private void Update()
    {

        if (seletedObjectsIndex < 0)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPositon();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
       
        if (Input.touchCount == 1)
        {
            if (!gridVisualizetion.activeSelf) { return; }
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                preview.StartShowingPlacementPreview(
              dataBase.objectsData[seletedObjectsIndex].Prefab,
              dataBase.objectsData[seletedObjectsIndex].Size);
            }else if(touch.phase == TouchPhase.Moved)
            {
                if (lastDetectedPostion != gridPosition)
                {
                    bool placementValidity = CheckPlacementValidity(gridPosition, seletedObjectsIndex);

                    mouseIndicator.transform.position = mousePosition;
                    preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
                    lastDetectedPostion = gridPosition;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                preview.StopShowingPerview();
            }
        }

    }

}