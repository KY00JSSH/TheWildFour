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
public class BigTreeData
{
    public int objectNumber;
    public Position position;
    public bool enable;
    public float health;
    public int type;
}

[System.Serializable]
public class SmallTreeData
{
    public int objectNumber;
    public Position position;
    public bool enable;
    public int berrCount;
    public int type;
}

[System.Serializable]
public class BigTreeDataList
{
    public List<BigTreeData> bigTreeObjs;
}
[System.Serializable]
public class SmallTreeDataList
{
    public List<SmallTreeData> smallTreeObjs;
}
public class TreeSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    private string bigTreeJsonFileName = "MapObj/bigTreeData";
    private string smallTreeJsonFileName = "MapObj/smallTreeData";
    private BigTreeDataList bigObjectList;
    private SmallTreeDataList smallObjectList;

    private void Start()
    {
        TextAsset bigJsonText = Resources.Load<TextAsset>(bigTreeJsonFileName);
        if (bigJsonText != null)
        {
            string bigDataAsJson = bigJsonText.text;
            bigObjectList = JsonUtility.FromJson<BigTreeDataList>(bigDataAsJson);
            SpawnBigTrees(bigObjectList.bigTreeObjs);
        }
        else
        {
            Debug.LogError("BIG Tree Json Not Exist");
        }

        TextAsset smallJsonText = Resources.Load<TextAsset>(smallTreeJsonFileName);
        if (smallJsonText != null)
        {
            string smallDataAsJson = smallJsonText.text;
            smallObjectList = JsonUtility.FromJson<SmallTreeDataList>(smallDataAsJson);
            SpawnSmallTrees(smallObjectList.smallTreeObjs);
        }
        else
        {
            Debug.LogError("SMALL Tree Json Not Exist");
        }
    }

    private void SpawnBigTrees(List<BigTreeData> objects)
    {
        foreach (BigTreeData objData in objects)
        {
            Vector3 position = new Vector3(objData.position.x, objData.position.y, objData.position.z);
            GameObject newObj = Instantiate(objectPrefabs[objData.type - 1], position, Quaternion.identity);
            newObj.transform.SetParent(gameObject.transform);

            TreeBigController treeB = newObj.GetComponent<TreeBigController>();
            treeB.InitializeObjData(objData);
        }
    }

    private void SpawnSmallTrees(List<SmallTreeData> objects)
    {
        foreach (SmallTreeData objData in objects)
        {
            Vector3 position = new Vector3(objData.position.x, objData.position.y, objData.position.z);
            Debug.Log("222" + objData.type);
            GameObject newObj = Instantiate(objectPrefabs[objData.type==5 ? 4 : 5], position, Quaternion.identity);
            newObj.transform.SetParent(gameObject.transform);

            TreeSmallController treeS = newObj.GetComponent<TreeSmallController>();
            treeS.InitializeObjData(objData);
        }
    }

    public void UpdateTreeData(int objectNumber, bool enable, float health)
    {
        //TODO: 저장시점이 현재는 데미지 받을때마다 여서 추후 저장타이밍 수정하기

        var tree = bigObjectList.bigTreeObjs.FirstOrDefault(t => t.objectNumber == objectNumber);
        if (tree != null)
        {
            tree.enable = enable;
            tree.health = health;
            string updatedJson = JsonUtility.ToJson(bigObjectList, true);
            File.WriteAllText(Path.Combine(Application.dataPath, "Resources", bigTreeJsonFileName + ".json"), updatedJson);
        }
    }
}