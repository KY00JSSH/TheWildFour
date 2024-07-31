using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class Position {
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class BigTreeData {
    public int objectNumber;
    public Position position;
    public bool enable;
    public float health;
    public int type;
}

[System.Serializable]
public class SmallTreeData {
    public int objectNumber;
    public Position position;
    public bool enable;
    public int berrCount;
    public int type;
}

[System.Serializable]
public class BigTreeDataList {
    public List<BigTreeData> bigTreeObjs;
}

[System.Serializable]
public class SmallTreeDataList {
    public List<SmallTreeData> smallTreeObjs;
}

public class TreeSpawner : MonoBehaviour {
    public GameObject[] objectPrefabs;
    private string bigTreeJsonFileName = "MapObj/bigTreeData";
    private string smallTreeJsonFileName = "MapObj/smallTreeData";
    private BigTreeDataList bigObjectList;
    private SmallTreeDataList smallObjectList;

    private void Start() {
        TextAsset bigJsonText = Resources.Load<TextAsset>(bigTreeJsonFileName);         //큰 나무 json Data Load
        if (bigJsonText != null) {
            string bigDataAsJson = bigJsonText.text;
            bigObjectList = JsonUtility.FromJson<BigTreeDataList>(bigDataAsJson);
            SpawnBigTrees(bigObjectList.bigTreeObjs);
        }
        else {
            Debug.LogError("BIG Tree Json Not Exist");
        }

        TextAsset smallJsonText = Resources.Load<TextAsset>(smallTreeJsonFileName);     //작은 나무 json Data Load
        if (smallJsonText != null) {
            string smallDataAsJson = smallJsonText.text;
            smallObjectList = JsonUtility.FromJson<SmallTreeDataList>(smallDataAsJson);
            SpawnSmallTrees(smallObjectList.smallTreeObjs);
        }
        else {
            Debug.LogError("SMALL Tree Json Not Exist");
        }
    }

    private void SpawnBigTrees(List<BigTreeData> objects) {
        foreach (BigTreeData objData in objects) {
            Vector3 position = new Vector3(objData.position.x, objData.position.y, objData.position.z);
            GameObject newObj = Instantiate(objectPrefabs[objData.type - 1], position, Quaternion.identity);
            newObj.transform.SetParent(gameObject.transform);

            TreeBigController treeB = newObj.GetComponent<TreeBigController>();
            treeB.InitializeObjData(objData);
        }

        Debug.Log(objects.Count);
    }

    private void SpawnSmallTrees(List<SmallTreeData> objects) {
        foreach (SmallTreeData objData in objects) {
            Vector3 position = new Vector3(objData.position.x, objData.position.y, objData.position.z);
            GameObject newObj = Instantiate(objectPrefabs[objData.type == 5 ? 4 : 5], position, Quaternion.identity);
            newObj.transform.SetParent(gameObject.transform);

            TreeSmallController treeS = newObj.GetComponent<TreeSmallController>();
            treeS.InitializeObjData(objData);
        }
    }

    public void UpdateTreeData(int objectNumber, bool enable, float health) {
        var tree = bigObjectList.bigTreeObjs.FirstOrDefault(t => t.objectNumber == objectNumber);
        if (tree != null) {
            tree.enable = enable;
            tree.health = health;
        }
    }

    public void SaveTreeData() {
        string bigTreeDataAsJson = JsonUtility.ToJson(bigObjectList, true);
        string smallTreeDataAsJson = JsonUtility.ToJson(smallObjectList, true);

        string bigTreeFilePath = Path.Combine(Application.dataPath, "Resources", bigTreeJsonFileName);
        string smallTreeFilePath = Path.Combine(Application.dataPath, "Resources", smallTreeJsonFileName);

        File.WriteAllText(bigTreeFilePath, bigTreeDataAsJson);
        File.WriteAllText(smallTreeFilePath, smallTreeDataAsJson);
    }
}
