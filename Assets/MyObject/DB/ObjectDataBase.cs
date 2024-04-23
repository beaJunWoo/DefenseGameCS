using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjectDataBase : ScriptableObject
{
    public List<ObjectData> objectsData;
}

[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public String Name { get; private set; }
    [field: SerializeField]
    public int iD { get; private set; }
    [field: SerializeField]

    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public int Price { get; private set; }

}