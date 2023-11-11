using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance; 
    
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask gridLayerMask;
    private Vector3 lastMousePosition;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }
    
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
        if (Physics.Raycast(ray, out hit, 100, gridLayerMask))
        {
            lastMousePosition = hit.point;
            return lastMousePosition;
        }
        return Vector3.negativeInfinity;
    }
}
