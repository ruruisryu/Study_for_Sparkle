using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static InputManager instance; 
    
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask blockLayerMask;
    private Vector3 lastMousePosition;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    // UI오브젝트 위에 포인터가 있는 없는지를 bool 타입으로 반환해주는 람다식
    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    /// <summary>
    /// 카메라에서 마우스 포인터가 가리키는 곳으로 Raycast한 다음 해당 위치를 마우스의 마지막 위치로서 반환
    /// <returns></returns>
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 MousePos = Input.mousePosition;
        //nearClipPlane: 해당 카메라의 위치부터 렌더링을 시작할 최소 위치까지의 거리
        MousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(MousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, blockLayerMask))
        {
            lastMousePosition = hit.point;
            return lastMousePosition;
        }
        return Vector3.negativeInfinity;
    }
}
