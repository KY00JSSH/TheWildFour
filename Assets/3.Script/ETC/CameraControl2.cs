using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Cinemachine;

public class CameraControl2 : MonoBehaviour //IScrollHandler
{
    public CinemachineFreeLook cinemachineFreeLook;
    public float rotationSpeed = 100f;
    public float zoomSpeed = 2f;

    // 플레이어 스킬 시야범위 확장 때문에
    // newFov = Mathf.Clamp 윗줄에 선언된 지역변수를 클래스변수로 이동
    public float minCinemachineFOV = 70f;
    public float maxCinemachineFOV = 100f;

    //미니맵 카메라 부모 객체 참조 추가
    public Transform miniMapCameraParent;

    public Camera mainCamera;  //240716 17:00
    public float minFOV = 15f; //240716 17:00
    public float maxFOV = 70f; //240716 17:00

    private void Awake() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        cinemachineFreeLook.Follow = player.transform;
        cinemachineFreeLook.LookAt = player.transform;
    }
    private void Update()
    {
        // 카메라 좌우 회전
        if (Input.GetKey(KeyCode.Q))
        {
            cinemachineFreeLook.m_XAxis.Value -= rotationSpeed * Time.deltaTime;
            if(miniMapCameraParent != null)
            {
                miniMapCameraParent.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            cinemachineFreeLook.m_XAxis.Value += rotationSpeed * Time.deltaTime;
            if(miniMapCameraParent != null)
            {
                miniMapCameraParent.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }

        //if(!IsmouseOverUI())
        //{
        //    return;
        //}

        HandleCinemachinZoom();

    }

    private void HandleCinemachinZoom()
    {
        // 시네머신 줌
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float currentFOV = cinemachineFreeLook.m_Lens.FieldOfView;
            float deltaFOV = scrollInput * 15f * -1; // -1 안곱하면 마우스 스크롤이 반대가 됨. 직관성을 위해 이렇게 합니다.
            float newFOV = currentFOV + deltaFOV;


            newFOV = Mathf.Clamp(newFOV, minCinemachineFOV, maxCinemachineFOV);

            cinemachineFreeLook.m_Lens.FieldOfView = newFOV;
        }
    }

    private bool IsmouseOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Transform>() == transform)
            {
                return true;
            }
        }
        return false;
    }
}
