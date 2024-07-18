using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Cinemachine;

public class CameraControl2 : MonoBehaviour //IScrollHandler
{
    public CinemachineFreeLook cinemachineFreeLook;
    public float rotationSpeed = 100f;
    public float zoomSpeed = 2f;

    // �÷��̾� ��ų �þ߹��� Ȯ�� ������
    // newFov = Mathf.Clamp ���ٿ� ����� ���������� Ŭ���������� �̵�
    public float minCinemachineFOV = 70f;
    public float maxCinemachineFOV = 100f;

    //�̴ϸ� ī�޶� �θ� ��ü ���� �߰�
    public Transform miniMapCameraParent;

    public Camera mainCamera;  //240716 17:00
    public float minFOV = 15f; //240716 17:00
    public float maxFOV = 70f; //240716 17:00

    public GameObject uiCanvas; //Ư�� Canvas UI�� Ȱ��ȭ�Ǿ� ���� �� ���� ���ξƿ� �ȵǰ� 240718 09:53

    private void Awake() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        cinemachineFreeLook.Follow = player.transform;
        cinemachineFreeLook.LookAt = player.transform;

        uiCanvas = GameObject.Find("Menu_Map"); //Ư�� Canvas UI�� Ȱ��ȭ�Ǿ� ���� �� ���� ���ξƿ� �ȵǰ� 240718 09:53
    }
    private void Update()
    {
        // ī�޶� �¿� ȸ��
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

        if(uiCanvas != null && uiCanvas.activeSelf) //Ư�� Canvas UI�� Ȱ��ȭ�Ǿ� ���� �� ���� ���ξƿ� �ȵǰ� 240718 09:53
        {
            return;
        }

        //if(!IsmouseOverUI())
        //{
        //    return;
        //}

        HandleCinemachinZoom();

    }

    private void HandleCinemachinZoom()
    {
        // �ó׸ӽ� ��
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float currentFOV = cinemachineFreeLook.m_Lens.FieldOfView;
            float deltaFOV = scrollInput * 15f * -1; // -1 �Ȱ��ϸ� ���콺 ��ũ���� �ݴ밡 ��. �������� ���� �̷��� �մϴ�.
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
