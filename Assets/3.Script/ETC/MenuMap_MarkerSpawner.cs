using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMap_MarkerSpawner : MonoBehaviour
{
    /*
    1. 각 빌딩과 모닥불의 프리팹을 배열로 갖고 있기
    2. 메서드로 타입, 위치값 등 배개변수로 하는 것 작성해서 다른 스크립트에서 끌어다 쓸 수 있게 하기
    2-1. 보내준 데이터를 리스트로 갖고 있기 - 건물 오브젝트 생성이니까
    2-2. 리스트를 순회해서 위치값에 해당하는 미니맵 위치에 아이콘 띄우기
    3. 실제 위치를 미니맵의 위치랑 맵핑해서 미니맵에 나타나게 하기
    4. 줌인/아웃할 때도 미니맵 아이콘 위치 정상적으로 연동되게 하기
    5. 패널 미니맵에도 아이콘 뜨게 하기
    5-1. 패널 미니맵은 어차피 맵 스프라이트와 크기가 1:1이니까 그 위치 그대로 갖다 넣으면 됨    
    */

    [SerializeField] public GameObject[] markerPrefabs;
    private List<GameObject> markers = new List<GameObject>();

    public RectTransform mapRect;

    public float mapWidth  = 250f;
    public float mapHeight = 250f;

    public MenuMapZoom zoom;

    private void Awake()
    {
        //zoom = FindObjectOfType<MenuMapZoom>();
        //if (zoom == null)
        //{
        //    Debug.LogError("MenuMapZoom 객체가 없습니다. Zoom 객체를 찾아주세요.");
        //}
        //mapRect = transform.Find("MenuMapSprite").GetComponent<RectTransform>();
        //if (mapRect == null)
        //{
        //    Debug.LogError("RectTransform component not found on MenuMapSprite.");
        //}
    }


    public void SetMarker(BuildingType type, Vector3 position)
    {
        if(type < 0 || (int)type >= markerPrefabs.Length)
        {
            Debug.LogError("건설할 오브젝트 타입이나 마커가 없습니다.");
            return;
        }

        GameObject markerOnMap = Instantiate(markerPrefabs[(int)type], /*transform.Find("MenuMapSprite")*/ mapRect); //오브젝트 계층구조 명시화: mapRect에 할당된 오브젝트의 자식으로 마커 생성
        RectTransform markerRect = markerOnMap.GetComponent<RectTransform>();

        Vector3 correctedPosition = WorldToMapPosition(position);
        markerRect.anchoredPosition = correctedPosition;
        markers.Add(markerOnMap);
    }

    public void RemoveMarker(BuildingType type)
    {

        foreach(GameObject markerOnMap in markers)
        {
            if ((markerOnMap.name[1] == 'o' && type == BuildingType.Workshop) ||
                 (markerOnMap.name[1] == 'h' && type == BuildingType.Shelter))
            {
                Destroy(markerOnMap);
                markers.Remove(markerOnMap);
                break;
            }

        }
    }

    private void Update()
    {
        foreach(var each in markers)
        {
            //each.transform.localPosition = WorldToMapPosition(each.transform.position);

            //each.anchoredPosition = WorldToMapPosition(each.position);

            Vector2 anchor = each.GetComponent<RectTransform>().anchoredPosition;

            Vector3 worldPos = MapToWorldPosition(anchor);
            anchor = WorldToMapPosition(worldPos);
        }
    }

    private Vector3 WorldToMapPosition(Vector3 worldPosition) //LerpPosition = 마커의 월드 좌표
    {
        if (mapWidth == 0 || mapHeight == 0)
        {
            Debug.LogError("mapWidth 또는 mapHeight가 0입니다. 올바른 값을 설정하세요.");
            return Vector3.zero;
        }

        //월드 좌표를 미니맵 좌표로 변환
        Vector3 relativePosition = worldPosition - transform.position;

        //미니맵의 실제 크기에 맞춰 비율 계산
        float normalizedX = relativePosition.x / mapWidth;
        float normalizedY = relativePosition.z / mapHeight;

        Debug.Log(zoom);
        //좌표 보정
        float ratio =
            Mathf.InverseLerp(zoom.maxOrthSize, zoom.minOrthSize, zoom.menuMapCamera.orthographicSize) *
            (zoom.maxOrthSize / zoom.minOrthSize - 1) + 1;

        float markerPosX = normalizedX * mapRect.rect.width * ratio;
        float markerPosY = normalizedY * mapRect.rect.height * ratio;

        if (float.IsNaN(markerPosX) || float.IsInfinity(markerPosX) || float.IsNaN(markerPosY) || float.IsInfinity(markerPosY))
        {
            Debug.LogError("계산된 마커 위치가 유효하지 않습니다. 입력값을 확인하세요.");
            return Vector3.zero;
        }
        return new Vector3(markerPosX, markerPosY, 0);
    }

    private Vector3 MapToWorldPosition(Vector3 mapPosition)
    {
        float normalizedX = mapPosition.x / mapRect.rect.width;
        float normalizedY = mapPosition.y / mapRect.rect.height;

        float worldPosX = normalizedX * mapWidth + transform.position.x;
        float worldPosZ = normalizedY * mapHeight + transform.position.z;

        return new Vector3(worldPosX, 0, worldPosZ);
    }
}
