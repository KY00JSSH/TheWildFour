using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMap_MarkerSpawner : MonoBehaviour
{
    [SerializeField] public GameObject[] markerPrefabs;
    private List<GameObject> markers = new List<GameObject>();

    public Transform mapTransform;
    public float mapWidth = 250f;
    public float mapHeight = 250f;

    // Start is called before the first frame update
    private void Start()
    {
        if (mapTransform == null)
        {
            Debug.LogError("맵이 할당되지 않았습니다.");
        }
    }

    public void SetMarker(BuildingType type, Vector3 position)
    {
        if(type < 0 || (int)type >= markerPrefabs.Length)
        {
            Debug.LogError("지정된 건물이 없습니다");
            return;
        }

        GameObject marker = Instantiate(markerPrefabs[(int)type], mapTransform);
        marker.transform.position = WorldToMapPosition(position);
        marker.layer = LayerMask.NameToLayer("MiniMapObject");
        marker.Add(marker);
    }

    public void RemoveMarker(BuildingType type, Vector3 position)
    {
        GameObject markerToRemove = null;

        foreach (GameObject marker in markers)
        {
            if (Vector3.Distance(marker.transform.position, position) < 0.1f && marker.name.Contains(type.ToString()))
            {
                markerToRemove = marker;
                break;
            }
        }

        if (markerToRemove != null)
        {
            markers.Remove(markerToRemove);
            Destroy(markerToRemove);
        }
        else
        {
            Debug.LogWarning("제거할 마커가 없습니다.");
        }
    }

    private Vector3 WorldToMapPosition(Vector3 worldPosition)
    {
        if (mapWidth == 0 || mapHeight == 0)
        {
            Debug.LogError("맵 크기 세팅이 잘못됐습니다.");
            return Vector3.zero;
        }

        Vector3 relativePosition = worldPosition - transform.position;

        float normalizedX = relativePosition.x / mapWidth;
        float normalizedY = relativePosition.z / mapHeight;

        float mapPosX = normalizedX * mapTransform.localScale.x;
        float mapPosY = normalizedY * mapTransform.localScale.y;

        return new Vector3(mapPosX, mapPosY, mapTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
