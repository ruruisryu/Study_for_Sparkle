using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlockSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private Grid grid;

    private void Update()
    {
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
