using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;
    public Camera minimapCamera;

    private float rotationSpeed = 3f;
    public float zoomSpeed = 2f;

    public float rotationDirection = 0f;

    private MiniMap_CompassRotation miniMapCompassRotation;

    public float minZoom = 8f;
    public float maxZoom = 13f;

    private void Awake() {
        cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
        minimapCamera = GameObject.Find("MiniMap_Camera").GetComponent<Camera>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        cinemachineFreeLook.Follow = player.transform;
        cinemachineFreeLook.LookAt = player.transform;
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
        }
        else if(Input.GetKeyDown(KeyCode.E)) {
            rotationDirection += 90f;
            if (rotationDirection >= 360) rotationDirection -= 360f;
        }
        cinemachineFreeLook.m_XAxis.Value = Mathf.LerpAngle(
            cinemachineFreeLook.m_XAxis.Value, rotationDirection, Time.deltaTime * rotationSpeed) ;
        minimapCamera.transform.rotation = Quaternion.Euler(90,
            Mathf.LerpAngle(cinemachineFreeLook.m_XAxis.Value, rotationDirection, Time.deltaTime * rotationSpeed), 0);


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
