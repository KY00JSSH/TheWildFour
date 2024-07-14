using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public GameObject player; // �÷��̾� ������Ʈ
    public RectTransform minimapContainer; // �̴ϸ� UI �����̳�
    public float mapScale = 10f; // �̴ϸ� ��� ����
    public Vector2 mapOffset; // �̴ϸ� ������
    public GameObject minimapTilePrefab; // �̴ϸ� Ÿ�� ������
    public RectTransform playerIcon; // �÷��̾� ������

    public float zoomSpeed = 1f; // �� ��/�ƿ� �ӵ�
    public float minMapScale = 5f; // �ּ� �� ������
    public float maxMapScale = 20f; // �ִ� �� ������

    void Start()
    {
        GenerateMinimap();
    }

    void Update()
    {
        HandleZoom();
        UpdatePlayerPosition();
    }

    void GenerateMinimap()
    {
        // ��� Ÿ���� �˻��Ͽ� �̴ϸʿ� �߰�
        foreach (var tile in FindObjectsOfType<MapTile>())
        {
            Vector3 tilePosition = tile.transform.position;
            GameObject minimapTile = Instantiate(minimapTilePrefab, minimapContainer);
            minimapTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(tilePosition.x, tilePosition.z) / mapScale + mapOffset;
            minimapTile.GetComponent<Image>().sprite = tile.minimapSprite; // Ÿ�Ͽ� �ش��ϴ� ��������Ʈ �Ҵ�
        }
    }

    void UpdatePlayerPosition()
    {
        Vector3 playerPosition = player.transform.position;
        playerIcon.anchoredPosition = new Vector2(playerPosition.x, playerPosition.z) / mapScale + mapOffset;
    }

    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            mapScale -= scrollInput * zoomSpeed;
            mapScale = Mathf.Clamp(mapScale, minMapScale, maxMapScale);

            // �̴ϸ� �������� �����մϴ�.
            foreach (Transform minimapTile in minimapContainer)
            {
                Vector3 tilePosition = minimapTile.transform.position;
                minimapTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(tilePosition.x, tilePosition.z) / mapScale + mapOffset;
            }
        }
    }

    public class MapTile : MonoBehaviour
    {
        public Sprite minimapSprite; // Ÿ�Ͽ� �ش��ϴ� �̴ϸ� ��������Ʈ
    }
}
