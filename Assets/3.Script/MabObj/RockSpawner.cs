using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

[System.Serializable]
public class RockData {
    public int objectNumber;
    public Position position;
    public bool enable;
    public float health;
    public int type;
}

[System.Serializable]
public class RockDataList {
    public List<RockData> rockObjs;
}

public class RockSpawner : MonoBehaviour {
    public GameObject[] objectPrefabs;

    private string bigRockDirectory = "Resources/MapObj/bigRockData";
    private string midRockDirectory = "Resources/MapObj/midRockData";

    private string bigRockStaticFileName = "bigRockData.json";
    private string midRockStaticFileName = "midRockData.json";

    private RockDataList bigRockList;
    private RockDataList midRockList;

    private void Start() {
        LoadRockData();
    }

    private void LoadRockData() {
        if (Save.Instance.saveData.isNewGame) {
            LoadBigRockDataFromResources();
            LoadMidRockDataFromResources();
        }
        else {
            string bigRockFilePath = NormalizePath(Path.Combine(Application.persistentDataPath, bigRockDirectory, $"bigRockData_{Save.Instance.saveData.saveTime}.json"));
            string midRockFilePath = NormalizePath(Path.Combine(Application.persistentDataPath, midRockDirectory, $"midRockData_{Save.Instance.saveData.saveTime}.json"));

            LoadBigRockData(bigRockFilePath);
            LoadMidRockData(midRockFilePath);
        }
    }
    private void LoadBigRockDataFromResources() {
        TextAsset bigDataAsset = Resources.Load<TextAsset>(Path.Combine("MapObj", "bigRockData"));
        if (bigDataAsset != null) {
            bigRockList = JsonUtility.FromJson<RockDataList>(bigDataAsset.text);
            SpawnRocks(bigRockList.rockObjs, true);
        }
        else {
            Debug.LogError("Big Rock Json Not Exist in Resources");
        }
    }

    private void LoadMidRockDataFromResources() {
        TextAsset midDataAsset = Resources.Load<TextAsset>(Path.Combine("MapObj", "midRockData"));
        if (midDataAsset != null) {
            midRockList = JsonUtility.FromJson<RockDataList>(midDataAsset.text);
            SpawnRocks(midRockList.rockObjs, false);
        }
        else {
            Debug.LogError("Mid Rock Json Not Exist in Resources");
        }
    }

    private void LoadBigRockData(string filePath) {
        if (File.Exists(filePath)) {
            string bigDataAsJson = File.ReadAllText(filePath);
            bigRockList = JsonUtility.FromJson<RockDataList>(bigDataAsJson);
            SpawnRocks(bigRockList.rockObjs, true);
        }
        else {
            Debug.LogError($"Big Rock Json Not Exist: {filePath}");
        }
    }

    private void LoadMidRockData(string filePath) {
        if (File.Exists(filePath)) {
            string midDataAsJson = File.ReadAllText(filePath);
            midRockList = JsonUtility.FromJson<RockDataList>(midDataAsJson);
            SpawnRocks(midRockList.rockObjs, false);
        }
        else {
            Debug.LogError($"Mid Rock Json Not Exist: {filePath}");
        }
    }

    private void SpawnRocks(List<RockData> rocks, bool isBig) {
        foreach (RockData objData in rocks) {
            Vector3 position = new Vector3(objData.position.x, objData.position.y, objData.position.z);
            GameObject newObj = Instantiate(isBig ? objectPrefabs[0] : objectPrefabs[1], position, Quaternion.identity);
            newObj.transform.SetParent(gameObject.transform);

            RockController rock = newObj.GetComponent<RockController>();
            rock.InitializeObjData(objData);
        }
    }


    public void UpdateRockData(int objectNumber, bool enable, float health, bool isBig) {
        if (isBig) {
            var rock = bigRockList.rockObjs.FirstOrDefault(r => r.objectNumber == objectNumber);
            if (rock != null) {
                rock.enable = enable;
                rock.health = health;
            }
        }
        else {
            var rock = midRockList.rockObjs.FirstOrDefault(r => r.objectNumber == objectNumber);
            if (rock != null) {
                rock.enable = enable;
                rock.health = health;
            }
        }
    }

    public void SaveRockData(string saveTime) {
        string bigDataAsJson = JsonUtility.ToJson(bigRockList, true);
        string midDataAsJson = JsonUtility.ToJson(midRockList, true);

        string bigRockDirectoryPath = NormalizePath(Path.Combine(Application.persistentDataPath, bigRockDirectory));
        string midRockDirectoryPath = NormalizePath(Path.Combine(Application.persistentDataPath, midRockDirectory));

        if (!Directory.Exists(bigRockDirectoryPath)) {
            Directory.CreateDirectory(bigRockDirectoryPath);
        }

        if (!Directory.Exists(midRockDirectoryPath)) {
            Directory.CreateDirectory(midRockDirectoryPath);
        }

        string bigRockFilePath =Path.Combine(bigRockDirectoryPath, $"bigRockData_{saveTime}.json");
        string midRockFilePath =Path.Combine(midRockDirectoryPath, $"midRockData_{saveTime}.json");

        File.WriteAllText(bigRockFilePath, bigDataAsJson);
        File.WriteAllText(midRockFilePath, midDataAsJson);
    }
    private string NormalizePath(string path) {
        return path.Replace("\\", "/");
    }
}
