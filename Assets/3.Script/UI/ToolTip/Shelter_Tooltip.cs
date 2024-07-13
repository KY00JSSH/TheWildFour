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

    [Space((int)2)]
    [Header("Tootip Function 추가")]
    // Function 용 추가
    [SerializeField] private Slider sleepTime;
    [SerializeField] private GameObject sleepFunc;
    [SerializeField] private GameObject itemimgs;
    [SerializeField] private GameObject itemtexts;

    //[SerializeField] private Button[] sleepOrAwake;
    private string[] additionalText = { "필요 포인트 1", "현재 필요 레벨" };
    private Vector2[] textPositionSave;

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
        if (tooltipImg == null) tooltipImg = ShelterTooltip.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        if (tooltipTitle == null) tooltipTitle = ShelterTooltip.transform.GetChild(1).GetComponent<Text>();
        if (tooltipMain == null) tooltipMain = ShelterTooltip.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        if (tooltipAdditionalText == null) tooltipAdditionalText = ShelterTooltip.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        textPositionSave = new Vector2[2];
        // 위치 변경되는 Text 위치 저장
        SaveTextPositions();
    }

    private void SaveTextPositions() {
        RectTransform tooltipMainRe = tooltipMain.GetComponent<RectTransform>();
        textPositionSave[0] = new Vector2(tooltipMainRe.anchoredPosition.x, tooltipMainRe.anchoredPosition.y);
        RectTransform tooltipAdditionalTextRe = tooltipAdditionalText.GetComponent<RectTransform>();
        textPositionSave[1] = new Vector2(tooltipAdditionalTextRe.anchoredPosition.x, tooltipAdditionalTextRe.anchoredPosition.y);
    }

    // 저장된 Text 위치 불러오기 + Function 객체들 초기화
    private void LoadTextPositions() {
        // 배경 길이 초기화
        RectTransform tooltipBg = ShelterTooltip.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 BgnewPosition = tooltipBg.anchoredPosition;
        Vector2 BgSize = tooltipBg.sizeDelta;
        BgSize.y = 350;
        tooltipBg.sizeDelta = BgSize;
        BgnewPosition.y = 0;
        tooltipBg.anchoredPosition = BgnewPosition;

        // Text 위치 크기 초기화
        RectTransform tooltipMainRe = tooltipMain.GetComponent<RectTransform>();
        tooltipMainRe.anchoredPosition = new Vector2(textPositionSave[0].x, textPositionSave[0].y);
        Vector2 MainnewSize = tooltipMainRe.sizeDelta;
        MainnewSize.x = 270;
        tooltipMainRe.sizeDelta = MainnewSize;


        RectTransform tooltipAdditionalTextRe = tooltipAdditionalText.GetComponent<RectTransform>();
        tooltipAdditionalTextRe.anchoredPosition = new Vector2(textPositionSave[1].x, textPositionSave[1].y);
        Vector2 AddnewPosition = tooltipAdditionalTextRe.anchoredPosition;
        AddnewPosition.y = 60;
        tooltipAdditionalTextRe.anchoredPosition = AddnewPosition;

        sleepTime.gameObject.SetActive(false);
        itemimgs.SetActive(false);
        itemtexts.SetActive(false);

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
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) ShelterTooltipShow(btn.gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn != null) dictionaryKey = 0;
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
            LoadTextPositions();
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
                ShelterFunctionInfo newFunc = new ShelterFunctionInfo(title, mainText, itemneed, sleepTime);
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
         * 0. 배경 길이 변경
         1. 기존 스킬나오는 이미지 비활성화 
         2. dictionaryKey 기준
         3. 내용 Text 위치 변경
         */

        tooltipImg.gameObject.SetActive(false);
        sleepTime.gameObject.SetActive(false);
        itemimgs.gameObject.SetActive(false);
        itemtexts.gameObject.SetActive(false);

        // 배경 길이 변경
        RectTransform tooltipBg = ShelterTooltip.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 BgnewPosition = tooltipBg.anchoredPosition;
        Vector2 BgSize = tooltipBg.sizeDelta;
        BgSize.y = 450;
        tooltipBg.sizeDelta = BgSize;
        BgnewPosition.y = 0;
        tooltipBg.anchoredPosition = BgnewPosition;

        // 내용 Text 위치 변경
        RectTransform tooltipMainRe = tooltipMain.GetComponent<RectTransform>();
        Vector2 MainnewPosition = tooltipMainRe.anchoredPosition;
        MainnewPosition.x = 0;
        tooltipMainRe.anchoredPosition = MainnewPosition;
        Vector2 MainnewSize = tooltipMainRe.sizeDelta;
        MainnewSize.x = 350;
        tooltipMainRe.sizeDelta = MainnewSize;

        // 추가 설명 Text 위치 변경
        RectTransform tooltipAddRe = tooltipAdditionalText.GetComponent<RectTransform>();
        Vector2 AddnewPosition = tooltipAddRe.anchoredPosition;
        AddnewPosition.y = 180;
        tooltipAddRe.anchoredPosition = AddnewPosition;

        switch (dictionaryKey) {
            case 60:
                // 1. 자기
                tooltipTitle.text = shelterFuncTooltip[0][0].Title;
                tooltipMain.text = shelterFuncTooltip[0][0].MainText;
                tooltipAdditionalText.text = "";
                // 슬라이터 활성화
                sleepTime.gameObject.SetActive(true);
                break;
            case 70:
                // 2. 업그레이드

                tooltipTitle.text = shelterFuncTooltip[1][shelterManager.ShelterLevel].Title;
                tooltipMain.text = shelterFuncTooltip[1][shelterManager.ShelterLevel].MainText;
                tooltipAdditionalText.text = "필요 구성품";

                // 아이템 이미지 활성화

                itemimgs.gameObject.SetActive(true);
                foreach (Transform child in itemimgs.transform) {
                    child.gameObject.SetActive(true);
                }

                // 아이템 텍스트 변경
                //TODO: 아이템 Inven에서 받아와야함
                itemtexts.gameObject.SetActive(true);
                foreach (Transform child in itemtexts.transform) {
                    child.gameObject.SetActive(true);
                }

                break;
            case 80:
                // 3. 짐 싸기
                tooltipTitle.text = shelterFuncTooltip[2][0].Title;
                tooltipMain.text = shelterFuncTooltip[2][0].MainText;
                tooltipAdditionalText.text = "";
                break;
            default:
                Debug.Log("FunctionTooltip 오류임");
                break;
        }

    }

    public void SleepFuncOnClick() {
        StopCoroutine("SleepSliderValue");
        Text text = sleepTime.GetComponentInChildren<Text>();
        //Todo: 코르틴 슬라이더 값 조절
        StartCoroutine(SleepSliderValue(text, sleepTime));
    }

    private IEnumerator SleepSliderValue(Text text, Slider sleepTime) {
        // 시간...
        string[] timeParts = text.text.Split(':');
        int minutes = int.Parse(timeParts[0]);
        int seconds = int.Parse(timeParts[1]);
        float totalSeconds = minutes * 60 + seconds;
        float decrement = 1 / totalSeconds;

        while (totalSeconds > 0) {
            yield return new WaitForSeconds(1f);
            totalSeconds--;
            minutes = (int)(totalSeconds / 60);
            seconds = (int)(totalSeconds % 60);
            sleepTime.value -= decrement;
            text.text = string.Format("{0}:{1:D2}", minutes, seconds);
        }
        sleepTime.value = 0;
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
    public GameObject ItemNeeds;
    public int[] ItemNeedNum;
    public Slider sleepTime;
    //public Button[] sleepOrAwake;

    // 1. 자기
    public ShelterFunctionInfo(string Title, string MainText, int[] ItemNeedNum, Slider sleepTime) {
        this.Title = Title;
        this.MainText = MainText;
        this.ItemNeedNum = ItemNeedNum;
        this.sleepTime = sleepTime;
    }

    // 2. 업그레이드
    public ShelterFunctionInfo(string Title, string MainText, GameObject ItemNeeds, int[] ItemNeedNum) {
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

