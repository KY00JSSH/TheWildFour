using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;


[System.Serializable]
public class SkillInfo {
    public string Title;
    public string MainText;

    public SkillInfo(string Title, string MainText) {
        this.Title = Title;
        this.MainText = MainText;
    }
}


public class Shelter_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public int dictionaryKey;

    [Header("Tootip 정보")]
    [SerializeField] private ShelterManager shelterManager;
    [SerializeField] private GameObject ShelterTooltip;
    private Image tooltipImg;
    private Text tooltipTitle;
    private Text tooltipMain;
    private Text tooltipAdditionalText;

    // Function 용 추가
    [SerializeField] private Slider sleepTime;
    [SerializeField] private Button[] sleepOrAwake;
    [SerializeField] private Image[] itemimgs;
    private string[] additionalText = { "필요 포인트 1", "현재 필요 레벨" };


    [Space((int)2)]
    [Header("Skill Info")]
    Dictionary<int, SkillInfo> shelterTooltip = new Dictionary<int, SkillInfo>(); public TextAsset textFile_Skill;
    [SerializeField] private List<SkillInfo> skilltexts = new List<SkillInfo>();

    // Function 용 추가
    [Space((int)2)]
    [Header("Funcion Info")]
    public TextAsset textFile_Func;
    [SerializeField]
    private Dictionary<int, Dictionary<int, ShelterFunctionInfo>> shelterFuncTooltip
        = new Dictionary<int, Dictionary<int, ShelterFunctionInfo>>(){
        { 0, new Dictionary<int, ShelterFunctionInfo>() },
        { 1, new Dictionary<int, ShelterFunctionInfo>() },
        { 2, new Dictionary<int, ShelterFunctionInfo>() }
    };

    private void Awake() {
        TextRead();
        TextReadFunc();
        tooltipImg = ShelterTooltip.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        tooltipTitle = ShelterTooltip.transform.GetChild(1).GetComponent<Text>();
        tooltipMain = ShelterTooltip.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        tooltipAdditionalText = ShelterTooltip.transform.GetChild(0).GetChild(2).GetComponent<Text>();
    }

    // 설명글  dictionary에 저장
    private void TextRead() {
        string[] lines = textFile_Skill.text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

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

        // Function 추가
        else if (btn.name.Contains("Sleep")) DictionKey = 60;
        else if (btn.name.Contains("Upgrade")) DictionKey = 70;
        else if (btn.name.Contains("Packing")) DictionKey = 80;

        if (dictionaryKey <= 50) {
            if (char.IsDigit(lastChar)) skillnum = lastChar - '1';
            else Debug.Log("Last character is not a number: " + lastChar);
            DictionKey += skillnum;
            return DictionKey;
        }
        else return DictionKey;
    }

    // dictionary의 key 값으로 tooltip에 넣기
    //TODO: 레벨 맞춰서 하단 추가정보란 표기
    public void ShelterTooltipShow(GameObject btn) {
        dictionaryKey = FindDictionaryKey(btn.gameObject);
        Debug.Log("dictionaryKey" + dictionaryKey);
        // skill 설명란
        if (dictionaryKey <= 50) {
            // 이미지 변경

            tooltipImg.gameObject.SetActive(true);
            tooltipImg.sprite = btn.gameObject.transform.GetChild(1).GetComponent<Image>().sprite;
            tooltipTitle.text = shelterTooltip[dictionaryKey].Title;
            tooltipMain.text = shelterTooltip[dictionaryKey].MainText;
            AdditionalText(dictionaryKey);
        }
        //TODO: Function 추가 예정
        else {

            FunctionTooltip(btn, dictionaryKey);
        }
    }

    /* 추가 정보란
     * 비교 대상 : 버튼의 dictionaryKey vs 현재 쉘터 레벨
     * 포인트 가져오는 법 : 해당 버튼의 키의 범위로 해당 포인트 들고오기 
    1. 레벨이 작을 경우 : 최소 레벨 -> 붉은 글씨
    2. 레벨이 맞으나 포인트가 없는 경우 : 필요 포인트 -> 붉은 글씨
    3. 레벨 맞으며 포인트 있음 : 필요 포인트 -> 하얀 글씨
    */

    private void AdditionalText(int dictionaryKey) {
        if ((dictionaryKey % 5 + 1) > shelterManager.ShelterLevel)
            tooltipAdditionalText.text = string.Format("<color=red>{0} : {1}</color>", additionalText[1], dictionaryKey % 5 + 1);
        else {
            string textColor = "white";
            if (0 <= dictionaryKey && dictionaryKey < 5) { textColor = shelterManager.MovePoint > 0 ? "white" : "red"; }
            else if (5 <= dictionaryKey && dictionaryKey < 10) { textColor = shelterManager.AttackPoint > 0 ? "white" : "red"; }
            else if (10 <= dictionaryKey && dictionaryKey < 15) { textColor = shelterManager.GatherPoint > 0 ? "white" : "red"; }

            tooltipAdditionalText.text = string.Format("<color={1}>{0}</color>", additionalText[0], textColor);
        }
    }



    // ============ ShelterFunction Test ============

    // Function 용 텍스트 파일 읽기 추가
    private void TextReadFunc() {
        string[] lines = textFile_Func.text.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        List<ShelterFunctionInfo> sleepFunc = new List<ShelterFunctionInfo>();
        List<ShelterFunctionInfo> shelterFunc = new List<ShelterFunctionInfo>();
        List<ShelterFunctionInfo> packingFunc = new List<ShelterFunctionInfo>();

        for (int i = 0; i < lines.Length; i++) {

            // Tooltip 내용 - 아이템
            string[] parts = lines[i].Split('/');
            string[] titleAndText = parts[0].Trim().Split('-');

            if (lines[i].Contains("Sleep")) {
                // 1번째 - 제목 / 내용
                string title = titleAndText[1].Trim();
                string mainText = titleAndText[2].Trim();

                // 2번째 - 아이템
                string[] resources = parts[1].Trim().Split('-');
                int woodneed = int.Parse(resources[0].Trim());
                int stoneneed = int.Parse(resources[1].Trim());
                int[] itemneed = { woodneed, stoneneed };

                // 1. 자기
                ShelterFunctionInfo newFunc = new ShelterFunctionInfo(title, mainText, itemneed, sleepTime, sleepOrAwake);
                sleepFunc.Add(newFunc);
                shelterFuncTooltip[0].Add(sleepFunc.Count - 1, newFunc);
            }
            else if (lines[i].Contains("Shelter")) {
                // 1번째 - 제목 / 내용
                string title = titleAndText[1].Trim();
                string mainText = titleAndText[2].Trim();

                // 2번째 - 아이템
                string[] resources = parts[1].Trim().Split('-');
                int woodneed = int.Parse(resources[0].Trim());
                int stoneneed = int.Parse(resources[1].Trim());
                int leatherneed = int.Parse(resources[2].Trim());
                int steelneed = int.Parse(resources[3].Trim());
                int[] itemneed = { woodneed, stoneneed, leatherneed, steelneed };

                // 2. 거처 업데이트
                ShelterFunctionInfo newFunc = new ShelterFunctionInfo(title, mainText, itemimgs, itemneed);
                shelterFunc.Add(newFunc);
                shelterFuncTooltip[1].Add(shelterFunc.Count - 1, newFunc);
            }
            else if (lines[i].Contains("Packing")) {
                // 1번째 - 제목 / 내용
                string title = titleAndText[1].Trim();
                string mainText = titleAndText[2].Trim();

                // 3. 짐 싸기
                ShelterFunctionInfo newFunc = new ShelterFunctionInfo(title, mainText);
                packingFunc.Add(newFunc);
                shelterFuncTooltip[2].Add(packingFunc.Count - 1, newFunc);
            }
        }
    }

    private void FunctionTooltip(GameObject btn, int dictionaryKey) {
        /*
         1. 기존 스킬나오는 이미지 비활성화 혹은 text부분만 크기 키움
         2.  dictionaryKey 기준으로 시발이다 진짜
         */


        tooltipImg.gameObject.SetActive(false);
        switch (dictionaryKey) {
            case 60:
                // 1. 자기
                tooltipTitle.text = shelterFuncTooltip[0][0].Title;
                tooltipMain.text = shelterFuncTooltip[0][0].MainText;
                break;
            case 70:
                // 2. 업그레이드
                tooltipTitle.text = shelterFuncTooltip[1][shelterManager.ShelterLevel - 1].Title;
                tooltipMain.text = shelterFuncTooltip[1][shelterManager.ShelterLevel - 1].MainText;
                break;
            case 80:
                // 3. 짐 싸기
                tooltipTitle.text = shelterFuncTooltip[2][0].Title;
                tooltipMain.text = shelterFuncTooltip[2][0].MainText;
                tooltipAdditionalText.text = null;
                break;
            default:
                Debug.Log("FunctionTooltip 오류임");
                break;
        }

    }


}

