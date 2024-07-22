
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Tooltip_Workshop : TooltipInfo, IPointerEnterHandler {
    /*
     1.CurrentupgradeDetail�� WorkshopManager���� Ȯ��
     2. ������
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
                               
                // ��ư -> ��ġ ��ü �ʱ�ȭ
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
                    if (eventData.position.x >= 210) { // â�� ��ġ ����
                        // y ��ġ�� ���� CurrentupgradeDetail or PackingDetal
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
        // 1. ���� 
        if (btnitemData.isWeap) {
            WorkshopItemData_Weap();

            //2. �ʿ��� ������
            WorkshopNeedItemSprite( btnitemData.weaponItem.MaterialKey, btnitemData.weaponItem.MaterialCount);
            if (isWSSkillAvailable) {
                if (!invenController.createItem(btnitemData.weaponItem)) {

                    L_TextResult.text = "<color=Red>�κ��丮 �ڸ� ����</color>";
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

                    L_TextResult.text = "<color=Red>�κ��丮 �ڸ� ����</color>";
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
        // �κ� ���� Ȯ�� �� ��ư Ŭ������ �Ұ��� ���� Ȯ��
        if (Count == matkeys.Length) {
            if (L_TextResult.text == "") L_TextResult.text = "<color=White>���� ����</color>";
            isWSSkillAvailable = true;
        }
        else {
            if (L_TextResult.text == "") L_TextResult.text = "<color=Red>�ڿ� ����</color>";
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
            L_TextResult.text = string.Format("<color=Red>�ʿ� �۾��� ����:{0}</color>", btnItemLevel);
        }
    }

    private void WorkshopItemData_Weap() {
        // ���ݷ�
        L_StatsImgs.transform.GetChild(0).GetComponent<Image>().sprite = L_StatsSprites[1];

        L_StatsImgs.transform.GetChild(1).gameObject.SetActive(false);
        //������
        L_StatsImgs.transform.GetChild(2).GetComponent<Image>().sprite = L_StatsSprites[5];

        // ���ݷ�
        L_StatsTexts.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}-{1}", btnitemData.weaponItem.MinPowerPoint, btnitemData.weaponItem.MaxPowerPoint);

        L_StatsTexts.transform.GetChild(1).gameObject.SetActive(false);
        //������
        L_StatsTexts.transform.GetChild(2).GetComponent<Text>().text = btnitemData.weaponItem.TotalDurability.ToString();
    }

    private void WorkshopItemData_Medi() {

        L_StatsImgs.transform.GetChild(0).gameObject.SetActive(false);
        // ��
        L_StatsImgs.transform.GetChild(1).GetComponent<Image>().sprite = L_StatsSprites[3];

        L_StatsImgs.transform.GetChild(2).gameObject.SetActive(false);

        int healAmount = (int)(btnitemData.medicItem.HealTime * playerStatus.GetHealTick());

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

        S_TextTitle.text = workShopUI.UpgradeDetail.name;
        S_TextMain.text = workShopUI.UpgradeDetail.description;
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
        // 24 07 16 ����� �Ǽ� ��ġ bool�߰� -> �κ� ������ ���� Ȯ��
        if (buildingCheckCount == workShopUI.UpgradeDetail.needItems.Length) {
            isWSUpgradeAvailable = true;
            S_TextResult.text = "<color=White>���׷��̵� ����</color>";
        }
        else {
            isWSUpgradeAvailable = false;
            S_TextResult.text = "<color=Red>�ڿ� ����</color>";
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


    // 3. �� �α�
    private void PackingTooltipShow() {
        S_TextTitle.text = workShopUI.PackingDetail.name;
        S_TextMain.text = workShopUI.PackingDetail.description;
        S_TextAdd.text = "";
        S_TextResult.text = "";
        S_ItemImgs.SetActive(false);
        S_ItemTexts.SetActive(false);
    }
}
