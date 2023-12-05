using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BlockDatabaseSO : ScriptableObject
{
    public List<BlockData> blockData;
}

[Serializable]
public class BlockData
{
    [field: SerializeField] public string Name { get; private set; }    // 필요없다면 삭제
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public List<Vector3Int> Size { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public List<int> Price;     // [10]
    [field: SerializeField] public List<int> Effect;    // [7]
    [field: SerializeField] public int Turn;

    /*
    public int CardNum;
    public List<int> Price; //[10]
    public List<int> Effect; //[7]
    public int Turn;
    public int Slot;
     */
}

