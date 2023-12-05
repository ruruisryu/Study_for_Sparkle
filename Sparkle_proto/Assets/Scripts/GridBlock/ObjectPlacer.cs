using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> placedGameObjects = new List<GameObject>();

    // 블럭(게임 오브젝트)를 해당하는 위치에 생성한다.
    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newBlock = Instantiate(prefab);
        newBlock.transform.position = position;
        placedGameObjects.Add(newBlock);
        return placedGameObjects.Count - 1;
    }

    // 해당 위치에 존재하는 블럭(게임 오브젝트)를 제거한다.기
    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null) 
            return;
        
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}
