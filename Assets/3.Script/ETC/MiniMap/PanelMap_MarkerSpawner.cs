using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMap_MarkerSpawner : MonoBehaviour
{
    [SerializeField] public GameObject[] markerPrefabs; // ��Ŀ ������
    private List<GameObject> markers = new List<GameObject>();

    public Transform mapTransform; // ��Ŀ�� ������ �θ� ������Ʈ�� �Ҵ��ϴ� ��

    //public float mapWidth = 250f; //�� ũ�� ��� ����. ������ �Ǽ��� �� ���� �������� ��Ŀ ��ġ�ɰŴϱ�
    //public float mapHeight = 250f;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    public void SetMarker(BuildingType type, Vector3 position) //��Ŀ ����
    {
        if(type < 0 || (int)type >= markerPrefabs.Length)
        {
            Debug.LogError("�Ǽ��� ������Ʈ Ÿ���̳� ��Ŀ�� �����ϴ�.");
            return;
        }

        GameObject markerOnMap = Instantiate(markerPrefabs[(int)type], mapTransform); //��ġ�� ��Ŀ �������� Ÿ�԰� �θ� ������Ʈ
        //Transform markerTransform = markerOnMap.GetComponent<Transform>();
        //markerOnMap.layer = LayerMask.NameToLayer("MiniMapObject"); //������ �ڵ����� MiniMapObject ���̾�� ����

        Vector3 pmMarkerPosition = new Vector3(position.x, 206f, position.z);
        markerOnMap.transform.position = pmMarkerPosition;
        markers.Add(markerOnMap); //����Ʈ�� ��Ŀ �߰�

        //Debug.Log($"��Ŀ�� �θ� ������Ʈ {markerOnMap.transform.parent.name}�� ������ {markerOnMap.transform.position} ��ġ�� �����ƽ��ϴ�.");
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
    //        Debug.LogWarning("������ ��Ŀ�� �����ϴ�.");
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
    //        Debug.LogError("mapWidth �Ǵ� mapHeight�� 0�Դϴ�. �ùٸ� ���� �����ϼ���.");
    //        return Vector3.zero;
    //    }
    //
    //    //Vector3 relativePosition = worldPosition - transform.position; // �̰� �ʿ���?? Ȯ�� �� 0722 09:29
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
