using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;
    private float rotationSpeed = 3f;
    public float zoomSpeed = 2f;

    // 플레이어 스킬 시야범위 확장 때문에
    // newFov = Mathf.Clamp 윗줄에 선언된 지역변수를 클래스변수로 이동
    public float minFOV = 70f;
    public float maxFOV = 100f;

    public float rotationDirection = 0f;

    private void Awake() {
        cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        cinemachineFreeLook.Follow = player.transform;
        cinemachineFreeLook.LookAt = player.transform;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            rotationDirection -= 90f;
            if (rotationDirection < 0) rotationDirection += 360f;
        }
        else if(Input.GetKeyDown(KeyCode.E)) {
            rotationDirection += 90f;
            if (rotationDirection >= 360) rotationDirection -= 360f;
        }
        cinemachineFreeLook.m_XAxis.Value = Mathf.LerpAngle(
            cinemachineFreeLook.m_XAxis.Value, rotationDirection, Time.deltaTime * rotationSpeed) ;

        // 카메라 줌
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float currentFOV = cinemachineFreeLook.m_Lens.FieldOfView;
            float deltaFOV = scrollInput * 15f * -1; // -1 안곱하면 마우스 스크롤이 반대가 됨. 직관성을 위해 이렇게 합니다.
            float newFOV = currentFOV + deltaFOV;

            newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);

            cinemachineFreeLook.m_Lens.FieldOfView = newFOV;
        }
    }
}
