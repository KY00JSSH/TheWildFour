using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class ObjectData
{
    public int objectNumber;
    public Position position;
    public bool enable;
    public float health;
    public int type;
}

[System.Serializable]
public class ObjectDataList
{
    public List<ObjectData> treeObjs;
}

public class TreeSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    private string jsonFileName = "MapObj/treeCoord";
    private ObjectDataList objectDataList;

    private void Start()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>(jsonFileName);
        if (jsonTextAsset != null)
        {
            string dataAsJson = jsonTextAsset.text;
            objectDataList = JsonUtility.FromJson<ObjectDataList>(dataAsJson);
            SpawnObjects(objectDataList.treeObjs);
        }
        else
        {
            Debug.LogError("������ ��ã�Ҿ��!");
        }
    }

    private void SpawnObjects(List<ObjectData> objects)
    {
        foreach (ObjectData objData in objects)
        {
            Vector3 position = new Vector3(objData.position.x, objData.position.y, objData.position.z);
            GameObject newObj = Instantiate(objectPrefabs[objData.type - 1], position, Quaternion.identity);
            newObj.transform.SetParent(gameObject.transform);

            TreeController tree = newObj.GetComponent<TreeController>();
            tree.InitializeObjData(objData);
        }
    }

    public void UpdateTreeData(int objectNumber, bool enable, float health)
    {
        //TODO: ��������� ����� ������ ���������� ���� ���� ����Ÿ�̹� �����ϱ�

        var tree = objectDataList.treeObjs.FirstOrDefault(t => t.objectNumber == objectNumber);
        if (tree != null)
        {
            tree.enable = enable;
            tree.health = health;
            string updatedJson = JsonUtility.ToJson(objectDataList, true);
            File.WriteAllText(Path.Combine(Application.dataPath, "Resources", jsonFileName + ".json"), updatedJson);
        }
    }
}