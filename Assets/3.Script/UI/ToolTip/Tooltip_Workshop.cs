using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Tooltip_Workshop : TooltipInfo, IPointerEnterHandler, IPointerExitHandler {
    /*
     1.CurrentupgradeDetail�� WorkshopManager���� Ȯ��
     2. ������
     */

    private WorkShopUI workShopUI;
    private WorkshopManager workshopManager;
    private TooltipNum tooltipNum;
    private UpgradeDetail currentupgradeDetail;
    private PackingDetail currentpackingDetail;
    private PlayerStatus playerStatus;

    public bool isWSUpgradeAvailable = false;


    protected override void Awake() {
        base.Awake();
        workShopUI = GetComponent<WorkShopUI>();
        tooltipNum = FindObjectOfType<TooltipNum>();
        workshopManager = FindObjectOfType<WorkshopManager>();
        playerStatus = FindObjectOfType<PlayerStatus>();
    }


    protected override void OnEnable() {
        base.OnEnable();
        WorkshopInit();
    }

    // ������ �� ���� Ȯ�� 
    private void WorkshopInit() {
        Debug.Log(workshopManager.WorkshopLevel);
        currentupgradeDetail = tooltipNum.UpgradeItemCheck(UpgradeType.Workshop, workshopManager.WorkshopLevel + 1);
        currentpackingDetail = tooltipNum.PackingItemCheck();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null && !btn.name.Contains("Exit")) {
                // ��ư -> ��ġ ��ü �ʱ�ȭ


                LoadTextPositions_Func(S_ItemTexts, S_ItemImgs, S_itemNeedPosSave);
                LoadTextPositions_Func(L_ItemTexts, L_ItemImgs, L_itemNeedPosSave);
                LoadTextPositions_Func(L_StatsTexts, L_StatsImgs, StatsPosSave);

                if (eventData.position.y >= 520) {
                    Tooltip_S.SetActive(false);
                    Tooltip_L.SetActive(true);
                    // ��ư DICTIONARY�� ������ ���� �ִٸ�
                    if (workShopUI.BtnItem.TryGetValue(btn, out Item btnItem)) {
                        WorkshopItemShow(btnItem);
                    }
                }
                else {
                    Tooltip_S.SetActive(true);
                    Tooltip_L.SetActive(false);
                    L_StatsActiveInit();
                    if (eventData.position.x >= 210) { // â�� ��ġ ����
                        // y ��ġ�� ���� CurrentupgradeDetail or PackingDetal
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
        L_StatsActiveInit();
        L_TextTitle.text = btnItem.itemData.name;
        L_TextMain.text = btnItem.itemData.Description;
        L_ItemImg.sprite = btnItem.itemData.Icon;
        // 1. ���� 2. �ʿ��� ������
        WorkshopLevelText(btnItem);
        //TODO: 2. �ʿ��� ������ �߰� �ؾ���!! + �������� ����� ��������
    }


    private void WorkshopLevelText(Item btnItem) {
        int btnItemLevel = 0;
        if (btnItem.itemData is WeaponItemData weap) {
            btnItemLevel = weap.Level;
            WorkshopItemData(weap);
        }
        else if (btnItem.itemData is MedicItemData medi) {
            Debug.Log("???????????????????????");
            btnItemLevel = medi.Level;
            WorkshopItemData(medi);
        }

        string color = btnItemLevel <= workshopManager.WorkshopLevel ? "White" : "Red";

        L_TextResult.text = string.Format("<color={1}>�ʿ� �۾��� ����:{0}</color>", btnItemLevel, color);
    }

    private void WorkshopItemData(WeaponItemData weap) {
        //TODO: �����ۺ��� �� Stat ������ ���;��� -> 1~3 ��ġ�� ����Ǿ���� => ä���� ���� 2���� ����
        // ���ݷ�
        L_StatsImgs.transform.GetChild(0).GetComponent<Image>().sprite = L_StatsSprites[1];

        L_StatsImgs.transform.GetChild(1).gameObject.SetActive(false);
        //������
        L_StatsImgs.transform.GetChild(2).GetComponent<Image>().sprite = L_StatsSprites[5];

        // ���ݷ�
        L_StatsTexts.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}-{1}", weap.MinPowerPoint, weap.MaxPowerPoint);

        L_StatsTexts.transform.GetChild(1).gameObject.SetActive(false);
        //������
        L_StatsTexts.transform.GetChild(2).GetComponent<Text>().text = weap.TotalDurability.ToString();
    }

    private void WorkshopItemData(MedicItemData medi) {
        //TODO: �����ۺ��� �� Stat ������ ���;��� -> 1~3 ��ġ�� ����Ǿ����

        L_StatsImgs.transform.GetChild(0).gameObject.SetActive(false);
        // ��
        L_StatsImgs.transform.GetChild(1).GetComponent<Image>().sprite = L_StatsSprites[3];

        L_StatsImgs.transform.GetChild(2).gameObject.SetActive(false);

        int healAmount = (int)(medi.HealTime * playerStatus.GetHealTick());

        L_StatsTexts.transform.GetChild(0).gameObject.SetActive(false);
        // ��
        L_StatsTexts.transform.GetChild(1).GetComponent<Text>().text = healAmount.ToString();

        L_StatsTexts.transform.GetChild(2).gameObject.SetActive(false);

    }
    //============== Func

    // 2. ���׷��̵�
    private void WorkshopUpgradeShow() {

        S_ItemImgs.SetActive(true);
        S_ItemTexts.SetActive(true);

        S_TextTitle.text = currentupgradeDetail.name;
        S_TextMain.text = currentupgradeDetail.description;
        S_TextAdd.text = "�ʿ� ����ǰ";
        LevelText.text = string.Format("<size=50>{0}</size>\n<size=30>����</size>", workshopManager.WorkshopLevel);

        // ������ �̹��� Ȱ��ȭ
        UpgradeFunc_ItemTextInit();
        // ������ �ؽ�Ʈ ����
        UpgradeFunc_ItemText();
        // ������ �ؽ�Ʈ �̹��� ��ġ ����
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
        // 24 07 16 ����� �Ǽ� ��ġ bool�߰� -> �κ� ������ ���� Ȯ��
        if (buildingCheckCount == currentupgradeDetail.needItems.Length) {
            isWSUpgradeAvailable = true;
            S_TextResult.text = "<color=White>���׷��̵� ����</color>";
        }
        else {
            isWSUpgradeAvailable = false;
            S_TextResult.text = "<color=Red>�ڿ� ����</color>";
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


    // 3. �� �α�
    private void PackingTooltipShow() {
        S_TextTitle.text = currentpackingDetail.name;
        S_TextMain.text = currentpackingDetail.description;
        S_TextAdd.text = "";
        S_TextResult.text = "";
        S_ItemImgs.SetActive(false);
        S_ItemTexts.SetActive(false);
    }
}
