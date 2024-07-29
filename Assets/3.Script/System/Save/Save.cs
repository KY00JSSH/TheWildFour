using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType {
    Jin,
    Hyeon,
    Hun,
    Ju
}

[System.Serializable]
public class SaveData {
    public string SaveName;
    public DateTime saveTime;
    public bool isExtreme;
    public PlayerType playerType;

    public float WorldTime;
    public int SurviveDay;
    public float TotalDay;

    public float playerAttack;
    public float playerAttackSpeed;
    public float playerCriticalAttack;
    public float playerCriticalChance;
    public float playerColdResistance;
    public float playerDefense;
    public float playerGather;
    public float playerSpeed;
    public float playerDashSpeed;
    public float playerDecDashGage;
    public float playerInvenCount;

    public float playerAddAttack;
    public float playerAddAttackSpeed;
    public float playerAddCriticalAttack;
    public float playerAddCriticalChance;
    public float playerAddColdResistance;
    public float playerAddDefense;
    public float playerAddGather;
    public float playerAddSpeed;
    public float playerAddDashSpeed;
    public float playerAddDecDashGage;
    public float playerAddInvenCount;
}

[System.Serializable]
public class SaveDataList {
    public List<SaveData> Data;
}
public class Save : MonoBehaviour {
    public static Save Instance = null;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public SaveData saveData = new SaveData();
    private string playerSaveJsonFilePath;

    public void SetFilePath() {
        saveData.SaveName = FindObjectOfType<InputField>().textComponent.text;
    }

    private void Start() {
        playerSaveJsonFilePath = Path.Combine(Application.persistentDataPath, "Save/saveData.json");
        if (!Directory.Exists(Path.GetDirectoryName(playerSaveJsonFilePath))) {
            Directory.CreateDirectory(Path.GetDirectoryName(playerSaveJsonFilePath));
        }
        saveData.WorldTime = 90f;
        saveData.SurviveDay = 0;
        saveData.TotalDay = (int)((saveData.WorldTime - 90f) / 360f);

        saveData.playerAttack = 2f;
        saveData.playerAttackSpeed = 1f;
        saveData.playerCriticalAttack = 5f;
        saveData.playerCriticalChance = 0.1f;
        saveData.playerColdResistance = 0f;
        saveData.playerDefense = 2f;
        saveData.playerGather = 2f;
        saveData.playerSpeed = 1f;
        saveData.playerDashSpeed = 2.5f;
        saveData.playerDecDashGage = 8f;
        saveData.playerInvenCount = 8;

        saveData.playerAddAttack = 0f;
        saveData.playerAddAttackSpeed = 0f;
        saveData.playerAddCriticalAttack = 0f;
        saveData.playerAddCriticalChance = 0f;
        saveData.playerAddColdResistance = 0f;
        saveData.playerAddDefense = 0f;
        saveData.playerAddGather = 0f;
        saveData.playerAddSpeed = 0f;
        saveData.playerAddDashSpeed = 0f;
        saveData.playerAddInvenCount = 0f;
        saveData.playerAddDecDashGage = 0f;


}

    public void MakeSave() {
        SaveDataList saveDataList = Load();
        if (saveDataList == null) {
            saveDataList = new SaveDataList { Data = new List<SaveData>() };
            File.WriteAllText(playerSaveJsonFilePath, JsonUtility.ToJson(saveDataList));
        }

        saveData.saveTime = DateTime.Now;
        saveDataList.Data.Add(saveData);
        if (saveDataList.Data.Count > 6) saveDataList.Data.RemoveAt(0);

        File.WriteAllText(playerSaveJsonFilePath, JsonUtility.ToJson(saveDataList));
    }

    public SaveDataList Load() {
        if (File.Exists(playerSaveJsonFilePath)) {
            return JsonUtility.FromJson<SaveDataList>(File.ReadAllText(playerSaveJsonFilePath));
        }
        return null;
    }
}

// 1. 뉴 게임시 세이브파일 이름을 입력받아서
// 2. 세이브 파일을 생성하고
// 3. 캐릭터 선택화면 넘어가서
// 4. 캐릭터 타입을 입력받아서
// 5. 타입에 따른 기초 능력치 설정 후 게임 시작

// 1. 로드 게임시 세이브 파일 리스트를 출력해서
// 2. 세이브 파일 이름, 날짜, 시간, totalDay, 캐릭터 종류를 출력하고
// 3. 선택된 세이브 파일로 능력치 설정 후 게임 시작
