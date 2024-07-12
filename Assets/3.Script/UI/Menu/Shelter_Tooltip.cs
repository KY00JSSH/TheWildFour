using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class SkillInfo {
    public string Title;
    public string MainText;

    public SkillInfo(string Title, string MainText) {
        this.Title = Title;
        this.MainText = MainText;
    }
}


public class Shelter_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    Dictionary<int, SkillInfo> shelterTooltip = new Dictionary<int, SkillInfo>();
    private int dictionaryKey;

    [SerializeField] private GameObject ShelterTooltip;
    private Image tooltipImg;
    private Text tooltipTitle;
    private Text tooltipMain;
    private int shelterLevel = 0;
    [SerializeField] private GameObject shelterLevelText;
    private ShelterManager shelterManager;    


    public TextAsset textFile;
    [SerializeField] private List<SkillInfo> skilltexts = new List<SkillInfo>();

    private void Awake() {
        shelterManager = FindObjectOfType<ShelterManager>();
        TextRead();
        tooltipImg = ShelterTooltip.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        tooltipTitle = ShelterTooltip.transform.GetChild(1).GetComponent<Text>();
        tooltipMain = ShelterTooltip.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        shelterLevelText = transform.GetChild(3).gameObject;
    }

    private void Update() {
        if (shelterLevel == shelterManager.ShelterLevel) ShelterLevel_Alpha();
    }

    // 설명글  dictionary에 저장
    private void TextRead() {
        string[] lines = textFile.text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

        for (int i = 0; i < lines.Length; i += 1) {
            string title = lines[i].Split('-')[0].Trim();
            string mainText = lines[i].Split('-')[1].Trim();
            SkillInfo newSkillInfo = new SkillInfo(title, mainText);
            skilltexts.Add(newSkillInfo);
            shelterTooltip.Add(i, skilltexts[i]);
        }
    }

    // 하위 버튼 마우스 위치 확인
    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("OnPointerEnter called");
        if (eventData.pointerEnter != null) {
            Debug.Log("Pointer Enter Object: " + eventData.pointerEnter.name);
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse enter");
                ShelterTooltipShow(btn.gameObject);

                Debug.Log("dictionaryKey 확인" + dictionaryKey); ;
            }
            else {
                Debug.Log("Not a button");
            }
        }
        else {
            Debug.Log("No object under pointer");
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("OnPointerExit called");
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Debug.Log("Pointer Exit Object: " + eventData.pointerCurrentRaycast.gameObject.name);
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse exit");
                dictionaryKey = 0;
            }
            else {
                Debug.Log("Exited non-button object");
            }
        }
        else {
            Debug.Log("Pointer exited to no object");
        }
    }

    // dictionary의 key 값 확인
    // 찾아진 오브젝트의 이름을 받아서 특정 포함되어있으면 값 더해서 리턴 
    private int FindDictionaryKey(GameObject btn) {
        int DictionKey = 0;
        char lastChar = btn.name[btn.name.Length - 1];
        int skillnum = 0;

        if (btn.name.Contains("Move")) {
            DictionKey = 0;
        }
        else if (btn.name.Contains("Attack")) {
            DictionKey = 5;
        }
        else if (btn.name.Contains("Gather")) {
            DictionKey = 10;
        }

        if (char.IsDigit(lastChar)) // Check if the last character is a digit
        {
            skillnum = lastChar - '1'; // Convert char to int
        }
        else {
            Debug.Log("Last character is not a number: " + lastChar);
        }

        DictionKey += skillnum;
        return DictionKey;
    }

    // dictionary의 key 값으로 tooltip에 넣기
    private void ShelterTooltipShow(GameObject btn) {

        // 이미지 변경
        tooltipImg.sprite = btn.gameObject.transform.GetChild(1).GetComponent<Image>().sprite;

        dictionaryKey = FindDictionaryKey(btn.gameObject);
        tooltipTitle.text = shelterTooltip[dictionaryKey].Title;
        tooltipMain.text = shelterTooltip[dictionaryKey].MainText;
    }

    // 레벨해야하는데 레벨받아와서 Text 알파값 조정
    private void ShelterLevel_Alpha() {
        Debug.Log("======================== 되는게 맞는건가 ");
        for (int i = 0; i < shelterLevelText.transform.childCount; i++) {
            char shelterLevelTextChar = shelterLevelText.transform.GetChild(i).name[0];
            int shelterLevelCount = 0;
            if (char.IsDigit(shelterLevelTextChar)) {
                shelterLevelCount = shelterLevelTextChar - '0';

                Text text = shelterLevelText.transform.GetChild(i).GetComponent<Text>();
                Color color = text.color;
                if (shelterLevelCount < shelterManager.ShelterLevel) {
                    color.a = 0.5f;
                }
                else {
                    color.a = 1f;
                }
            }
        }

    }
}
