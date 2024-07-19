using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Tooltip_Workshop : TooltipInfo, IPointerEnterHandler, IPointerExitHandler {
    /*
     1.CurrentupgradeDetail은 WorkshopManager에서 확인
     2. 아이템
     */

    private WorkShopUI workShopUI;
    private WorkshopManager workshopManager;
    private TooltipNum tooltipNum;
    private UpgradeDetail currentupgradeDetail;
    private PackingDetail currentpackingDetail;

    public bool isWSUpgradeAvailable = false;


    protected override void Awake() {
        base.Awake();
        workShopUI = GetComponent<WorkShopUI>();
        tooltipNum = FindObjectOfType<TooltipNum>();
        workshopManager = FindObjectOfType<WorkshopManager>();
    }


    protected override void OnEnable() {
        base.OnEnable();
        WorkshopInit();
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
                        WorkshopItemShow(btnItem);
                    }
                }
                else {
                    Tooltip_S.SetActive(true);
                    Tooltip_L.SetActive(false);
                    Debug.Log(eventData.position.x);
                    if(eventData.position.x >= 210) { // 창고 위치 막음
                        // y 위치에 따라 CurrentupgradeDetail or PackingDetal
                        if (365 <= eventData.position.y && eventData.position.y < 440) {
                            PackingTooltipShow();
                        }
                        else {
                            WorkshopUpgradeShow();
                        }
                    }

                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn == null) {
                Tooltip_S.SetActive(false);
                Tooltip_L.SetActive(false);
            }
        }
    }

    private void WorkshopItemShow(Item btnItem) {
        L_TextTitle.text = btnItem.itemData.name;
        L_TextMain.text = btnItem.itemData.Description;
        WorkshopLevelText(btnItem);
        WorkshopItemImgShow(btnItem);
    }

    private void WorkshopLevelText(Item btnItem) {
        int btnItemLevel = 0;
        if (btnItem.itemData is WeaponItemData weap) {
            btnItemLevel = weap.Level;
        }
        else if (btnItem.itemData is MedicItemData medi) {
            //TODO: 의약품 현재 레벨 없음 추가해야함!!!
            btnItemLevel = 99;
        }

        string color = "White";
        color = btnItemLevel <= workshopManager.WorkshopLevel ? "White" : "Red";

        L_TextResult.text = string.Format("<color={1}>필요 작업장 레벨:{0}</color>", btnItemLevel, color);
    }

    private void WorkshopItemImgShow(Item btnItem) {
        //TODO: 아이템별로 각 Stat 정보를 들고와야함 -> 1~3 위치도 변경되어야함


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
        UpgradeFunc_ItemTextPosition();
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
            if (needItem == 0) {
                S_ItemTexts.transform.GetChild(i).gameObject.SetActive(false);
                S_ItemImgs.transform.GetChild(i).gameObject.SetActive(false);
                buildingCheckCount++;
                continue;
            }
            else {
                Text text = S_ItemTexts.transform.GetChild(i).GetComponent<Text>();
                string textColor = "white";
                textColor = currentItem >= needItem ? "white" : "red";
                if (currentItem >= needItem) {
                    buildingCheckCount++;
                }
                text.text = string.Format("<color={0}>{1} / {2}</color>", textColor, currentItem, needItem);
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
