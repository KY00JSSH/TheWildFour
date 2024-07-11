using UnityEngine;
using Cinemachine;

public class CameraControl2 : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;
    public float rotationSpeed = 100f;
    public float zoomSpeed = 2f;

    // 플레이어 스킬 시야범위 확장 때문에
    // newFov = Mathf.Clamp 윗줄에 선언된 지역변수를 클래스변수로 이동
    public float minFOV = 70f;
    public float maxFOV = 100f;

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
        }
        if (Input.GetKey(KeyCode.E))
        {
            cinemachineFreeLook.m_XAxis.Value += rotationSpeed * Time.deltaTime;
        }


        // 카메라 줌
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float currentFOV = cinemachineFreeLook.m_Lens.FieldOfView;
            float deltaFOV = scrollInput * 10f * -1; // -1 안곱하면 마우스 스크롤이 반대가 됨. 직관성을 위해 이렇게 합니다.
            float newFOV = currentFOV + deltaFOV;


            newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);

            cinemachineFreeLook.m_Lens.FieldOfView = newFOV;
        }
    }
}
