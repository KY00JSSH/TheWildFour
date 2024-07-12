using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
public class ButtonTest : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Button Clicked!");
    }
}

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
    public int dictionaryKey;

    [SerializeField] private GameObject ShelterTooltip;
    private Image tooltipImg;
    private Text tooltipTitle;
    private Text tooltipMain;

    public TextAsset textFile;
    [SerializeField] private List<SkillInfo> skilltexts = new List<SkillInfo>();

    private void Awake() {
        TextRead();
        tooltipImg = ShelterTooltip.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        tooltipTitle = ShelterTooltip.transform.GetChild(1).GetComponent<Text>();
        tooltipMain = ShelterTooltip.transform.GetChild(0).GetChild(1).GetComponent<Text>();
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
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse enter");
                ShelterTooltipShow(btn.gameObject);

                Debug.Log("dictionaryKey 확인" + dictionaryKey); ;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("OnPointerExit called");
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn != null) {
                Debug.Log(btn.gameObject.name + " - Mouse exit");
                dictionaryKey = 0;
            }
        }

    }

    // dictionary의 key 값 확인
    // 찾아진 오브젝트의 이름을 받아서 특정 포함되어있으면 값 더해서 리턴 
    private int FindDictionaryKey(GameObject btn) {
        int DictionKey = 0;
        char lastChar = btn.name[btn.name.Length - 1];
        int skillnum = 0;

        if (btn.name.Contains("Move")) DictionKey = 0;
        else if (btn.name.Contains("Attack")) DictionKey = 5;
        else if (btn.name.Contains("Gather")) DictionKey = 10;

        if (char.IsDigit(lastChar)) skillnum = lastChar - '1';
        else Debug.Log("Last character is not a number: " + lastChar);

        DictionKey += skillnum;
        return DictionKey;
    }

    // dictionary의 key 값으로 tooltip에 넣기
    public void ShelterTooltipShow(GameObject btn) {

        // 이미지 변경
        tooltipImg.sprite = btn.gameObject.transform.GetChild(1).GetComponent<Image>().sprite;

        dictionaryKey = FindDictionaryKey(btn.gameObject);
        tooltipTitle.text = shelterTooltip[dictionaryKey].Title;
        tooltipMain.text = shelterTooltip[dictionaryKey].MainText;
    }


}
