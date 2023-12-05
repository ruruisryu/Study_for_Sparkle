using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private Grid grid;
    private PreviewSystem previewSystem;
    private GridData gridData;
    private ObjectPlacer objectPlacer;

    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData gridData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (gridData.CanPlaceObjectAt(gridPosition, new List<Vector3Int>() { Vector3Int.zero }) == false)
        {
            selectedData = gridData;
        }

        if (selectedData == null)
        {
            // TODO sound to inform nothing to remove
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if(gameObjectIndex == -1) {return;}

            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, true);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = gridData.CanPlaceObjectAt(gridPosition, new List<Vector3Int>() { Vector3Int.zero });
        previewSystem.ShowPreview();
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
