using UnityEngine;
using Cinemachine;

public class CameraControl2 : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;
    public float rotationSpeed = 100f;
    public float zoomSpeed = 2f;

    private void Update()
    {
        // ī�޶� �¿� ȸ��
        if (Input.GetKey(KeyCode.Q))
        {
            cinemachineFreeLook.m_XAxis.Value -= rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            cinemachineFreeLook.m_XAxis.Value += rotationSpeed * Time.deltaTime;
        }


        // ī�޶� ��
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float currentFOV = cinemachineFreeLook.m_Lens.FieldOfView;
            float deltaFOV = scrollInput * 10f * -1; // -1 �Ȱ��ϸ� ���콺 ��ũ���� �ݴ밡 ��. �������� ���� �̷��� �մϴ�.
            float newFOV = currentFOV + deltaFOV;

            float minFOV = 70f;
            float maxFOV = 100f;
            newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);

            cinemachineFreeLook.m_Lens.FieldOfView = newFOV;
        }
    }
}