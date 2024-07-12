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

public class ShelterUI : MonoBehaviour {

    private ShelterManager shelterManager;

    [SerializeField] private string spritesPath = "4.Sprite/UI/Shelter_Upgrade/SkillCount";
    [SerializeField] private SkillCountImg skillCountImg;

    [SerializeField] private GameObject shelterLevelText; // ��ų �� ���� ǥ��
    [SerializeField] private Text shelterLevel;       // shelter ���� ǥ��
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
    /*
     1. �ش� ��ư�� ������ ��ư�� ������ü 3���� �̸� ������ ���� Ȯ�� -> ��ųī��Ʈ ����
     2. �ش� ������ ���ڷ� SkillCountImgŬ������ ��������Ʈ list�迭 �ҷ�����
     3. ���� ��������Ʈ �̸��� �������� �迭�� ������ ���Ͽ� ������ �̹������ return
     4. �ƴ϶�� ��ü
     
     */
    public void HandleButtonClick(GameObject clickedButton) {
        Debug.Log("HandleButtonClick called");

        Button btn = clickedButton.GetComponent<Button>();
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

}
