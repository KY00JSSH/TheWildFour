using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMap_MarkerSpawner : MonoBehaviour
{
    [SerializeField] public GameObject[] markerPrefabs; // 마커 프리팹
    private List<GameObject> markers = new List<GameObject>();

    public Transform mapTransform; // 마커가 생성될 부모 오브젝트를 할당하는 곳

    //public float mapWidth = 250f; //맵 크기 상관 없음. 어차피 건설된 그 위에 수직으로 마커 배치될거니까
    //public float mapHeight = 250f;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    public void SetMarker(BuildingType type, Vector3 position) //마커 세팅
    {
        if(type < 0 || (int)type >= markerPrefabs.Length)
        {
            Debug.LogError("건설할 오브젝트 타입이나 마커가 없습니다.");
            return;
        }

        GameObject markerOnMap = Instantiate(markerPrefabs[(int)type], mapTransform); //배치될 마커 프리팹의 타입과 부모 오브젝트
        //Transform markerTransform = markerOnMap.GetComponent<Transform>();
        //markerOnMap.layer = LayerMask.NameToLayer("MiniMapObject"); //생성시 자동으로 MiniMapObject 레이어로 설정

        Vector3 pmMarkerPosition = new Vector3(position.x, 206f, position.z);
        markerOnMap.transform.position = pmMarkerPosition;
        markers.Add(markerOnMap); //리스트에 마커 추가

        Debug.Log($"마커가 부모 오브젝트 {markerOnMap.transform.parent.name}의 하위인 {markerOnMap.transform.position} 위치에 생성됐습니다.");
    }

    //public void RemoveMarker(BuildingType type, Vector3 position)
    //{
    //    GameObject markerToRemove = null;
    //
    //    foreach (GameObject marker in markers)
    //    {
    //        if (Vector3.Distance(marker.transform.position, position) < 0.1f && marker.name.Contains(type.ToString()))
    //        {
    //            markerToRemove = marker;
    //            break;
    //        }
    //    }
    //
    //    if (markerToRemove != null)
    //    {
    //        markers.Remove(markerToRemove);
    //        Destroy(markerToRemove);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("제거할 마커가 없습니다.");
    //    }
    //}


    public void RemoveMarker(BuildingType type)
    {

        foreach (GameObject markerOnMap in markers)
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


    //private Vector3 WorldToMapPosition(Vector3 worldPosition)
    //{
    //    if (mapWidth == 0 || mapHeight == 0)
    //    {
    //        Debug.LogError("mapWidth 또는 mapHeight가 0입니다. 올바른 값을 설정하세요.");
    //        return Vector3.zero;
    //    }
    //
    //    //Vector3 relativePosition = worldPosition - transform.position; // 이거 필요함?? 확인 좀 0722 09:29
    //    Vector3 localPosition = mapTransform.InverseTransformPoint(worldPosition);
    //
    //    //float normalizedX = localPosition.x / mapWidth;
    //    //float normalizedY = localPosition.z / mapHeight;
    //
    //    float normalizedX = (localPosition.x + mapWidth / 2) / mapWidth;
    //    float normalizedY = (localPosition.z + mapHeight / 2) / mapHeight;
    //
    //    float mapPosX = normalizedX * mapWidth;
    //    float mapPosY = normalizedY * mapHeight;
    //
    //    return new Vector3(mapPosX, mapPosY, mapTransform.position.z);
    //}
}
