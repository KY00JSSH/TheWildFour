using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
}

[System.Serializable]
public class ObjectDataList
{
    public List<ObjectData> treeObjs;
}

public class InitTreeObj : MonoBehaviour
{
    public GameObject objectPrefab;
    private string jsonFileName = "MapObj/treeCoord";

    void Start()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>(jsonFileName);
        if (jsonTextAsset != null)
        {
            string dataAsJson = jsonTextAsset.text;
            ObjectDataList objectDataList = JsonUtility.FromJson<ObjectDataList>("{\"objects\":" + dataAsJson + "}");
            SpawnObjects(objectDataList.treeObjs);
        }
        else
        {
            Debug.LogError("파일을 못찾았어요!");
        }
    }

    void SpawnObjects(List<ObjectData> objects)
    {
        foreach (ObjectData objData in objects)
        {
            Vector3 position = new Vector3(objData.position.x, objData.position.y, objData.position.z);
            Instantiate(objectPrefab, position, Quaternion.identity);
        }
    }
}
