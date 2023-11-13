using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlockSystem : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private BlockDatabaseSO databases;

    private int selectedBlockIndex = -1;
    [SerializeField] private GameObject gridVisualization;
    private GridData floorData, blockData;
    private List<GameObject> placedGameObjects = new List<GameObject>();
    [SerializeField] private PreviewSystem preview;
    
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        blockData = new GridData();
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
        preview.StartShowingPlacementPreview(databases.blockData[selectedBlockIndex].Prefab, databases.blockData[selectedBlockIndex].Size);
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
        
        preview.UpdatePosition(grid.CellToWorld(gridPos), false);
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
        preview.StopShowingPreview();
        lastDetectedPosition = Vector3Int.zero;
        InputManager.instance.OnClicked -= PlaceStructure;
        InputManager.instance.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedBlockIndex < 0 || CheckCursorInGrid() == false)
        {
            preview.HidePreview();
            return;
        }
        // 그리드 내에서 마우스의 움직임을 감지했다면
        Vector3 mousePos = InputManager.instance.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        if (lastDetectedPosition != gridPos)
        {
            // 마우스를 클릭한 곳에 블럭을 놓을 수 있는지 없는지를 색으로 나타내줌.
            bool placementValidity = CheckPlacementValidity(gridPos, selectedBlockIndex);
            preview.ShowPreview();
            preview.UpdatePosition(grid.CellToWorld(gridPos), placementValidity);
            lastDetectedPosition = gridPos;
        }
    }

    public bool CheckCursorInGrid()
    {
        Vector3 mousePos = InputManager.instance.GetSelectedMapPosition();
        bool isInGrid = mousePos.x != Vector3.negativeInfinity.x;
        return isInGrid;
    }
}