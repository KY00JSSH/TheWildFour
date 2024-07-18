using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// ��ų ������ ��������Ʈ ���� �ҷ�����
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
        if (skillSprites.Length == 0) Debug.LogError("��� ���� : " + path);
        else Debug.Log(skillSprites.Length + " ���� ��������Ʈ ���: " + path);
    }

    private void SeparateSprites() {
        for (int i = 0; i < skillSprites.Length; i++) {
            string spriteName = skillSprites[i].name;
            char lastChar = spriteName[spriteName.Length - 1];

            if (char.IsDigit(lastChar)) {
                int number = lastChar - '1';
                if (number >= 0 && number < skillCnt.Length) skillCnt[number].Add(skillSprites[i]);
            }
            else Debug.Log($"Sprite {spriteName} ������ ���ڰ� ���� �ƴ�");
        }
    }
}


//TODO: Inven => ��ũ��Ʈ ���� ������ ��ư �̺�Ʈ �߰� �ʿ���
public class ShelterUI : MonoBehaviour {

    private ShelterManager shelterManager;

    [Header("Skill Splite Change")]
    [SerializeField] private string spritesPath = "4.Sprite/UI/Shelter_Upgrade/SkillCount";
    [SerializeField] private SkillCountImg skillCountImg;
    [Space((int)2)]
    [Header("Skill Level Alpha Change")]
    [SerializeField] private GameObject shelterLevelText; // ��ų �� ���� ǥ��
    [SerializeField] private Text shelterLevel;       // shelter ���� ǥ��
    [SerializeField] private GameObject shelterbuttons;
    [Space((int)2)]
    [Header("Main Button Disapear")]
    [SerializeField] private GameObject menuButton;
    [Space((int)2)]
    [Header("Skill Slider")]
    [SerializeField] private Slider[] sliders;
    [Space((int)1)]
    [Header("Skill Pointer")]
    [SerializeField] private Text[] pointers;



    private void Awake() {
        shelterManager = FindObjectOfType<ShelterManager>();
        shelterLevelText = transform.GetChild(2).gameObject;
        skillCountImg.Init(spritesPath);        
    }
    private void Start() {
        ShelterLevel_Alpha();
        ShelterLevel_AlphaBtns();
    }

    private void OnEnable() {
        ShelterLevel_Alpha();
        ShelterLevel_AlphaBtns();
        menuButton.SetActive(false);
    }
    private void OnDisable() {
        menuButton.SetActive(true);
    }

    private void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape)) Escape();
        SkillSliderValue();
        SkillPointerValue();
    }

    // ShelterManager �����޾ƿͼ� Text ���İ� ����
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


    // ShelterManager �����޾ƿͼ� ��ư ��� Sprite ����
    public void ShelterLevel_AlphaBtns() {

        shelterLevel.text = string.Format("<size=50>{0}</size>\n<size=30>����</size>", shelterManager.ShelterLevel);

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

    //TODO: �ش� ��ư�� ������ ��ų �Һ� ���� �� ��
    //TODO: ��ư ����Ʈ ���� ������ ����ǰ� �Ұ�
    /*
     1. �ش� ��ư�� ������ ��ư�� ������ü 3���� �̸� ������ ���� Ȯ�� -> ��ųī��Ʈ ����
     2. �ش� ������ ���ڷ� SkillCountImgŬ������ ��������Ʈ list�迭 �ҷ�����
     3. ���� ��������Ʈ �̸��� �������� �迭�� ������ ���Ͽ� ������ �̹������ return
     4. �ƴ϶�� ��ü
     
     */
    public void HandleButtonClick(GameObject clickedButton) {
        Button btn = clickedButton.GetComponent<Button>();

        // ��ư ����Ʈ���� ������ return
        if (btn.name.Contains("Move") && shelterManager.MovePoint <= 0) return;
        else if (btn.name.Contains("Attack") && shelterManager.AttackPoint <= 0) return;
        else if (btn.name.Contains("Gather") && shelterManager.GatherPoint <= 0) return; 


        if (btn != null) {
            string nowBtnSpriteName = btn.transform.GetChild(2).GetComponent<Image>().sprite.name;

            char lastChar = nowBtnSpriteName[nowBtnSpriteName.Length - 1];
            if (char.IsDigit(lastChar)) {
                int skillCntNum = lastChar - '0'; //  ��������Ʈ list�迭�� ��ų ī��Ʈ���

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


    // �����̴� ��ġ ����(update)
    public void SkillSliderValue() {
        for (int i = 0; i < sliders.Length; i++) {
            if (sliders[i].name.Contains("Move")) sliders[i].value = shelterManager.MoveCurrentExp / shelterManager.MoveTotalExp;
            else if (sliders[i].name.Contains("Attack")) sliders[i].value = shelterManager.AttackCurrentExp / shelterManager.AttackTotalExp;
            else if (sliders[i].name.Contains("Gather")) sliders[i].value = shelterManager.GatherCurrentExp / shelterManager.GatherTotalExp;
        }
    }

    // ������ ���� ����(update)
    public void SkillPointerValue() {
        for (int i = 0; i < pointers.Length; i++) {
            if (pointers[i].name.Contains("Move")) pointers[i].text = string.Format("{0} Pts", shelterManager.MovePoint);
            else if (pointers[i].name.Contains("Attack")) pointers[i].text = string.Format("{0} Pts", shelterManager.AttackPoint);
            else if (pointers[i].name.Contains("Gather")) pointers[i].text = string.Format("{0} Pts", shelterManager.GatherPoint);
        }
    }

    // ���α� -> escape

    public void Escape() {
        menuButton.SetActive(true);
        transform.gameObject.SetActive(false);
    }
}
