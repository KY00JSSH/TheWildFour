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

    private string bigRockJsonFileName = "MapObj/bigRockData";
    private string midRockJsonFileName = "MapObj/midRockData";

    private RockDataList bigRockList;
    private RockDataList midRockList;

    private void Start() {
        TextAsset bigJsonTxt = Resources.Load<TextAsset>(bigRockJsonFileName);
        if (bigJsonTxt != null) {
            string bigDataAsJson = bigJsonTxt.text;
            bigRockList = JsonUtility.FromJson<RockDataList>(bigDataAsJson);
            SpawnRocks(bigRockList.rockObjs, true);
        }
        else {
            Debug.LogError("Big Rock Json Not Exist");
        }

        TextAsset midJsonTxt = Resources.Load<TextAsset>(midRockJsonFileName);
        if (midJsonTxt != null) {
            string midDataAsJson = midJsonTxt.text;
            midRockList = JsonUtility.FromJson<RockDataList>(midDataAsJson);
            SpawnRocks(midRockList.rockObjs, false);
        }
        else {
            Debug.LogError("Mid Rock Json Not Exist");
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

    public void SaveRockData() {
        string bigDataAsJson = JsonUtility.ToJson(bigRockList, true);
        string midDataAsJson = JsonUtility.ToJson(midRockList, true);

        string bigPath = Path.Combine(Application.dataPath, "Resources", $"{bigRockJsonFileName}.json");
        string midPath = Path.Combine(Application.dataPath, "Resources", $"{midRockJsonFileName}.json");

        File.WriteAllText(bigPath, bigDataAsJson);
        File.WriteAllText(midPath, midDataAsJson);
    }
}
