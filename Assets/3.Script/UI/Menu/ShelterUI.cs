using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 스킬 레벨업 스프라이트 파일 불러오기
[System.Serializable]
public class SkillCountImg {

    private Sprite[] skillSprites;

    [SerializeField] public List<Sprite>[] skillCnt = new List<Sprite>[8];


    public void Init(string path) {
        for (int i = 0; i < skillCnt.Length; i++) {
            skillCnt[i] = new List<Sprite>();
        }
        LoadSprites(path);
        SeparateSprites();
    }
    private void LoadSprites(string path) {
        skillSprites = Resources.LoadAll<Sprite>(path);


        if (skillSprites.Length == 0) Debug.LogError("경로 오류 : " + path);
        else Debug.Log(skillSprites.Length + " 현재 스프라이트 경로: " + path);
    }

    private void SeparateSprites() {
        for (int i = 0; i < skillSprites.Length; i++) {
            string spriteName = skillSprites[i].name;
            char lastChar = spriteName[spriteName.Length - 1];

            if (char.IsDigit(lastChar)) {
                int number = lastChar - '1';
                if (number >= 0 && number < skillCnt.Length) skillCnt[number].Add(skillSprites[i]);
            }
            else Debug.Log($"Sprite {spriteName} 마지막 글자가 숫자 아님");
        }
    }
}

public class ShelterUI : MonoBehaviour {

    private ShelterManager shelterManager;

    [SerializeField] private string spritesPath = "4.Sprite/UI/Shelter_Upgrade/SkillCount";
    [SerializeField] private SkillCountImg skillCountImg;

    [SerializeField] private GameObject shelterLevelText; // 스킬 위 레벨 표시
    [SerializeField] private Text shelterLevel;       // shelter 레벨 표시
    [SerializeField] private GameObject shelterbuttons;

    [SerializeField] private Shelter_Tooltip shelter_Tooltip;

    private void Awake() {
        shelter_Tooltip = GetComponent<Shelter_Tooltip>();
        shelterManager = FindObjectOfType<ShelterManager>();
        shelterLevelText = transform.GetChild(2).gameObject;
        skillCountImg.Init(spritesPath);
    }

    private void OnEnable() {
        ShelterLevel_Alpha();
        ShelterLevel_AlphaBtns();
    }


    // ShelterManager 레벨받아와서 Text 알파값 조정
    public void ShelterLevel_Alpha() {
        for (int i = 0; i < shelterLevelText.transform.childCount; i++) {
            char shelterLevelTextChar = shelterLevelText.transform.GetChild(i).name[0];
            int shelterLevelCount = 0;
            if (char.IsDigit(shelterLevelTextChar)) {
                shelterLevelCount = shelterLevelTextChar - '0';

                Text text = shelterLevelText.transform.GetChild(i).GetComponent<Text>();
                Color color = text.color;

                if (shelterLevelCount > shelterManager.ShelterLevel) color.a = 0.5f;
                else color.a = 1f;

                text.color = color;
            }
        }
    }


    // ShelterManager 레벨받아와서 버튼 잠김 Sprite 변경
    public void ShelterLevel_AlphaBtns() {

        shelterLevel.text = string.Format("<size=50>{0}</size>\n<size=30>레벨</size>", shelterManager.ShelterLevel);

        for (int i = 0; i < shelterbuttons.transform.childCount; i++) {
            GameObject gameObject = shelterbuttons.transform.GetChild(i).gameObject;
            for (int j = 3; j < gameObject.transform.childCount; j++) {

                GameObject buttonImg = gameObject.transform.GetChild(j).GetChild(1).gameObject;
                Color color = buttonImg.GetComponent<Image>().color;

                if ((j - 2) > shelterManager.ShelterLevel) {
                    color.a = 0.5f;
                    gameObject.transform.GetChild(j).Find("LockImg").gameObject.SetActive(true);
                    gameObject.transform.GetChild(j).GetComponent<Button>().interactable = false;
                }
                else {
                    color.a = 1f;
                    gameObject.transform.GetChild(j).Find("LockImg").gameObject.SetActive(false);
                    gameObject.transform.GetChild(j).GetComponent<Button>().interactable = true;
                }

                buttonImg.GetComponent<Image>().color = color;
            }
        }
    }

    //TODO: 해당 버튼이 눌리면 스킬 불빛 변경 할 것
    /*
     1. 해당 버튼이 눌리면 버튼의 하위객체 3번의 이름 마지막 글자 확인 -> 스킬카운트 갯수
     2. 해당 글자의 숫자로 SkillCountImg클래스의 스프라이트 list배열 불러오기
     3. 현재 스프라이트 이름의 마지막과 배열의 개수를 비교하여 마지막 이미지라면 return
     4. 아니라면 교체
     
     */
    public void HandleButtonClick(GameObject clickedButton) {
        Debug.Log("HandleButtonClick called");

        Button btn = clickedButton.GetComponent<Button>();
        if (btn != null) {
            string nowBtnSpriteName = btn.transform.GetChild(2).GetComponent<Image>().sprite.name;
            char lastChar = nowBtnSpriteName[nowBtnSpriteName.Length - 1];

            if (char.IsDigit(lastChar)) {
                int skillCntNum = lastChar - '0'; //  스프라이트 list배열의 스킬 카운트계산

                List<Sprite> skillSprites = skillCountImg.skillCnt[skillCntNum - 1];
                int spriteIndex = skillSprites.FindIndex(sprite => sprite.name == nowBtnSpriteName);

                if (spriteIndex >= (skillSprites.Count - 1)) return;
                else btn.transform.GetChild(2).GetComponent<Image>().sprite = skillSprites[spriteIndex + 1];
            }
        }
        else {
            Debug.Log("Not a button");
        }
    }

}
