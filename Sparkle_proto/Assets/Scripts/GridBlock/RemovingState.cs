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
        // 클릭된 위치에 블럭이 존재하는지 체크한다.
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
            // 블럭이 존재한다면
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if(gameObjectIndex == -1) {return;}

            // 블럭의 data, 게임 오브젝트 둘다 제거해준다.
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
        // 커서 위치 업데이트해주기
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, true);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        // 마우스를 대고 있는 곳에 삭제할 오브젝트가 있는지 없는지를 나타내줌
        bool validity = gridData.CanPlaceObjectAt(gridPosition, new List<Vector3Int>() { Vector3Int.zero });
        previewSystem.ShowPreview();
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
