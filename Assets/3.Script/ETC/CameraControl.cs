using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;
    private float rotationSpeed = 3f;
    public float zoomSpeed = 2f;

    public float rotationDirection = 0f;

    private MiniMap_CompassRotation miniMapCompassRotation;

    public float minZoom = 8f;
    public float maxZoom = 13f;

    private void Awake() {
        cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        cinemachineFreeLook.Follow = player.transform;
        cinemachineFreeLook.LookAt = player.transform;
        miniMapCompassRotation = FindObjectOfType<MiniMap_CompassRotation>();
    }

    private void Start() {
        cinemachineFreeLook.m_Lens.FieldOfView = 50f;
        cinemachineFreeLook.m_Orbits[0].m_Height = maxZoom;
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

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float currentZoom = cinemachineFreeLook.m_Orbits[0].m_Height;
            float deltaZoom = scrollInput * 15f * -1;
            float newZoom = currentZoom + deltaZoom;

            newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

            cinemachineFreeLook.m_Orbits[0].m_Height = newZoom;
        }
    }
}
