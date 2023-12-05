using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    private Dictionary<Vector3Int, PlacementData> placedObjects = new Dictionary<Vector3Int, PlacementData>();

    // Data(Dictionary)상의 오브젝트 추가
    // 블럭을 추가로 생성하고 삭제할 수 있는 위치를 파악하기 위함
    public void AddObjectAt(Vector3Int gridPosition, List<Vector3Int> objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell posiiton {pos}");
            }
            placedObjects[pos] = data;
        }
    }

    // 클릭된 그리드 포지션과 오브젝트의 사이즈를 기반으로 오브젝트가 위치하게 되는 그리드 상의 좌표들을 계산
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, List<Vector3Int> objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int i = 0; i < objectSize.Count; i++)
        {
            returnVal.Add((gridPosition + new Vector3Int(objectSize[i].x, 0, objectSize[i].z)));
        }
        return returnVal;
    }

    // 블럭이 놓일 그리드 상의 좌표를 계산한 뒤 그 위치에 놓을 수 있는지 판단
    public bool CanPlaceObjectAt(Vector3Int gridPosition, List<Vector3Int> objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    public int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
        {
            return -1;
        }
        return placedObjects[gridPosition].PlaceObjectIndex;
    }

    // 클릭된 좌표에 블럭이 있다면 Data 상으로 삭제
    public void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

// 그리드 위에 놓여있는 블럭들을 관리하기 위한 데이터 구조
public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlaceObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int id, int placeObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = id;
        PlaceObjectIndex = placeObjectIndex;
    }
}