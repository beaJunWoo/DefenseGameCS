using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition,
                            Vector2Int objectSize,
                            int ID,
                            int placedobjectIndex)
    {
        Debug.Log(gridPosition.x+","+gridPosition.y + "," + gridPosition.z);
        List<Vector3Int> positionToOccupy = CalulatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedobjectIndex);
        foreach(var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                throw new Exception($"Dicitonary already contains this cell positoin{pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalulatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }
    public bool CanPlaceObejctAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positonToOccupy = CalulatePositions(gridPosition, objectSize);
        foreach (var pos in positonToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }
    public bool RemoveObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
       
        // 해당 위치에서 객체 데이터 가져오기
        PlacementData data = placedObjects[gridPosition];
        List<Vector3Int> positionToOccupy = CalulatePositions(gridPosition, objectSize);
        // 객체가 차지하고 있는 모든 위치를 딕셔너리에서 제거
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                placedObjects.Remove(pos);
            }
            else
            {
                Debug.LogError($"Trying to remove an object at {pos}, but it's not found in the dictionary.");
            }
        }

        return true;
    }
}
public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}