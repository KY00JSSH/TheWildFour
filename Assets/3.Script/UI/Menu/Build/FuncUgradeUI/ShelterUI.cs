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


public class ShelterUI : UIInfo {

    private ShelterManager shelterManager;

    [Header("Skill Splite Change")]
    [SerializeField] private string spritesPath = "4.Sprite/UI/Shelter_Upgrade/SkillCount";
    [SerializeField] private SkillCountImg skillCountImg;
    [Space((int)2)]
    [Header("Skill Level Alpha Change")]
    [SerializeField] private GameObject shelterLevelText; // 스킬 위 레벨 표시
    [SerializeField] private Text shelterLevel;       // shelter 레벨 표시
    [SerializeField] private GameObject shelterbuttons; // 스킬만 모아놓은 object
    [Space((int)2)]
    [Header("Skill Slider")]
    [SerializeField] private Slider[] sliders;
    [Space((int)1)]
    [Header("Skill Pointer")]
    [SerializeField] private Text[] pointers;


    static public bool isShelterUIOpen { get { return _isShelterUIOpen; } }
    static private bool _isShelterUIOpen = false;

    private void Awake() {
        shelterManager = FindObjectOfType<ShelterManager>();
        shelterLevelText = transform.GetChild(2).gameObject;
        skillCountImg.Init(spritesPath);
        ShelterLevelSkillListInit();
    }
    private void Start() {
        ShelterLevel_Alpha();
        ShelterLevel_AlphaBtns();
    }

    protected override void OnEnable() {
        base.OnEnable();
        _isShelterUIOpen = true;
        ShelterLevel_Alpha();
        ShelterLevel_AlphaBtns();

        // 스킬 스프라이트 표시
        ShelterLevelSkillSpriteInit(moveSkill, shelterManager.skillMove);
        ShelterLevelSkillSpriteInit(attackSkill, shelterManager.skillAttack);
        ShelterLevelSkillSpriteInit(gatherSkill, shelterManager.skillGather);
    }
    protected override void OnDisable() {
        _isShelterUIOpen = false;
        base.OnDisable();
    }
    protected override void Update() {
        base.Update();
        SkillSliderValue();
        SkillPointerValue();
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

    /*
     1. 해당 버튼이 눌리면 버튼의 하위객체 3번의 이름 마지막 글자 확인 -> 스킬카운트 갯수
     2. 해당 글자의 숫자로 SkillCountImg클래스의 스프라이트 list배열 불러오기
     3. 현재 스프라이트 이름의 마지막과 배열의 개수를 비교하여 마지막 이미지라면 return
     4. 아니라면 교체     
     */
    private List<GameObject> moveSkill = new List<GameObject>();
    private List<GameObject> attackSkill = new List<GameObject>();
    private List<GameObject> gatherSkill = new List<GameObject>();

    public bool isShleterSkillAvailable = false;

    // 버튼 list 저장 
    private void ShelterLevelSkillListInit() {
        moveSkill.Clear();
        attackSkill.Clear();
        gatherSkill.Clear();
        for (int i = 0; i < shelterbuttons.transform.childCount; i++) {
            GameObject parent = shelterbuttons.transform.GetChild(i).gameObject;
            for (int j = 3; j < parent.transform.childCount; j++) {
                switch (i) {
                    case 0:
                        moveSkill.Add(parent.transform.GetChild(j).gameObject);
                        break;
                    case 1:
                        attackSkill.Add(parent.transform.GetChild(j).gameObject);
                        break;
                    case 2:
                        gatherSkill.Add(parent.transform.GetChild(j).gameObject);
                        break;
                    default:
                        break;
                }
            }
        }
    }


    // 현재 쉘터 레벨받아서 스프라이트 표기
    private void ShelterLevelSkillSpriteInit(List<GameObject> skillList, Skill[] skill) {
        for (int i = 0; i < skillList.Count; i++) {
            List<Sprite> skillSprites = skillCountImg.skillCnt[skill[i].maxSkillLevel - 1];
            int spriteIndex = skill[i].nowSkillLevel ;
            skillList[i].transform.GetChild(2).GetComponent<Image>().sprite = skillSprites[spriteIndex];
        }
    }


    private bool CheckSkillPointMaxSkillLevel(Button btn) {
        int skillNum = 0;
        if (btn != null) {
            skillNum = int.Parse(btn.name[btn.name.Length - 1].ToString()) - 1;
        }

        // 버튼 포인트값이 없으면 false and 버튼 최대 레벨이면 false
        if (btn.name.Contains("Move")) {
            if (shelterManager.MovePoint <= 0
                || shelterManager.skillMove[skillNum].nowSkillLevel == shelterManager.skillMove[skillNum].maxSkillLevel) return false;
            else return true;
        }
        else if (btn.name.Contains("Attack")) {
            if (shelterManager.AttackPoint <= 0
                || shelterManager.skillAttack[skillNum].nowSkillLevel == shelterManager.skillAttack[skillNum].maxSkillLevel) return false;
            else return true;
        }
        else if (btn.name.Contains("Gather")) {
            if (shelterManager.GatherPoint <= 0
                || shelterManager.skillGather[skillNum].nowSkillLevel == shelterManager.skillGather[skillNum].maxSkillLevel) return false;
            else return true;
        }

        return false;
    }


    public void HandleButtonClick(GameObject clickedButton) {
        Button btn = clickedButton.GetComponent<Button>();

        // 버튼 포인트값이 없으면 return
        if (!CheckSkillPointMaxSkillLevel(btn)) {
            isShleterSkillAvailable = false;
            return;
        }
        else {
            isShleterSkillAvailable = true;
        }

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


    // 슬라이더 수치 변경(update)
    public void SkillSliderValue() {
        for (int i = 0; i < sliders.Length; i++) {
            if (sliders[i].name.Contains("Move")) sliders[i].value = shelterManager.MoveCurrentExp / shelterManager.MoveTotalExp;
            else if (sliders[i].name.Contains("Attack")) sliders[i].value = shelterManager.AttackCurrentExp / shelterManager.AttackTotalExp;
            else if (sliders[i].name.Contains("Gather")) sliders[i].value = shelterManager.GatherCurrentExp / shelterManager.GatherTotalExp;
        }

    }

    // 포인터 글자 변경(update)
    public void SkillPointerValue() {
        pointers[0].text = string.Format("{0} Pts", shelterManager.MovePoint);
        pointers[1].text = string.Format("{0} Pts", shelterManager.AttackPoint);
        pointers[2].text = string.Format("{0} Pts", shelterManager.GatherPoint);
    }

    // 짐싸기 -> escape 
    // 상속에 있음

    public override void Escape() {
        base.Escape();
        ShelterCreate shelterCreate = FindObjectOfType<ShelterCreate>();
        shelterCreate.Building.GetComponent<BuildingInteraction>().PlayerExitBuilding<ShelterCreate>();
    }

}