// ============ ShelterFunction Test ============

public class ShelterFunctionInfo {
    /* shelter 기능
     * 1. 자기 2. 업그레이드 3. 짐싸기 
     - 메인 text의 사용 개수가 다름 : List 배열로 저장 고려 중
     */
    public string Title;
    public string MainText;
    public Image[] ItemNeeds;
    public int[] ItemNeedNum;
    public Slider sleepTime;
    public Button[] sleepOrAwake;

    // 1. 자기
    public ShelterFunctionInfo(string Title, string MainText, int[] ItemNeedNum, Slider sleepTime, Button[] sleepOrAwake) {
        this.Title = Title;
        this.MainText = MainText;
        this.ItemNeedNum = ItemNeedNum;
        this.sleepTime = sleepTime;
        this.sleepOrAwake = sleepOrAwake;
    }

    // 2. 업그레이드
    public ShelterFunctionInfo(string Title, string MainText, Image[] ItemNeeds, int[] ItemNeedNum) {
        this.Title = Title;
        this.MainText = MainText;
        this.ItemNeeds = ItemNeeds;
        this.ItemNeedNum = ItemNeedNum;
    }

    // 3. 짐싸기 
    public ShelterFunctionInfo(string Title, string MainText) {
        this.Title = Title;
        this.MainText = MainText;
    }

}

