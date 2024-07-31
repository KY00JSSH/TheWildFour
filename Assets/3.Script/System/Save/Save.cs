using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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
    public string saveTime;
    public bool isExtreme;
    public PlayerType playerType;

    public float WorldTime;
    public int SurviveDay;
    public float TotalDay;

    public Vector3 playerTransform;

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
    public int playerInvenCount;

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
    public int playerAddInvenCount;

    public int shelterLevel;
    public int shelterMoveLevel;
    public int shelterAttackLevel;
    public int shelterGatherLevel;
    public int shelterMovePoint;
    public int shelterAttackPoint;
    public int shelterGatherPoint;
    public float shelterMoveCurrentExp;
    public float shelterAttackCurrentExp;
    public float shelterGatherCurrentExp;
    public int workshopLevel;

    public Vector3 shelterPosition;
    public Quaternion shelterRotation;
    public Vector3 workshopPosition;
    public Quaternion workshopRotation;

    public List<Vector3> campfirePosition;
    public List<Vector3> furnacePosition;
    public List<Vector3> chestPosition;

    public int[] skillMoveLevel = new int[5];
    public int[] skillAttackLevel = new int[5];
    public int[] skillGatherLevel = new int[5];

    public List<int> ItemKey;
    public List<Vector3> ItemPosition;
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
        InitSaveFile();
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
    }

    public void MakeSave() {
        SaveDataList saveDataList = Load();
        if (saveDataList == null) {
            saveDataList = new SaveDataList { Data = new List<SaveData>() };
            File.WriteAllText(playerSaveJsonFilePath, JsonUtility.ToJson(saveDataList));
        }

        GetTargetSaveData();

        saveData.saveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
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

    public void GetTargetSaveData() {
        saveData.WorldTime = TimeManager.Instance.GetWorldTime();
        saveData.SurviveDay = TimeManager.Instance.GetSurviveDay();
        saveData.TotalDay = TimeManager.Instance.GetTotalDay();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            player = FindObjectOfType<PlayerManager>().ActivatePlayer();
        saveData.playerTransform = player.transform.position;

        ShelterManager shelterManager = FindObjectOfType<ShelterManager>();
        saveData.shelterLevel = shelterManager.ShelterLevel;
        saveData.shelterMoveLevel = shelterManager.MoveLevel;
        saveData.shelterAttackLevel = shelterManager.AttackLevel;
        saveData.shelterGatherLevel = shelterManager.GatherLevel;
        saveData.shelterMovePoint = shelterManager.MovePoint;
        saveData.shelterAttackPoint = shelterManager.AttackPoint;
        saveData.shelterGatherPoint = shelterManager.GatherPoint;
        saveData.shelterMoveCurrentExp = shelterManager.MoveCurrentExp;
        saveData.shelterAttackCurrentExp = shelterManager.AttackCurrentExp;
        saveData.shelterGatherCurrentExp = shelterManager.GatherCurrentExp;

        WorkshopManager workshopManager = FindObjectOfType<WorkshopManager>();
        saveData.workshopLevel = workshopManager.WorkshopLevel;

        saveData.shelterPosition = shelterManager.GetComponent<ShelterCreate>().Building.transform.position;
        saveData.workshopPosition = workshopManager.GetComponent<WorkshopCreate>().Building.transform.position;
        saveData.shelterRotation = shelterManager.GetComponent<ShelterCreate>().Building.transform.rotation;
        saveData.workshopRotation = workshopManager.GetComponent<WorkshopCreate>().Building.transform.rotation;

        CampfireChestCreate[] campfireChestCreates = FindObjectsOfType<CampfireChestCreate>();
        foreach(var eachCreate in campfireChestCreates) {
            foreach(Transform eachChild in eachCreate.transform) {
                if (eachChild.TryGetComponent(out Campfire campfire)) {
                    if (saveData.campfirePosition == null) saveData.campfirePosition = new List<Vector3>();
                    saveData.campfirePosition.Add(eachChild.position);
                }
                else if (eachChild.TryGetComponent(out Furnace furnace)) {
                    if (saveData.furnacePosition == null) saveData.furnacePosition = new List<Vector3>();
                    saveData.furnacePosition.Add(eachChild.position);
                }
                else {
                    if (saveData.chestPosition == null) saveData.chestPosition = new List<Vector3>();
                    saveData.chestPosition.Add(eachChild.position);
                }
            }
        }

        //TODO: 여기부터 연동 되있는지 확인
        for(int i = 0; i< saveData.skillMoveLevel.Length;i++) {
            saveData.skillMoveLevel[i] = shelterManager.skillMove[i].nowSkillLevel;
            saveData.skillAttackLevel[i] = shelterManager.skillAttack[i].nowSkillLevel;
            saveData.skillGatherLevel[i] = shelterManager.skillGather[i].nowSkillLevel;
        }

    }

    public void InitSaveFile() {
        saveData.WorldTime = 90f;
        saveData.SurviveDay = 0;
        saveData.TotalDay = (int)((saveData.WorldTime - 90f) / 360f);

        saveData.playerType = PlayerType.Ju;

        saveData.playerTransform = Vector3.zero;

        saveData.playerAttack = 2f;
        saveData.playerAttackSpeed = 1f;
        saveData.playerCriticalAttack = 5f;
        saveData.playerCriticalChance = 0.1f;
        saveData.playerColdResistance = 0f;
        saveData.playerDefense = 2f;
        saveData.playerGather = 2f;
        saveData.playerSpeed = 1f;
        saveData.playerDashSpeed = 2.5f;
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
        saveData.playerAddInvenCount = 0;

        saveData.shelterLevel = 1;
        saveData.shelterMoveLevel = 0;
        saveData.shelterAttackLevel = 0;
        saveData.shelterGatherLevel = 0;
        saveData.shelterMovePoint = 0;
        saveData.shelterAttackPoint = 0;
        saveData.shelterGatherPoint = 0;
        saveData.shelterMoveCurrentExp = 0;
        saveData.shelterAttackCurrentExp = 0;
        saveData.shelterGatherCurrentExp = 0;
        saveData.workshopLevel = 1;

        saveData.shelterPosition = Vector3.zero;
        saveData.workshopPosition = Vector3.zero;
        saveData.shelterRotation = Quaternion.identity;
        saveData.workshopRotation = Quaternion.identity;

        saveData.campfirePosition = null;
        saveData.furnacePosition = null;
        saveData.chestPosition = null;

        saveData.ItemKey = null;
        saveData.ItemPosition = null;
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
