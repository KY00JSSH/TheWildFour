using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    
}

[System.Serializable]
public class PlayerDataList {
    public List<PlayerData> Data;
}
public class Save : MonoBehaviour {
    public static string saveName = null;
    private string playerSaveJsonFilePath;

    public void SetFilePath(string fileName) {
        playerSaveJsonFilePath = Path.Combine("Save/", saveName, ".json");
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
