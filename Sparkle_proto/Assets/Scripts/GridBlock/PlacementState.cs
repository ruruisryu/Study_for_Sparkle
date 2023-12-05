using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedBlockIndex = -1;
    private int ID;
    private Grid grid;
    private PreviewSystem previewSystem;
    private BlockDatabaseSO database;
    private GridData gridData;
    private ObjectPlacer objectPlacer;

    public PlacementState(
        int id, 
        Grid grid, 
        PreviewSystem previewSystem, 
        BlockDatabaseSO database, 
        GridData gridData, 
        ObjectPlacer objectPlacer)
    {
        ID = id;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.gridData = gridData;
        this.objectPlacer = objectPlacer;
        
        // FindIndex에 인자로 들어갈 predicate를 람다식으로 건네준 것
        selectedBlockIndex = database.blockData.FindIndex(data => data.ID == id);
        if (selectedBlockIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview( database.blockData[selectedBlockIndex].Prefab);
        }
        else
        {
            throw new System.Exception($"No object with ID {id}");
        }
    }

    // 블럭을 배치하는 상태를 종료시킨다.
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        // 클락한 위치에 블럭을 놓을 수 있는지 판단
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedBlockIndex);
        if (placementValidity == false)
        {
            return;
        }

        // 블럭 (게임 오브젝트) 추가
        int index = objectPlacer.PlaceObject(database.blockData[selectedBlockIndex].Prefab, grid.CellToWorld(gridPosition));
        // 블럭 (데이터) 추가
        gridData.AddObjectAt(gridPosition, 
            database.blockData[selectedBlockIndex].Size,
            database.blockData[selectedBlockIndex].ID,
            index);
        
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    // 해당 위치에 블럭을 놓을 수 있는지 없는지
    public bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        return gridData.CanPlaceObjectAt(gridPosition, database.blockData[selectedBlockIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        // 마우스를 대고 있는 곳에 블럭을 놓을 수 있는지 없는지를 색으로 나타내줌.
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedBlockIndex);
        previewSystem.ShowPreview();
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
