using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public GameObject player; // 플레이어 오브젝트
    public RectTransform minimapContainer; // 미니맵 UI 컨테이너
    public float mapScale = 10f; // 미니맵 축소 비율
    public Vector2 mapOffset; // 미니맵 오프셋
    public GameObject minimapTilePrefab; // 미니맵 타일 프리팹
    public RectTransform playerIcon; // 플레이어 아이콘

    public float zoomSpeed = 1f; // 줌 인/아웃 속도
    public float minMapScale = 5f; // 최소 맵 스케일
    public float maxMapScale = 20f; // 최대 맵 스케일

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
        // 모든 타일을 검색하여 미니맵에 추가
        foreach (var tile in FindObjectsOfType<MapTile>())
        {
            Vector3 tilePosition = tile.transform.position;
            GameObject minimapTile = Instantiate(minimapTilePrefab, minimapContainer);
            minimapTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(tilePosition.x, tilePosition.z) / mapScale + mapOffset;
            minimapTile.GetComponent<Image>().sprite = tile.minimapSprite; // 타일에 해당하는 스프라이트 할당
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

            // 미니맵 스케일을 조정합니다.
            foreach (Transform minimapTile in minimapContainer)
            {
                Vector3 tilePosition = minimapTile.transform.position;
                minimapTile.GetComponent<RectTransform>().anchoredPosition = new Vector2(tilePosition.x, tilePosition.z) / mapScale + mapOffset;
            }
        }
    }

    public class MapTile : MonoBehaviour
    {
        public Sprite minimapSprite; // 타일에 해당하는 미니맵 스프라이트
    }
}
