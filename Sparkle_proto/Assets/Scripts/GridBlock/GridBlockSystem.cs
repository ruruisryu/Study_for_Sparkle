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

    private void Start()
    {
        StopPlacement();
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
        Vector3 mousePos = InputManager.instance.GetSelectedMapPosition();
        bool isInGrid = mousePos.x != Vector3.negativeInfinity.x;
        if (isInGrid)
        {
            Debug.Log("isInGrid: true");
            Vector3Int gridPos = grid.WorldToCell(mousePos);
            GameObject newBlock = Instantiate(databases.blockData[selectedBlockIndex].Prefab);
            newBlock.transform.position = grid.CellToWorld(gridPos);
        }
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
        if (isInGrid)
        {
            // CellToWorld: cell position -> world position
            // WorldToCell: world position -> cell posoition
            Vector3Int gridPos = grid.WorldToCell(mousePos);
            cellIndicator.transform.position = grid.CellToWorld(gridPos);
        }
    }
}
