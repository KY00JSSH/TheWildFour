
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Tooltip_Shelter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {


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
    [SerializeField] private GameObject itemimgs;
    [SerializeField] private GameObject itemtexts;
    public Text shelterLevelText;

    private string[] additionalText = { "필요 포인트 1", "현재 필요 레벨" };
    private Vector2[] textPositionSave;
    private Vector2[][] itemNeedPositionSave;


    private TooltipNum tooltipNum;
    private SkillDetail currentskillDetail;
    private SleepDetail currentsleepDetail;
    private UpgradeDetail currentupgradeDetail;
    private PackingDetail currentpackingDetail;

    private Button Tooltip_btn;
    public bool isUpgradeAvailable { get; private set; }

    private void Awake() {

        tooltipNum = FindObjectOfType<TooltipNum>();
        if (tooltipImg == null) tooltipImg = ShelterTooltip.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        if (tooltipTitle == null) tooltipTitle = ShelterTooltip.transform.GetChild(1).GetComponent<Text>();
        if (tooltipMain == null) tooltipMain = ShelterTooltip.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        if (tooltipAdditionalText == null) tooltipAdditionalText = ShelterTooltip.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        textPositionSave = new Vector2[2];
        itemNeedPositionSave = new Vector2[2][];
        itemNeedPositionSave[0] = new Vector2[4];
        itemNeedPositionSave[1] = new Vector2[4];
        // 위치 변경되는 Text 위치 저장
        SaveTextPositions();
        SaveTextPositions_Func();
    }

    private void Update() {
        if (sleepTime.value < 0) {
            SleepSliderInit();
        }
        if(ShelterTooltip.activeSelf) {
            if (365 <= Tooltip_btn.transform.position.y && Tooltip_btn.transform.position.y < 440) {
                UpgradeFunc_ItemText();
            }
        }
    }

    private void SaveTextPositions() {
        RectTransform tooltipMainRe = tooltipMain.GetComponent<RectTransform>();
        textPositionSave[0] = tooltipMainRe.anchoredPosition;
        RectTransform tooltipAdditionalTextRe = tooltipAdditionalText.GetComponent<RectTransform>();
        textPositionSave[1] = tooltipAdditionalTextRe.anchoredPosition;
    }
    private void SaveTextPositions_Func() {
        for (int i = 0; i < itemtexts.transform.childCount; i++) {
            RectTransform eachRe = itemtexts.transform.GetChild(i).GetComponent<RectTransform>();
            itemNeedPositionSave[0][i] = eachRe.anchoredPosition;
        }
        for (int i = 0; i < itemimgs.transform.childCount; i++) {
            RectTransform eachRe = itemimgs.transform.GetChild(i).GetComponent<RectTransform>();
            itemNeedPositionSave[1][i] = eachRe.anchoredPosition;
        }
    }

    // 저장된 Text 위치 불러오기 + Function 객체들 비활성화
    private void LoadTextPositions() {
        // 배경 길이 초기화
        RectTransform tooltipBg = ShelterTooltip.transform.GetChild(0).GetComponent<RectTransform>();
        tooltipBg.sizeDelta = new Vector2(tooltipBg.sizeDelta.x, 350);
        tooltipBg.anchoredPosition = new Vector2(tooltipBg.anchoredPosition.x, 0);

        // Text 위치 크기 초기화
        RectTransform tooltipMainRe = tooltipMain.GetComponent<RectTransform>();
        tooltipMainRe.anchoredPosition = textPositionSave[0];
        tooltipMainRe.sizeDelta = new Vector2(270, tooltipMainRe.sizeDelta.y);

        RectTransform tooltipAdditionalTextRe = tooltipAdditionalText.GetComponent<RectTransform>();
        tooltipAdditionalTextRe.anchoredPosition = new Vector2(textPositionSave[1].x, 20);

        sleepTime.gameObject.SetActive(false);
        itemimgs.SetActive(false);
        itemtexts.SetActive(false);
    }
    private void LoadTextPositions_Func() {
        for (int i = 0; i < itemtexts.transform.childCount; i++) {
            RectTransform eachRe = itemtexts.transform.GetChild(i).GetComponent<RectTransform>();
            eachRe.anchoredPosition = itemNeedPositionSave[0][i];
        }
        for (int i = 0; i < itemimgs.transform.childCount; i++) {
            RectTransform eachRe = itemimgs.transform.GetChild(i).GetComponent<RectTransform>();
            eachRe.anchoredPosition = itemNeedPositionSave[1][i];
        }
    }


    // 하위 버튼 마우스 위치 확인
    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null && !btn.name.Contains("Exit")) {
                ShelterTooltip.SetActive(true);
                Tooltip_btn = btn;
                // 선택
                if (eventData.position.y >= 520) {
                    // 위치 초기화
                    ShelterTooltip.SetActive(true);
                    LoadTextPositions();
                    tooltipImg.gameObject.SetActive(true);
                    tooltipImg.sprite = btn.gameObject.transform.GetChild(1).GetComponent<Image>().sprite;

                    // skill type 나누는 메소드
                    SkillType skillType = FindSkillType(btn.gameObject);
                    currentskillDetail = tooltipNum.ShelterItemCheck(skillType, btn.gameObject);
                    SkillTooltipShow();
                }
                else {

                    LoadTextPositions_Func();
                    FunctionTooltipInit();
                    if (eventData.position.x >= 210) {// 창고 위치 막음
                        if (440 <= eventData.position.y && eventData.position.y < 520) {
                            currentsleepDetail = tooltipNum.SleepItemCheck();
                            SleepTooltipShow();
                        }
                        else if (365 <= eventData.position.y && eventData.position.y < 440) {
                            currentupgradeDetail = tooltipNum.UpgradeItemCheck(UpgradeType.Shelter, shelterManager.ShelterLevel + 1);
                            UpgradeTooltipShow();
                        }
                        else {
                            currentpackingDetail = tooltipNum.PackingItemCheck();
                            PackingTooltipShow();
                        }
                    }
                }


            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn != null) {
                ShelterTooltip.SetActive(false);
            }
        }

    }

    // ================================== Skill 

    // skill 
    public void SkillTooltipShow() {
        // 이미지 변경
        tooltipTitle.text = currentskillDetail.name;
        tooltipMain.text = currentskillDetail.description;
        AdditionalText();
    }
    private void AdditionalText() {
        if (currentskillDetail.skillNum > shelterManager.ShelterLevel)
            tooltipAdditionalText.text = string.Format("<color=red>{0} : {1}</color>", additionalText[1], currentskillDetail.skillNum);
        else {
            string textColor = "white";
            if (currentskillDetail.skillType == SkillType.Move) { textColor = shelterManager.MovePoint > 0 ? "white" : "red"; }
            else if (currentskillDetail.skillType == SkillType.Attack) { textColor = shelterManager.AttackPoint > 0 ? "white" : "red"; }
            else if (currentskillDetail.skillType == SkillType.Gather) { textColor = shelterManager.GatherPoint > 0 ? "white" : "red"; }

            tooltipAdditionalText.text = string.Format("<color={1}>{0}</color>", additionalText[0], textColor);
        }
    }
    private SkillType FindSkillType(GameObject btn) {
        if (btn.name.Contains("Move")) { return SkillType.Move; }
        else if (btn.name.Contains("Attack")) { return SkillType.Attack; }
        else if (btn.name.Contains("Gather")) { return SkillType.Gather; }
        return SkillType.Null;
    }

    // ================================== Function 

    // function 위치 초기화
    private void FunctionTooltipInit() {

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
        AddnewPosition.y = 150;
        tooltipAddRe.anchoredPosition = AddnewPosition;

    }

    private void UpgradeFunc_ItemTextInit() {
        itemimgs.gameObject.SetActive(true);
        foreach (Transform item in itemimgs.transform) {
            item.gameObject.SetActive(true);
        }
        itemtexts.gameObject.SetActive(true);
        foreach (Transform item in itemtexts.transform) {
            item.gameObject.SetActive(true);
        }
    }

    // 1. 자기
    private void SleepTooltipShow() {
        tooltipTitle.text = currentsleepDetail.name;
        tooltipMain.text = currentsleepDetail.description;
        tooltipAdditionalText.text = "";
        // 슬라이터 활성화
        sleepTime.gameObject.SetActive(true);
    }
    public void SleepFuncOnClick() {

        // item 개수 비교해야함

        StopCoroutine("SleepSliderValue");
        Text text = sleepTime.GetComponentInChildren<Text>();
        // 코르틴 슬라이더 값 조절
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
    private void SleepSliderInit() {
        sleepTime.value = 1;
        Text text = sleepTime.GetComponentInChildren<Text>();
        text.text = "2:00";
    }
    private void OnDisable() {
        SleepSliderInit();
    }

    // 2. 업그레이드
    private void UpgradeTooltipShow() {
        tooltipTitle.text = currentupgradeDetail.name;
        tooltipMain.text = currentupgradeDetail.description;
        tooltipAdditionalText.text = "필요 구성품"; 
        shelterLevelText.text = string.Format("<size=50>{0}</size>\n<size=30>레벨</size>", shelterManager.ShelterLevel);

        // 아이템 이미지 활성화
        UpgradeFunc_ItemTextInit();
        // 아이템 텍스트 변경
        UpgradeFunc_ItemText();
        // 아이템 텍스트 이미지 위치 변경
        UpgradeFunc_ItemTextPosition();
    }

    private void UpgradeFunc_ItemText() {
        int buildingCheckCount = 0;
        for (int i = 0; i < currentupgradeDetail.needItems.Length; i++) {
            int needItem = currentupgradeDetail.needItems[i].ItemNeedNum;
            int currentItem = tooltipNum.InvenItemGet(currentupgradeDetail.needItems[i].ItemKey);
            if (needItem == 0) {
                itemtexts.transform.GetChild(i).gameObject.SetActive(false);
                itemimgs.transform.GetChild(i).gameObject.SetActive(false);
                buildingCheckCount++;
                continue;
            }
            else {
                Text text = itemtexts.transform.GetChild(i).GetComponent<Text>();
                string textColor = "white";
                textColor = currentItem >= needItem ? "white" : "red";
                if (currentItem >= needItem) {
                    buildingCheckCount++;
                }
                text.text = string.Format("<color={0}>{1} / {2}</color>", textColor, currentItem, needItem);
            }
        }
        // 24 07 16 김수주 건설 설치 bool추가 -> 인벤 아이템 개수 확인
        if (buildingCheckCount == currentupgradeDetail.needItems.Length)
            isUpgradeAvailable = true;
        else isUpgradeAvailable = false;
    }

    private void UpgradeFunc_ItemTextPosition() {
        if (currentupgradeDetail.needItems.All(item => item.ItemNeedNum == 0)) return;

        int needzeroitemcount = 0;
        foreach (NeedItem item in currentupgradeDetail.needItems) {
            if (item.ItemNeedNum != 0) {
                needzeroitemcount++;
            }
        }

        for (int i = 0; i < currentupgradeDetail.needItems.Length; i++) {
            RectTransform eachTextsRe = itemtexts.transform.GetChild(i).GetComponent<RectTransform>();
            RectTransform eachImgsRe = itemimgs.transform.GetChild(i).GetComponent<RectTransform>();
            eachTextsRe.anchoredPosition = new Vector2(eachTextsRe.anchoredPosition.x + 40 * (currentupgradeDetail.needItems.Length - needzeroitemcount), eachTextsRe.anchoredPosition.y);
            eachImgsRe.anchoredPosition = new Vector2(eachImgsRe.anchoredPosition.x + 40 * (currentupgradeDetail.needItems.Length - needzeroitemcount), eachImgsRe.anchoredPosition.y);
        }
    }

    // 3. 짐 싸기
    private void PackingTooltipShow() {
        tooltipTitle.text = currentpackingDetail.name;
        tooltipMain.text = currentpackingDetail.description;
        tooltipAdditionalText.text = "";
    }



}
