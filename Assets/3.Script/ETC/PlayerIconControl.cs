using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconControl : MonoBehaviour
{
    public Transform playerTransform;
    public RectTransform playerIconRect;
    public RectTransform mapRect;
    public Camera worldMapCamera; //이게 꼭 필요한가
    public float mapWidth;
    public float mapHeight;

    //public RectTransform arrowIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 playerPos = playerTransform.position;
        Vector3 relativePos = playerPos - worldMapCamera.transform.position;

        float normalizedX = relativePos.x / mapWidth;
        float normalizedZ = relativePos.z / mapHeight;

        float iconPosX = normalizedX * mapRect.rect.width;
        float iconPosY = normalizedZ * mapRect.rect.height;

        playerIconRect.anchoredPosition = new Vector2(iconPosX, iconPosY);

        //UpdateArrowIconRotation();
    }

    //private void UpdateArrowIconRotation()
    //{
    //    //마우스 위치를 월드 좌표로 변환
    //    Vector3 mousePosition = Input.mousePosition;
    //    Vector3 worldMousePosition = 
    //        Camera.main.ScreenToWorldPoint(
    //            new Vector3(mousePosition.x, mousePosition.y, playerTransform.position.z - Camera.main.transform.position.z)
    //                                      );
    //
    //    //플레이어와 마우스 위치 간의 방향 계산
    //    Vector3 direction = worldMousePosition - playerTransform.position;
    //    direction.y = 0; //y축 무시
    //
    //    //방향 벡터를 회전 각도로 변환
    //    float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
    //
    //    arrowIcon.localRotation = Quaternion.Euler(0, 0, playerIconRect.eulerAngles.y * -1);
    //}
}
