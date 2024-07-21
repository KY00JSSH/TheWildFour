
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Tooltip_Workshop : TooltipInfo, IPointerEnterHandler {
    /*
     1.CurrentupgradeDetail은 WorkshopManager에서 확인
     2. 아이템
     */
    private InvenController invenController;
    private WorkShopUI workShopUI;
    private WorkshopManager workshopManager;
    private TooltipNum tooltipNum;
    private UpgradeDetail currentupgradeDetail;
    private PackingDetail currentpackingDetail;
    private PlayerStatus playerStatus;
    private Button Tooltip_L_btn;
    public bool isWSUpgradeAvailable { get; private set; }
    public bool isWSSkillAvailable { get; private set; }

    public int ItemKeyInWS { get { return _itemkeyinWs; } }
    public int _itemkeyinWs = 0;


    protected override void Awake() {
        base.Awake();
        workShopUI = GetComponent<WorkShopUI>();
        tooltipNum = FindObjectOfType<TooltipNum>();
        workshopManager = FindObjectOfType<WorkshopManager>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        invenController = FindObjectOfType<InvenController>();
    }


    protected override void OnEnable() {
        base.OnEnable();
        WorkshopInit();
    }
    private void Update() {
        if (Tooltip_S.activeSelf) {
            UpgradeFunc_ItemText();
        }
        else if (Tooltip_L.activeSelf) {
            // TryGetValue를 사용하여 키가 존재하는지 확인
            if (workShopUI.BtnItem.TryGetValue(Tooltip_L_btn, out Item btnItem)) {
                WorkshopItemShow(Tooltip_L_btn, btnItem);
            }
        }
    }
    // 시작할 때 레벨 확인 
    private void WorkshopInit() {
        Debug.Log(workshopManager.WorkshopLevel);
        currentupgradeDetail = tooltipNum.UpgradeItemCheck(UpgradeType.Workshop, workshopManager.WorkshopLevel + 1);
        currentpackingDetail = tooltipNum.PackingItemCheck();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null && !btn.name.Contains("Exit")) {
                // 버튼 -> 위치 전체 초기화
                LoadTextPositions_Func(S_ItemTexts, S_ItemImgs, S_itemNeedPosSave);
                LoadTextPositions_Func(L_ItemTexts, L_ItemImgs, L_itemNeedPosSave);
                LoadTextPositions_Func(L_StatsTexts, L_StatsImgs, StatsPosSave);

                if (eventData.position.y >= 520) {
                    Tooltip_S.SetActive(false);
                    Tooltip_L.SetActive(true);
                    // 버튼 DICTIONARY에 아이템 값이 있다면
                    if (workShopUI.BtnItem.TryGetValue(btn, out Item btnItem)) {
                        Tooltip_L_btn = btn;
                        _itemkeyinWs = btnItem.Key;
                        WorkshopItemShow(btn, btnItem);
                    }
                }
                else {
                    Tooltip_S.SetActive(true);
                    Tooltip_L.SetActive(false);
                    TextImgActiveInit(L_StatsTexts, L_StatsImgs);
                    if (eventData.position.x >= 210) { // 창고 위치 막음
                        // y 위치에 따라 CurrentupgradeDetail or PackingDetal
                        if (400 <= eventData.position.y && eventData.position.y < 520) {
                            WorkshopUpgradeShow();
                        }
                        else {
                            PackingTooltipShow();
                        }
                    }

                }
            }
        }
    }


    private void WorkshopItemShow(Button btn, Item btnItem) {
        TextImgActiveInit(L_StatsTexts, L_StatsImgs);
        TextImgActiveInit(L_ItemTexts, L_ItemImgs);
        L_TextTitle.text = btnItem.itemData.name;
        L_TextMain.text = btnItem.itemData.Description;
        L_ItemImg.sprite = btnItem.itemData.Icon;
        // 1. 스탯 
        WorkshopStatsLevelText(btnItem);
        //2. 필요한 아이템
        WorkshopNeedItemCheck(btn, btnItem);
    }


    private void WorkshopNeedItemCheck(Button btn, Item btnItem) {
        if (btnItem.itemData is WeaponItemData weap) {
            WorkshopNeedItemSprite(btn, weap.MaterialKey, weap.MaterialCount);
            if (isWSSkillAvailable) {
                if (!invenController.checkCanCreateItem(btnItem.Key)) {

                    L_TextResult.text = "<color=Red>인벤토리 자리 부족</color>";
                    isWSSkillAvailable = false;
                }
            }
            WorkshopNeedItemDisappear(weap.MaterialKey.Length);
        }
        else if (btnItem.itemData is MedicItemData medic) {
            WorkshopNeedItemSprite(btn, medic.MaterialKey, medic.MaterialCount);
            WorkshopNeedItemDisappear(medic.MaterialKey.Length);
        }
        else {
            Debug.Log(btnItem.name);
        }
    }

    private void WorkshopNeedItemSprite(Button btn, int[] matkeys, int[] matcnts) {

        int Count = 0;
        for (int i = 0; i < matkeys.Length; i++) {
            int needItemkey = matkeys[i];
            int needItemkeyNum = matcnts[i];
            int currentItem = tooltipNum.InvenItemGet(needItemkey);
            Text text = L_ItemTexts.transform.GetChild(i).GetComponent<Text>();
            Image image = L_ItemImgs.transform.GetChild(i).GetComponent<Image>();
            string textColor = currentItem >= needItemkeyNum ? "white" : "red";
            if (currentItem >= needItemkeyNum) {
                Count++;
            }
            text.text = string.Format("<color={0}>{1} / {2}</color>", textColor, currentItem, needItemkeyNum);

            List<Item> _items = FindObjectOfType<WorkshopItemSpawner>().Materialitems;
            for (int j = 0; j < _items.Count; j++) {
                if (needItemkey == _items[j].itemData.Key) {
                    image.sprite = _items[j].itemData.Icon;
                }
            }
        }
        // 인벤 개수 확인 후 버튼 클릭가능 불가능 여부 확인
        if (Count == matkeys.Length) {
            if (L_TextResult.text == "") L_TextResult.text = "<color=White>제작 가능</color>";
            isWSSkillAvailable = true;
        }
        else {
            if (L_TextResult.text == "") L_TextResult.text = "<color=Red>자원 부족</color>";
            isWSSkillAvailable = false;
        }
    }

    private void WorkshopStatsLevelText(Item btnItem) {
        int btnItemLevel = 0;
        if (btnItem.itemData is WeaponItemData weap) {
            btnItemLevel = weap.Level;
            WorkshopItemData(weap);
        }
        else if (btnItem.itemData is MedicItemData medi) {
            btnItemLevel = medi.Level;
            WorkshopItemData(medi);
        }

        if (btnItemLevel <= workshopManager.WorkshopLevel) {
            L_TextResult.text = null;
        }
        else {
            L_TextResult.text = string.Format("<color=Red>필요 작업장 레벨:{0}</color>", btnItemLevel);
        }
    }

    private void WorkshopItemData(WeaponItemData weap) {
        // 공격력
        L_StatsImgs.transform.GetChild(0).GetComponent<Image>().sprite = L_StatsSprites[1];

        L_StatsImgs.transform.GetChild(1).gameObject.SetActive(false);
        //내구도
        L_StatsImgs.transform.GetChild(2).GetComponent<Image>().sprite = L_StatsSprites[5];

        // 공격력
        L_StatsTexts.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}-{1}", weap.MinPowerPoint, weap.MaxPowerPoint);

        L_StatsTexts.transform.GetChild(1).gameObject.SetActive(false);
        //내구도
        L_StatsTexts.transform.GetChild(2).GetComponent<Text>().text = weap.TotalDurability.ToString();
    }

    private void WorkshopItemData(MedicItemData medi) {

        L_StatsImgs.transform.GetChild(0).gameObject.SetActive(false);
        // 힐
        L_StatsImgs.transform.GetChild(1).GetComponent<Image>().sprite = L_StatsSprites[3];

        L_StatsImgs.transform.GetChild(2).gameObject.SetActive(false);

        int healAmount = (int)(medi.HealTime * playerStatus.GetHealTick());

        L_StatsTexts.transform.GetChild(0).gameObject.SetActive(false);
        // 힐
        L_StatsTexts.transform.GetChild(1).GetComponent<Text>().text = healAmount.ToString();

        L_StatsTexts.transform.GetChild(2).gameObject.SetActive(false);

    }
    //============== Func

    // 2. 업그레이드
    private void WorkshopUpgradeShow() {

        S_ItemImgs.SetActive(true);
        S_ItemTexts.SetActive(true);

        S_TextTitle.text = currentupgradeDetail.name;
        S_TextMain.text = currentupgradeDetail.description;
        S_TextAdd.text = "필요 구성품";
        LevelText.text = string.Format("<size=50>{0}</size>\n<size=30>레벨</size>", workshopManager.WorkshopLevel);

        // 아이템 이미지 활성화
        UpgradeFunc_ItemTextInit();
        // 아이템 텍스트 변경
        UpgradeFunc_ItemText();
        // 아이템 텍스트 이미지 위치 변경
        //UpgradeFunc_ItemTextPosition();
    }
    private void UpgradeFunc_ItemTextInit() {
        S_ItemImgs.gameObject.SetActive(true);
        foreach (Transform item in S_ItemImgs.transform) {
            item.gameObject.SetActive(true);
        }
        S_ItemTexts.gameObject.SetActive(true);
        foreach (Transform item in S_ItemTexts.transform) {
            item.gameObject.SetActive(true);
        }
    }
    private void UpgradeFunc_ItemText() {
        int buildingCheckCount = 0;
        for (int i = 0; i < currentupgradeDetail.needItems.Length; i++) {
            int needItem = currentupgradeDetail.needItems[i].ItemNeedNum;
            int currentItem = tooltipNum.InvenItemGet(currentupgradeDetail.needItems[i].ItemKey);

            Text text = S_ItemTexts.transform.GetChild(i).GetComponent<Text>();
            Image img = S_ItemImgs.transform.GetChild(i).GetComponent<Image>();

            if (needItem == 0) {
                text.gameObject.SetActive(false);
                img.gameObject.SetActive(false);
                buildingCheckCount++;
                continue;
            }
            else {
                string textColor = currentItem >= needItem ? "white" : "red";
                if (currentItem >= needItem) {
                    buildingCheckCount++;
                }
                text.text = string.Format("<color={0}>{1} / {2}</color>", textColor, currentItem, needItem);


                List<Item> _items = FindObjectOfType<WorkshopItemSpawner>().Materialitems;
                for (int j = 0; j < _items.Count; j++) {
                    if (currentupgradeDetail.needItems[i].ItemKey == _items[j].itemData.Key) {
                        img.sprite = _items[j].itemData.Icon;
                    }
                }
            }
        }
        // 24 07 16 김수주 건설 설치 bool추가 -> 인벤 아이템 개수 확인
        if (buildingCheckCount == currentupgradeDetail.needItems.Length) {
            isWSUpgradeAvailable = true;
            S_TextResult.text = "<color=White>업그레이드 가능</color>";
        }
        else {
            isWSUpgradeAvailable = false;
            S_TextResult.text = "<color=Red>자원 부족</color>";
        }
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
            RectTransform eachTextsRe = S_ItemTexts.transform.GetChild(i).GetComponent<RectTransform>();
            RectTransform eachImgsRe = S_ItemImgs.transform.GetChild(i).GetComponent<RectTransform>();
            eachTextsRe.anchoredPosition = new Vector2(eachTextsRe.anchoredPosition.x + 40 * (3 - needzeroitemcount), eachTextsRe.anchoredPosition.y);
            eachImgsRe.anchoredPosition = new Vector2(eachImgsRe.anchoredPosition.x + 40 * (3 - needzeroitemcount), eachImgsRe.anchoredPosition.y);
        }
    }


    // 3. 짐 싸기
    private void PackingTooltipShow() {
        S_TextTitle.text = currentpackingDetail.name;
        S_TextMain.text = currentpackingDetail.description;
        S_TextAdd.text = "";
        S_TextResult.text = "";
        S_ItemImgs.SetActive(false);
        S_ItemTexts.SetActive(false);
    }
}
