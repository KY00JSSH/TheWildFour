using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuMapZoom : MonoBehaviour
{
    public RectTransform menuMap;
    public Camera menuMapCameara;

    public float minFOV = 10f;
    public float maxFOV = 60f;
    public float zoomSpeed = 10f;

    private void Awake()
    {
        menuMap = GameObject.Find("RawImage").GetComponent<RectTransform>();
        menuMapCameara = GetComponent<Camera>();
    }

    private void Update()
    {
        if(IsMouseOverUIElement(menuMap))
        { 
            if(EventSystem.current.IsPointerOverGameObject())
            
               
            
            MenuMap_Zoom();

        }
    }

    private void MenuMap_Zoom()
    {
        float scroll_Input = Input.GetAxis("Mouse ScrollWheel");
        if(scroll_Input != 0)
        {
            float currentFOV = menuMapCameara.fieldOfView;
            float deltaFOV = scroll_Input * 15f * -1;
            float newFOV = currentFOV + deltaFOV;

            newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);

            menuMapCameara.fieldOfView = newFOV;
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
}
