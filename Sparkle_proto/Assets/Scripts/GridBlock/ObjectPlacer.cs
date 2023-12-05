using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> placedGameObjects = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newBlock = Instantiate(prefab);
        newBlock.transform.position = position;
        placedGameObjects.Add(newBlock);
        return placedGameObjects.Count - 1;
    }

    public void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null) 
            return;
        
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}
