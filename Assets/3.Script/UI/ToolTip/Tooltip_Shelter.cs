
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Tooltip_Shelter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {


    [Header("Tootip ����")]
    [SerializeField] private ShelterManager shelterManager;
    [SerializeField] private GameObject ShelterTooltip;
    private Image tooltipImg;
    private Text tooltipTitle;
    private Text tooltipMain;
    private Text tooltipAdditionalText;

    [Space((int)2)]
    [Header("Tootip Function �߰�")]
    // Function �� �߰�
    [SerializeField] private Slider sleepTime;
    //[SerializeField] private GameObject sleepFunc;
    [SerializeField] private GameObject itemimgs;
    [SerializeField] private GameObject itemtexts;

    private string[] additionalText = { "�ʿ� ����Ʈ 1", "���� �ʿ� ����" };
    private Vector2[] textPositionSave;
    private Vector2[][] itemNeedPositionSave;


    private TooltipNum tooltipNum;
    private SkillDetail currentskillDetail;
    private SleepDetail currentsleepDetail;
    private UpgradeDetail currentupgradeDetail;
    private PackingDetail currentpackingDetail;


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
        // ��ġ ����Ǵ� Text ��ġ ����
        SaveTextPositions();
        SaveTextPositions_Func();
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

    // ����� Text ��ġ �ҷ����� + Function ��ü�� ��Ȱ��ȭ
    private void LoadTextPositions() {
        // ��� ���� �ʱ�ȭ
        RectTransform tooltipBg = ShelterTooltip.transform.GetChild(0).GetComponent<RectTransform>();
        tooltipBg.sizeDelta = new Vector2(tooltipBg.sizeDelta.x, 350);
        tooltipBg.anchoredPosition = new Vector2(tooltipBg.anchoredPosition.x, 0);

        // Text ��ġ ũ�� �ʱ�ȭ
        RectTransform tooltipMainRe = tooltipMain.GetComponent<RectTransform>();
        tooltipMainRe.anchoredPosition = textPositionSave[0];
        tooltipMainRe.sizeDelta = new Vector2(270, tooltipMainRe.sizeDelta.y);

        RectTransform tooltipAdditionalTextRe = tooltipAdditionalText.GetComponent<RectTransform>();
        tooltipAdditionalTextRe.anchoredPosition = new Vector2(textPositionSave[1].x, 60);

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


    // ���� ��ư ���콺 ��ġ Ȯ��
    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) {

                ShelterTooltip.SetActive(true);
                // ��ġ �ʱ�ȭ
                LoadTextPositions();
                LoadTextPositions_Func();

                // ����
                if (eventData.position.y >= 520) {
                    // skill type ������ �޼ҵ�
                    tooltipImg.gameObject.SetActive(true);
                    tooltipImg.sprite = btn.gameObject.transform.GetChild(1).GetComponent<Image>().sprite;
                    currentskillDetail = tooltipNum.ShelterItemCheck(btn.gameObject);
                    SkillTooltipShow();
                }
                else if (440 <= eventData.position.y && eventData.position.y < 520) {
                    FunctionTooltipInit();
                    currentsleepDetail = tooltipNum.SleepItemCheck();
                    SleepTooltipShow();
                }
                else if (365 <= eventData.position.y && eventData.position.y < 440) {
                    FunctionTooltipInit();
                    currentupgradeDetail = tooltipNum.UpgradeItemCheck(UpgradeType.Shelter, shelterManager.ShelterLevel);
                    UpgradeTooltipShow();
                }
                else {
                    FunctionTooltipInit();
                    currentpackingDetail = tooltipNum.PackingItemCheck();
                    PackingTooltipShow();
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

    public void SkillTooltipShow() {

        // �̹��� ����
        Debug.Log(tooltipTitle);
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



    // function ��ġ �ʱ�ȭ
    private void FunctionTooltipInit() {

        tooltipImg.gameObject.SetActive(false);
        sleepTime.gameObject.SetActive(false);
        itemimgs.gameObject.SetActive(false);
        itemtexts.gameObject.SetActive(false);


        // ��� ���� ����
        RectTransform tooltipBg = ShelterTooltip.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 BgnewPosition = tooltipBg.anchoredPosition;
        Vector2 BgSize = tooltipBg.sizeDelta;
        BgSize.y = 450;
        tooltipBg.sizeDelta = BgSize;
        BgnewPosition.y = 0;
        tooltipBg.anchoredPosition = BgnewPosition;

        // ���� Text ��ġ ����
        RectTransform tooltipMainRe = tooltipMain.GetComponent<RectTransform>();
        Vector2 MainnewPosition = tooltipMainRe.anchoredPosition;
        MainnewPosition.x = 0;
        tooltipMainRe.anchoredPosition = MainnewPosition;
        Vector2 MainnewSize = tooltipMainRe.sizeDelta;
        MainnewSize.x = 350;
        tooltipMainRe.sizeDelta = MainnewSize;

        // �߰� ���� Text ��ġ ����
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

    // 1. �ڱ�
    private void SleepTooltipShow() {

        tooltipTitle.text = currentsleepDetail.name;
        tooltipMain.text = currentsleepDetail.description;
        tooltipAdditionalText.text = "";
        // �������� Ȱ��ȭ
        sleepTime.gameObject.SetActive(true);
    }

    // 2. ���׷��̵�
    private void UpgradeTooltipShow() {

        tooltipTitle.text = currentupgradeDetail.name;
        tooltipMain.text = currentupgradeDetail.description;
        tooltipAdditionalText.text = "�ʿ� ����ǰ";
        // �������� Ȱ��ȭ
        sleepTime.gameObject.SetActive(true);

        // ������ �̹��� Ȱ��ȭ
        UpgradeFunc_ItemTextInit();
        // ������ �ؽ�Ʈ ����
        UpgradeFunc_ItemText();
        // ������ �ؽ�Ʈ �̹��� ��ġ ����
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
                buildingCheckCount = currentItem >= needItem ? buildingCheckCount++ : 0;
                text.text = string.Format("<color={0}>{1} / {2}</color>", textColor, currentItem, needItem);
            }
        }
        Debug.Log("������ �κ��丮 ���� Ȯ��" + buildingCheckCount + " / " + itemtexts.transform.childCount);

    }

    private void UpgradeFunc_ItemTextPosition() {
        if (currentupgradeDetail.needItems.All(item => item.ItemNeedNum == 0)) return;

        int needitemcount = 0;
        foreach (NeedItem item in currentupgradeDetail.needItems) {
            if (item.ItemNeedNum != 0) {
                needitemcount++;
            }
        }

        for (int i = 0; i < currentupgradeDetail.needItems.Length; i++) {

            RectTransform eachTextsRe = itemtexts.transform.GetChild(i).GetComponent<RectTransform>();
            RectTransform eachImgsRe = itemimgs.transform.GetChild(i).GetComponent<RectTransform>();
            eachTextsRe.anchoredPosition = new Vector2(eachTextsRe.anchoredPosition.x + 40 * (currentupgradeDetail.needItems.Length - needitemcount), eachTextsRe.anchoredPosition.y);
            eachImgsRe.anchoredPosition = new Vector2(eachImgsRe.anchoredPosition.x + 40 * (currentupgradeDetail.needItems.Length - needitemcount), eachImgsRe.anchoredPosition.y);

        }
    }

    // 3. �� �α�
    private void PackingTooltipShow() {

        tooltipTitle.text = currentpackingDetail.name;
        tooltipMain.text = currentpackingDetail.description;
        tooltipAdditionalText.text = "";
        // �������� Ȱ��ȭ
        sleepTime.gameObject.SetActive(true);
    }



}
