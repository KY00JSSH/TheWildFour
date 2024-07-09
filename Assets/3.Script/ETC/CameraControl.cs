using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public Transform player;
    public float rotationSpeed = 100f;
    public float zoomSpeed = 2f;
    public float minZoomDistance = 2f;
    public float maxZoomDistance = 10f;

    private Transform cameraTransform;
    private float currentZoomDistance;

    private void Start()
    {
        cameraTransform = cinemachineVirtualCamera.transform;
        currentZoomDistance = Vector3.Distance(cameraTransform.position, player.position);
    }

    private void Update()
    {
        //Ä«¸Þ¶ó ÁÂ¿ì È¸Àü
        if(Input.GetKey(KeyCode.Q))
        {
            cameraTransform.RotateAround(player.position, Vector3.up, -rotationSpeed * Time.deltaTime);
            Debug.Log("Q" + cameraTransform.position);
        }
        if (Input.GetKey(KeyCode.E))
        {
            cameraTransform.RotateAround(player.position, Vector3.up, rotationSpeed * Time.deltaTime);
            Debug.Log("E" + cameraTransform.position);
        }

        //ÁÜ
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            currentZoomDistance -= scrollInput * zoomSpeed;
            currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);

            Vector3 direction = (cameraTransform.position - player.position).normalized;
            cameraTransform.position = player.position + direction * currentZoomDistance;

            Debug.Log("ÁÜ" + scrollInput + "ÁÜ °Å¸®" + currentZoomDistance + "Ä«¸Þ¶óÆ÷Áö¼Ç" + cameraTransform.position);
        }
    }
}
