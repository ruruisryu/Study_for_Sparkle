using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private BlockDatabaseSO databases;

    [SerializeField] private GameObject gridVisualization;
    private GridData blockData;
    [SerializeField] private PreviewSystem preview;
    [SerializeField] private ObjectPlacer objectPlacer;
    
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        blockData = new GridData();
    }

    public void StartPlacement(int id)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(id, grid, preview, databases, blockData, objectPlacer);
        InputManager.instance.OnClicked += PlaceStructure;
        InputManager.instance.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, blockData, objectPlacer);
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
        buildingState.OnAction(gridPos);
    }

    private void StopPlacement()
    {
        if (buildingState == null)
        {
            return;
        }
        gridVisualization.SetActive(false);
        buildingState.EndState();
        lastDetectedPosition = Vector3Int.zero;
        InputManager.instance.OnClicked -= PlaceStructure;
        InputManager.instance.OnExit -= StopPlacement;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null || CheckCursorInGrid() == false)
        {
            preview.HidePreview();
            return;
        }
        // 그리드 내에서 마우스의 움직임을 감지했다면
        Vector3 mousePos = InputManager.instance.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        if (lastDetectedPosition != gridPos)
        {
            buildingState.UpdateState(gridPos);
            lastDetectedPosition = gridPos;
        }
    }

    // 마우스 커서가 그리드 내부에 있는지 확인
    public bool CheckCursorInGrid()
    {
        Vector3 mousePos = InputManager.instance.GetSelectedMapPosition();
        bool isInGrid = mousePos.x != Vector3.negativeInfinity.x;
        return isInGrid;
    }
}