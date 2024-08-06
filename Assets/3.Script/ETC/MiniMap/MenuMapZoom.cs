using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuMapZoom : MonoBehaviour
{
    public RectTransform menuMap;
    public Camera menuMapCamera;

    public float minOrthSize = 125f;
    public float maxOrthSize = 65f;
    public float zoomSpeed = 10f;

    private Vector3 dragOrigin;

    private void Awake()
    {
        menuMapCamera.orthographic = true; //카메라를 orthographic으로 설정
    }

    private void Update()
    {       
        if (IsMouseOverUIElement(menuMap))
        {                              
            MenuMap_Zoom();

            if(Input.GetMouseButtonDown(0))
            {
                dragOrigin = Input.mousePosition;
                return;
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 pos = menuMapCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                Vector3 move =
                new Vector3(pos.x * menuMapCamera.orthographicSize * 2, 0, pos.y * menuMapCamera.orthographicSize * 2);
                menuMapCamera.transform.Translate(-move, Space.World);

                dragOrigin = Input.mousePosition;

                ClampCameraPosition();
            }
        }
    }

    private void MenuMap_Zoom()
    {
        float scroll_Input = Input.GetAxis("Mouse ScrollWheel");
        if(scroll_Input != 0)
        {
            float currentOrthSize = menuMapCamera.orthographicSize;
            float deltaOrthSize = scroll_Input * zoomSpeed * -1;
            float newOrthSize = currentOrthSize + deltaOrthSize;
            
            menuMapCamera.orthographicSize = Mathf.Clamp(newOrthSize, minOrthSize, maxOrthSize);

            ClampCameraPosition();
        }
    }

    private bool IsMouseOverUIElement(RectTransform rectTransform)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach(RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<RectTransform>() == rectTransform)
            {
                return true;
            }
        }
        return false;        
    }

    private void ClampCameraPosition()
    {
        Vector3 camPos = menuMapCamera.transform.position;
               
        float vertExtent = menuMapCamera.orthographicSize;
        //float horzExtent = menuMapCamera.orthographicSize * menuMapCamera.aspect;
        float horzExtent = vertExtent * menuMapCamera.aspect;

        float leftBound   = menuMap.rect.xMin + horzExtent;
        float rightBound  = menuMap.rect.xMax - horzExtent;
        float bottomBound = menuMap.rect.yMin + vertExtent;
        float topBound    = menuMap.rect.yMax - vertExtent;

        camPos.x = Mathf.Clamp(camPos.x, leftBound, rightBound);
        camPos.z = Mathf.Clamp(camPos.z, bottomBound, topBound);       

        menuMapCamera.transform.position = camPos;
    }    
}
