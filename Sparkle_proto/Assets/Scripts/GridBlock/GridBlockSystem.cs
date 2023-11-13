using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlockSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private Grid grid;
    [SerializeField] private BlockDatabaseSO databases;

    private int selectedBlockIndex = -1;
    [SerializeField] private GameObject gridVisualization;
    private GridData floorData, blockData;
    private Renderer preveiwRenderer;
    private List<GameObject> placedGameObjects = new List<GameObject>();

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        blockData = new GridData();
        preveiwRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        // FindIndex에 인자로 들어갈 predicate를 람다식으로 건네준 것
        selectedBlockIndex = databases.blockData.FindIndex(data => data.ID == id);
        if (selectedBlockIndex < 0)
        {
            Debug.LogError($"No ID found {id}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        InputManager.instance.OnClicked += PlaceStructure;
        InputManager.instance.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (InputManager.instance.IsPointerOverUI())
        {
            return;
        }
        // 마우스로 클릭한 부분에 해당하는 GridPosition을 계산한다.
        Vector3 mousePos = InputManager.instance.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        
        // 그리드 밖을 클릭했다면 return
        bool isInGrid = mousePos.x != Vector3.negativeInfinity.x;
        if (!isInGrid)
        {
            return;
        }
        // 클락한 위치에 블럭을 놓을 수 있는지 판단하고 블럭을 놓는다.
        bool placementValidity = CheckPlacementValidity(gridPos, selectedBlockIndex);
        if (placementValidity == false)
        {
            return;
        }
        GameObject newBlock = Instantiate(databases.blockData[selectedBlockIndex].Prefab);
        newBlock.transform.position = grid.CellToWorld(gridPos);
        placedGameObjects.Add(newBlock);
        GridData selectedData = databases.blockData[selectedBlockIndex].ID == 0 ? floorData : blockData;
        selectedData.AddObjectAt(gridPos, 
                                databases.blockData[selectedBlockIndex].Size,
                                databases.blockData[selectedBlockIndex].ID,
                                placedGameObjects.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = databases.blockData[selectedBlockIndex].ID == 0 ? floorData : blockData;
        return selectedData.CanPlaceObjectAt(gridPosition, databases.blockData[selectedBlockIndex].Size);
    }

    private void StopPlacement()
    {
        selectedBlockIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        InputManager.instance.OnClicked -= PlaceStructure;
        InputManager.instance.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedBlockIndex < 0)
        {
            return;
        }
        // 마우스가 그리드 위에서 움직이고 있다면, mouseIndicator를 활성화하고 마우스를 따라다니도록 한다.
        // 마우스가 그리드 바깥에서 움직이고 있다면, 비활성화한다.
        Vector3 mousePos = InputManager.instance.GetSelectedMapPosition();
        bool isInGrid = mousePos.x != Vector3.negativeInfinity.x;
        cellIndicator.SetActive(isInGrid);
        if (!isInGrid)
        {
            return;
        }
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        // 마우스를 클릭한 곳에 블럭을 놓을 수 없다면 cellIndicator의 색깔을 빨간색으로 바꿈
        bool placementValidity = CheckPlacementValidity(gridPos, selectedBlockIndex);
        preveiwRenderer.material.color = placementValidity ? Color.white : Color.red;
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
    }
}