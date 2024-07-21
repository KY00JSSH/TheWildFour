using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMap_MarkerSpawner : MonoBehaviour
{
    /*
    1. �� ������ ��ں��� �������� �迭�� ���� �ֱ�
    2. �޼���� Ÿ��, ��ġ�� �� �谳������ �ϴ� �� �ۼ��ؼ� �ٸ� ��ũ��Ʈ���� ����� �� �� �ְ� �ϱ�
    2-1. ������ �����͸� ����Ʈ�� ���� �ֱ� - �ǹ� ������Ʈ �����̴ϱ�
    2-2. ����Ʈ�� ��ȸ�ؼ� ��ġ���� �ش��ϴ� �̴ϸ� ��ġ�� ������ ����
    3. ���� ��ġ�� �̴ϸ��� ��ġ�� �����ؼ� �̴ϸʿ� ��Ÿ���� �ϱ�
    4. ����/�ƿ��� ���� �̴ϸ� ������ ��ġ ���������� �����ǰ� �ϱ�
    5. �г� �̴ϸʿ��� ������ �߰� �ϱ�
    5-1. �г� �̴ϸ��� ������ �� ��������Ʈ�� ũ�Ⱑ 1:1�̴ϱ� �� ��ġ �״�� ���� ������ ��    
    */

    [SerializeField] public GameObject[] markerPrefabs;
    private List<RectTransform> markers = new List<RectTransform>();

    public RectTransform mapRect;

    public float mapWidth  = 250f;
    public float mapHeight = 250f;

    public MenuMapZoom zoom;

    private void Start()
    {
        zoom = FindObjectOfType<MenuMapZoom>();
        if (zoom == null)
        {
            Debug.LogError("MenuMapZoom ��ü�� �����ϴ�. Zoom ��ü�� ã���ּ���.");
        }
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
            Debug.LogError("�Ǽ��� ������Ʈ Ÿ���̳� ��Ŀ�� �����ϴ�.");
            return;
        }

        GameObject markerOnMap = Instantiate(markerPrefabs[(int)type], /*transform.Find("MenuMapSprite")*/ mapRect);
        RectTransform markerRect = markerOnMap.GetComponent<RectTransform>();

        Vector3 correctedPosition = WorldToMapPosition(position);
        markerRect.anchoredPosition = correctedPosition;
        markers.Add(markerRect);
    }

    public void RemoveMarker(BuildingType type, Vector3 position)
    {
        RectTransform markerToRemove = null;

        foreach(RectTransform markerOnMap in markers)
        {
            //if(markerOnMap.transform.position == position && markerOnMap.name.Contains(type.ToString()))
            //{
            //    markerToRemove = markerOnMap;
            //    break;
            //}

            if (Vector3.Distance(markerOnMap.transform.position, position) < 0.1f && markerOnMap.name.Contains(type.ToString()))
            {
                markerToRemove = markerOnMap;
                break;
            }
        }
        if(markerToRemove != null)
        {
            markers.Remove(markerToRemove);
            Destroy(markerToRemove.gameObject);
        }
        else
        {
            Debug.LogWarning("������ ��Ŀ�� �����ϴ�.");
        }
    }

    private void Update()
    {
        foreach(var each in markers)
        {
            //each.transform.localPosition = WorldToMapPosition(each.transform.position);

            //each.anchoredPosition = WorldToMapPosition(each.position);

            Vector3 worldPos = MapToWorldPosition(each.anchoredPosition);
            each.anchoredPosition = WorldToMapPosition(worldPos);
        }
    }

    private Vector3 WorldToMapPosition(Vector3 worldPosition) //LerpPosition = ��Ŀ�� ���� ��ǥ
    {
        if (mapWidth == 0 || mapHeight == 0)
        {
            Debug.LogError("mapWidth �Ǵ� mapHeight�� 0�Դϴ�. �ùٸ� ���� �����ϼ���.");
            return Vector3.zero;
        }

        //���� ��ǥ�� �̴ϸ� ��ǥ�� ��ȯ
        Vector3 relativePosition = worldPosition - transform.position;

        //�̴ϸ��� ���� ũ�⿡ ���� ���� ���
        float normalizedX = relativePosition.x / mapWidth;
        float normalizedY = relativePosition.z / mapHeight;

        //��ǥ ����
        float ratio =
            Mathf.InverseLerp(zoom.maxOrthSize, zoom.minOrthSize, zoom.menuMapCamera.orthographicSize) *
            (zoom.maxOrthSize / zoom.minOrthSize - 1) + 1;

        float markerPosX = normalizedX * mapRect.rect.width * ratio;
        float markerPosY = normalizedY * mapRect.rect.height * ratio;

        if (float.IsNaN(markerPosX) || float.IsInfinity(markerPosX) || float.IsNaN(markerPosY) || float.IsInfinity(markerPosY))
        {
            Debug.LogError("���� ��Ŀ ��ġ�� ��ȿ���� �ʽ��ϴ�. �Է°��� Ȯ���ϼ���.");
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
