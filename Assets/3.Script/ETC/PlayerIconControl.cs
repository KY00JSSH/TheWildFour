using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�޴� UI���� Ȯ���� �� �ִ� ���� �÷��̾� ������ ��Ʈ��

public class PlayerIconControl : MonoBehaviour
{
    public Transform playerTransform;
    public RectTransform playerIconRect;
    public RectTransform mapRect;
    public Camera worldMapCamera; 
    public float mapWidth;
    public float mapHeight;


    private MenuMapZoom zoom;
    //public RectTransform arrowIcon;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        zoom = FindObjectOfType<MenuMapZoom>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 playerPos = playerTransform.position;
        Vector3 relativePos = playerPos - worldMapCamera.transform.position;

        //���� ����/�ƿ��� ���� �÷��̾� �������� ��ġ�̵� �� �߰��� ���� ���� ���
        float ratio = 
            Mathf.InverseLerp(zoom.maxOrthSize, zoom.minOrthSize, zoom.menuMapCamera.orthographicSize) * 
            (zoom.maxOrthSize / zoom.minOrthSize - 1) + 1;

        float normalizedX = relativePos.x / mapWidth;
        float normalizedZ = relativePos.z / mapHeight;

        float iconPosX = normalizedX * mapRect.rect.width * ratio;  //�������� ��ġ�� ����� ���� �߰� ���
        float iconPosY = normalizedZ * mapRect.rect.height * ratio; //�������� ��ġ�� ����� ���� �߰� ���

        playerIconRect.anchoredPosition = new Vector2(iconPosX, iconPosY);

        //UpdateArrowIconRotation();
    }

    //private void UpdateArrowIconRotation()
    //{
    //    //���콺 ��ġ�� ���� ��ǥ�� ��ȯ
    //    Vector3 mousePosition = Input.mousePosition;
    //    Vector3 worldMousePosition = 
    //        Camera.main.ScreenToWorldPoint(
    //            new Vector3(mousePosition.x, mousePosition.y, playerTransform.position.z - Camera.main.transform.position.z)
    //                                      );
    //
    //    //�÷��̾�� ���콺 ��ġ ���� ���� ���
    //    Vector3 direction = worldMousePosition - playerTransform.position;
    //    direction.y = 0; //y�� ����
    //
    //    //���� ���͸� ȸ�� ������ ��ȯ
    //    float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
    //
    //    arrowIcon.localRotation = Quaternion.Euler(0, 0, playerIconRect.eulerAngles.y * -1);
    //}
}
