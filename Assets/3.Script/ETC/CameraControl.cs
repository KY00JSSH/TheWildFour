using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;
    private float rotationSpeed = 3f;
    public float zoomSpeed = 2f;

    // �÷��̾� ��ų �þ߹��� Ȯ�� ������
    // newFov = Mathf.Clamp ���ٿ� ����� ���������� Ŭ���������� �̵�
    public float minFOV = 70f;
    public float maxFOV = 100f;

    public float rotationDirection = 0f;

    private MiniMap_CompassRotation miniMapCompassRotation;

    private void Awake() {
        cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        cinemachineFreeLook.Follow = player.transform;
        cinemachineFreeLook.LookAt = player.transform;



        GameObject.FindGameObjectWithTag("Player");
        GameObject.FindObjectOfType<Renderer>();

        miniMapCompassRotation = FindObjectOfType<MiniMap_CompassRotation>();

     //   transform.GetComponent<>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            rotationDirection -= 90f;
            if (rotationDirection < 0) rotationDirection += 360f;
            miniMapCompassRotation.SetRotationDirection(rotationDirection);
        }
        else if(Input.GetKeyDown(KeyCode.E)) {
            rotationDirection += 90f;
            if (rotationDirection >= 360) rotationDirection -= 360f;
            miniMapCompassRotation.SetRotationDirection(rotationDirection);
        }
        cinemachineFreeLook.m_XAxis.Value = Mathf.LerpAngle(
            cinemachineFreeLook.m_XAxis.Value, rotationDirection, Time.deltaTime * rotationSpeed) ;

        // ī�޶� ��
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float currentFOV = cinemachineFreeLook.m_Lens.FieldOfView;
            float deltaFOV = scrollInput * 15f * -1; // -1 �Ȱ��ϸ� ���콺 ��ũ���� �ݴ밡 ��. �������� ���� �̷��� �մϴ�.
            float newFOV = currentFOV + deltaFOV;

            newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);

            cinemachineFreeLook.m_Lens.FieldOfView = newFOV;
        }
    }
}
