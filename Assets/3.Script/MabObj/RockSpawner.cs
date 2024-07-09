using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RockData
{
    public int objectNumber;
    public Position position;
    public bool enable;
    public float health;
    public int type;
}

[System.Serializable]
public class RockDataList
{
    public List<RockData> rockObjs;
}

public class RockSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;

    private string bigRockJsonFileName = "MapObj/bigRockData";
    private string midRockJsonFileName = "MapObj/midRockData";

    private RockDataList bigRockList;
    private RockDataList midRockList;

    private void Start()
    {
        TextAsset bigJsonTxt = Resources.Load<TextAsset>(bigRockJsonFileName);
        if (bigJsonTxt != null)
        {
            string bigDataAsJson = bigJsonTxt.text;
            bigRockList = JsonUtility.FromJson<RockDataList>(bigDataAsJson);
            SpawnRocks(bigRockList.rockObjs, true);
        }
        else
        {
            Debug.LogError("Big Rock Json Not Exist");
        }

        TextAsset midJsonTxt = Resources.Load<TextAsset>(midRockJsonFileName);
        if (midJsonTxt != null)
        {
            string midDataAsJson = midJsonTxt.text;
            midRockList = JsonUtility.FromJson<RockDataList>(midDataAsJson);
            SpawnRocks(midRockList.rockObjs, false);
        }
        else
        {
            Debug.LogError("Mid Rock Json Not Exist");
        }
    }

    private void SpawnRocks(List<RockData> rocks, bool isBig)
    {
        foreach (RockData objData in rocks)
        {
            Vector3 position = new Vector3(objData.position.x, objData.position.y, objData.position.z);
            GameObject newObj = Instantiate(isBig ? objectPrefabs[0] : objectPrefabs[1], position, Quaternion.identity);
            newObj.transform.SetParent(gameObject.transform);

            RockController rock = newObj.GetComponent<RockController>();
            rock.InitializeObjData(objData);
        }
    }
}
