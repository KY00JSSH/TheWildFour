
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
    private PlayerStatus playerStatus;
    private Button Tooltip_L_btn;
    public bool isWSUpgradeAvailable { get; private set; }
    public bool isWSSkillAvailable { get; private set; }

    public int ItemKeyInWS { get { return _itemkeyinWs; } }
    public int _itemkeyinWs = 0;

    private WorkshopBtn btnitemData;
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
    }
    private void Update() {
        if (Tooltip_S.activeSelf) {
            UpgradeFunc_ItemText();
        }
        else if (Tooltip_L.activeSelf) {
            WorkshopItemShow();
        }
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
                    btnitemData = btn.GetComponent<WorkshopBtn>();
                    if (btnitemData.isWeap) {

                        Debug.Log(btnitemData.weaponItem.name);
                        Debug.Log(btnitemData.weaponItem.Key);
                    }
                    else {
                        Debug.Log(btnitemData.medicItem.name);
                        Debug.Log(btnitemData.medicItem.Key);
                    }
                    Tooltip_S.SetActive(false);
                    Tooltip_L.SetActive(true);
                    WorkshopItemShow();
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


    private void WorkshopItemShow() {
        TextImgActiveInit(L_StatsTexts, L_StatsImgs);
        TextImgActiveInit(L_ItemTexts, L_ItemImgs);
        L_TextTitle.text = btnitemData.iTemData.name;
        L_TextMain.text = btnitemData.iTemData.Description;
        L_ItemImg.sprite = btnitemData.iTemData.Icon;
        // 1. 스탯 
        if (btnitemData.isWeap) {
            WorkshopItemData_Weap();

            //2. 필요한 아이템
            WorkshopNeedItemSprite( btnitemData.weaponItem.MaterialKey, btnitemData.weaponItem.MaterialCount);
            if (isWSSkillAvailable) {
                if (!invenController.createItem(btnitemData.weaponItem)) {

                    L_TextResult.text = "<color=Red>인벤토리 자리 부족</color>";
                    isWSSkillAvailable = false;
                }
            }
            WorkshopNeedItemDisappear(btnitemData.weaponItem.MaterialKey.Length);
        }
        else if (btnitemData.isMedic) {
            WorkshopItemData_Medi();

            WorkshopNeedItemDisappear(btnitemData.medicItem.MaterialKey.Length);
            if (isWSSkillAvailable) {
                if (!invenController.createItem(btnitemData.medicItem)) {

                    L_TextResult.text = "<color=Red>인벤토리 자리 부족</color>";
                    isWSSkillAvailable = false;
                }
            }
        }

        WorkshopLevelText();
    }



    private void WorkshopNeedItemSprite(int[] matkeys, int[] matcnts) {

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

    private void WorkshopLevelText() {
        int btnItemLevel = 0;
        if (btnitemData.isWeap) {
            btnItemLevel = btnitemData.weaponItem.Level;
        }
        else if (btnitemData.isMedic) {
            btnItemLevel = btnitemData.medicItem.Level;
        }

        if (btnItemLevel <= workshopManager.WorkshopLevel) {
            L_TextResult.text = null;
        }
        else {
            L_TextResult.text = string.Format("<color=Red>필요 작업장 레벨:{0}</color>", btnItemLevel);
        }
    }

    private void WorkshopItemData_Weap() {
        // 공격력
        L_StatsImgs.transform.GetChild(0).GetComponent<Image>().sprite = L_StatsSprites[1];

        L_StatsImgs.transform.GetChild(1).gameObject.SetActive(false);
        //내구도
        L_StatsImgs.transform.GetChild(2).GetComponent<Image>().sprite = L_StatsSprites[5];

        // 공격력
        L_StatsTexts.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}-{1}", btnitemData.weaponItem.MinPowerPoint, btnitemData.weaponItem.MaxPowerPoint);

        L_StatsTexts.transform.GetChild(1).gameObject.SetActive(false);
        //내구도
        L_StatsTexts.transform.GetChild(2).GetComponent<Text>().text = btnitemData.weaponItem.TotalDurability.ToString();
    }

    private void WorkshopItemData_Medi() {

        L_StatsImgs.transform.GetChild(0).gameObject.SetActive(false);
        // 힐
        L_StatsImgs.transform.GetChild(1).GetComponent<Image>().sprite = L_StatsSprites[3];

        L_StatsImgs.transform.GetChild(2).gameObject.SetActive(false);

        int healAmount = (int)(btnitemData.medicItem.HealTime * playerStatus.GetHealTick());

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

        S_TextTitle.text = workShopUI.UpgradeDetail.name;
        S_TextMain.text = workShopUI.UpgradeDetail.description;
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
        for (int i = 0; i < workShopUI.UpgradeDetail.needItems.Length; i++) {
            int needItem = workShopUI.UpgradeDetail.needItems[i].ItemNeedNum;
            int currentItem = tooltipNum.InvenItemGet(workShopUI.UpgradeDetail.needItems[i].ItemKey);

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
                    if (workShopUI.UpgradeDetail.needItems[i].ItemKey == _items[j].itemData.Key) {
                        img.sprite = _items[j].itemData.Icon;
                    }
                }
            }
        }
        // 24 07 16 김수주 건설 설치 bool추가 -> 인벤 아이템 개수 확인
        if (buildingCheckCount == workShopUI.UpgradeDetail.needItems.Length) {
            isWSUpgradeAvailable = true;
            S_TextResult.text = "<color=White>업그레이드 가능</color>";
        }
        else {
            isWSUpgradeAvailable = false;
            S_TextResult.text = "<color=Red>자원 부족</color>";
        }
    }
    private void UpgradeFunc_ItemTextPosition() {
        if (workShopUI.UpgradeDetail.needItems.All(item => item.ItemNeedNum == 0)) return;

        int needzeroitemcount = 0;
        foreach (NeedItem item in workShopUI.UpgradeDetail.needItems) {
            if (item.ItemNeedNum != 0) {
                needzeroitemcount++;
            }
        }

        for (int i = 0; i < workShopUI.UpgradeDetail.needItems.Length; i++) {
            RectTransform eachTextsRe = S_ItemTexts.transform.GetChild(i).GetComponent<RectTransform>();
            RectTransform eachImgsRe = S_ItemImgs.transform.GetChild(i).GetComponent<RectTransform>();
            eachTextsRe.anchoredPosition = new Vector2(eachTextsRe.anchoredPosition.x + 40 * (3 - needzeroitemcount), eachTextsRe.anchoredPosition.y);
            eachImgsRe.anchoredPosition = new Vector2(eachImgsRe.anchoredPosition.x + 40 * (3 - needzeroitemcount), eachImgsRe.anchoredPosition.y);
        }
    }


    // 3. 짐 싸기
    private void PackingTooltipShow() {
        S_TextTitle.text = workShopUI.PackingDetail.name;
        S_TextMain.text = workShopUI.PackingDetail.description;
        S_TextAdd.text = "";
        S_TextResult.text = "";
        S_ItemImgs.SetActive(false);
        S_ItemTexts.SetActive(false);
    }
}
