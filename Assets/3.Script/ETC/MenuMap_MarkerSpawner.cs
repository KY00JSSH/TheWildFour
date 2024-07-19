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
            Debug.LogError("�Ǽ��� ������Ʈ Ÿ���̳� ��Ŀ�� �����ϴ�. ������ �밡�� �ھ�");
            return;
        }

        GameObject markerOnMap = Instantiate(markerPrefabs[(int)type], transform);

        Vector3 correctedPosition = WorldToMapPosition(position); /**/
        markerOnMap.transform.localPosition = correctedPosition;
        markers.Add(markerOnMap);
    }

    public void RemoveMarker(BuildingType type, Vector3 position)
    {
        GameObject markerToRemove = null;

        foreach(GameObject markerOnMap in markers)
        {
            if(markerOnMap.transform.position == position && markerOnMap.name.Contains(type.ToString()))
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
            Debug.LogWarning("������ ��Ŀ�� �����ϴ�.");
        }
    }

    private void Update()
    {
        foreach(var each in markers)
        {
            each.transform.localPosition = WorldToMapPosition(each.transform.position);
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
}
