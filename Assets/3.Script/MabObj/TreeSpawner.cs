using System;
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
    private string bigTreeDirectory = "Resources/MapObj/bigTreeData";
    private string smallTreeDirectory = "Resources/MapObj/smallTreeData";

    private string bigTreeStaticFileName = "bigTreeData.json";
    private string smallTreeStaticFileName = "smallTreeData.json";

    private BigTreeDataList bigObjectList;
    private SmallTreeDataList smallObjectList;

    private void Start() {
        LoadTreeData();
    }

    private void LoadTreeData() {
        if (Save.Instance.saveData.isNewGame) {
            LoadBigTreeDataFromResources();
            LoadSmallTreeDataFromResources();
        }
        else {

            string time = Save.Instance.saveData.saveTime;
            string formattedSaveTime = time.Replace(':', '-');

            string bigTreeFilePath = NormalizePath(Path.Combine(Application.persistentDataPath, bigTreeDirectory, $"bigTreeData_{formattedSaveTime}.json"));
            string smallTreeFilePath = NormalizePath(Path.Combine(Application.persistentDataPath, smallTreeDirectory, $"smallTreeData_{formattedSaveTime}.json"));

            LoadBigTreeData(bigTreeFilePath);
            LoadSmallTreeData(smallTreeFilePath);
        }
    }

    private void LoadBigTreeDataFromResources() {
        TextAsset bigDataAsset = Resources.Load<TextAsset>(Path.Combine("MapObj", "bigTreeData"));
        if (bigDataAsset != null) {
            bigObjectList = JsonUtility.FromJson<BigTreeDataList>(bigDataAsset.text);
            SpawnBigTrees(bigObjectList.bigTreeObjs);
        }
        else {
            Debug.LogError("Big Tree Json Not Exist in Resources");
        }
    }

    private void LoadSmallTreeDataFromResources() {
        TextAsset smallDataAsset = Resources.Load<TextAsset>(Path.Combine("MapObj", "smallTreeData"));
        if (smallDataAsset != null) {
            smallObjectList = JsonUtility.FromJson<SmallTreeDataList>(smallDataAsset.text);
            SpawnSmallTrees(smallObjectList.smallTreeObjs);
        }
        else {
            Debug.LogError("Small Tree Json Not Exist in Resources");
        }
    }

    private void LoadBigTreeData(string filePath) {
        if (File.Exists(filePath)) {
            string bigDataAsJson = File.ReadAllText(filePath);
            bigObjectList = JsonUtility.FromJson<BigTreeDataList>(bigDataAsJson);
            SpawnBigTrees(bigObjectList.bigTreeObjs);
        }
        else {
            Debug.LogError($"Big Tree Json Not Exist: {filePath}");
        }
    }

    private void LoadSmallTreeData(string filePath) {
        if (File.Exists(filePath)) {
            string smallDataAsJson = File.ReadAllText(filePath);
            smallObjectList = JsonUtility.FromJson<SmallTreeDataList>(smallDataAsJson);
            SpawnSmallTrees(smallObjectList.smallTreeObjs);
        }
        else {
            Debug.LogError($"Small Tree Json Not Exist: {filePath}");
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

    public void SaveTreeData(string saveTime) {

        string formattedSaveTime = saveTime.Replace(':', '-');

        string bigTreeDataAsJson = JsonUtility.ToJson(bigObjectList, true);
        string smallTreeDataAsJson = JsonUtility.ToJson(smallObjectList, true);

        string bigTreeDirectoryPath = NormalizePath(Path.Combine(Application.persistentDataPath, bigTreeDirectory));
        string smallTreeDirectoryPath = NormalizePath(Path.Combine(Application.persistentDataPath, smallTreeDirectory));

        if (!Directory.Exists(bigTreeDirectoryPath)) {
            Directory.CreateDirectory(bigTreeDirectoryPath);
        }

        if (!Directory.Exists(smallTreeDirectoryPath)) {
            Directory.CreateDirectory(smallTreeDirectoryPath);
        }

        string bigTreeFilePath = NormalizePath(Path.Combine(bigTreeDirectoryPath, $"bigTreeData_{formattedSaveTime}.json"));
        string smallTreeFilePath = NormalizePath(Path.Combine(smallTreeDirectoryPath, $"smallTreeData_{formattedSaveTime}.json"));

        File.WriteAllText(bigTreeFilePath, bigTreeDataAsJson);
        File.WriteAllText(smallTreeFilePath, smallTreeDataAsJson);
    }

    private string NormalizePath(string path) {
        return path.Replace("\\", "/");
    }
}
