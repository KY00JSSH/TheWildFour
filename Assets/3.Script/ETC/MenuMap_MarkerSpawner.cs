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

    private MenuMapZoom zoom;

    private void Awake()
    {
        zoom = FindObjectOfType<MenuMapZoom>();        
    }


    public void SetMarker(BuildingType type, Vector3 position)
    {
        if(type < 0 || (int)type >= markerPrefabs.Length)
        {
            Debug.LogError("건설할 오브젝트 타입이나 마커가 없습니다. 박지훈 대가리 박아");
            return;
        }

        GameObject markerOnMap = Instantiate(markerPrefabs[(int)type], transform);
        markerOnMap.transform.localPosition = position;
        markers.Add(markerOnMap);
    }

    public void RemoveMarker(BuildingType type, Vector3 position)
    {
        GameObject markerToRemove = null;

        foreach(GameObject markerOnMap in markers)
        {
            if(markerOnMap.transform.localPosition == position && markerOnMap.name.Contains(type.ToString()))
            {
                markerToRemove = markerOnMap;
                break;
            }
        }
        if(markerToRemove != null)
        {
            markers.Remove(markerToRemove);
            Destroy(markerToRemove);
        }
        else
        {
            Debug.LogWarning("제거할 마커가 없습니다.");
        }
    }

    private void Update()
    {
        foreach(var each in markers)
        {
            each.transform.localPosition = NewPosition(each.transform.localPosition);
        }
    }

    private Vector3 NewPosition(Vector3 LerpPosition)
    {
        //월드 좌표를 미니맵 좌표로 변환
        Vector3 relativePosition = LerpPosition - transform.position;

        //미니맵의 실제 크기에 맞춰 비율 계산
        float normalizedX = relativePosition.x / mapWidth;
        float normalizedZ = relativePosition.z / mapHeight;

        //좌표 보정
        float ratio =
            Mathf.InverseLerp(zoom.maxOrthSize, zoom.minOrthSize, zoom.menuMapCamera.orthographicSize) *
            (zoom.maxOrthSize / zoom.minOrthSize - 1) + 1;

        float markerPosX = normalizedX * mapRect.rect.width * ratio;
        float markerPosY = normalizedZ * mapRect.rect.height * ratio;

        return new Vector3(markerPosX, markerPosY, 0);
    }
}
