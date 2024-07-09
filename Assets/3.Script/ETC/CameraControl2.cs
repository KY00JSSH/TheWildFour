using UnityEngine;
using Cinemachine;

public class CameraControl2 : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;
    public float rotationSpeed = 100f;
    public float zoomSpeed = 2f;

    private void Update()
    {
        // ÁÂ¿ì È¸Àü
        if (Input.GetKey(KeyCode.Q))
        {
            cinemachineFreeLook.m_XAxis.Value -= rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            cinemachineFreeLook.m_XAxis.Value += rotationSpeed * Time.deltaTime;
        }






        // ÁÜ -> Æ÷±â??
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float currentFOV = cinemachineFreeLook.m_Lens.FieldOfView;
            float deltaFOV = scrollInput * 10f;
            float newFOV = currentFOV + deltaFOV;
            cinemachineFreeLook.m_Lens.FieldOfView = newFOV;
        }
    }
}
