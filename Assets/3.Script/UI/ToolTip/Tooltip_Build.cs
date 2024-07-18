using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tooltip_Build : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private Menu_Controll menuControll;
    private BuildDetail currentBuildDetail;
    private TooltipNum tooltipNum;

    [SerializeField] private Button[] buttons;
    public GameObject tooltipbox;
    private Text tooltipTitle;   // ������ �̸� �ؽ�Ʈ
    private Text tooltipMain; // ������ ���� �ؽ�Ʈ


    [SerializeField] private GameObject itemimgs;
    [SerializeField] private GameObject itemtexts;

    public bool isBuildAvailable = false;
    int btnIndex = 0;
    private int[] btnNum = new int[5];

    private void Awake() {
        if (buttons == null) buttons = transform.GetComponentsInChildren<Button>();
        menuControll = FindObjectOfType<Menu_Controll>();
        tooltipNum = FindObjectOfType<TooltipNum>();
        if (tooltipTitle == null) tooltipTitle = tooltipbox.transform.GetChild(1).GetComponent<Text>();
        if (tooltipMain == null) tooltipMain = tooltipbox.transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null && IsButtonInArray(btn)) {
                menuControll.ButtonMove(400, false);
                tooltipbox.gameObject.SetActive(true);
                //buttonNum = FindDictionaryKey(btn);
                btnIndex = FindDictionaryKey(btn);
                int buttonValue = btnNum[btnIndex];
                currentBuildDetail = tooltipNum.BuildItemCheck(btnIndex, buttonValue);
                if (currentBuildDetail != null) {
                    // Text ǥ��
                    BuildTooltipShow();
                    Build_ItemTextInit();
                    Build_ItemText();
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn != null) {
                btnIndex = 0;
                //currentBuildDetail = null;
            }
        }
    }

    private bool IsButtonInArray(Button btn) {
        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i] == btn) {
                return true;
            }
        }
        return false;
    }

    private void BuildTooltipShow() {
        tooltipTitle.text = currentBuildDetail.name;
        tooltipMain.text = currentBuildDetail.description;
    }


    private int FindDictionaryKey(Button btn) {
        int buttonNum = 0;
        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i].name == btn.name) {
                buttonNum = i;
                break;
            }
        }
        return buttonNum;
    }

    // tooltip �̹��� Ȱ��ȭ
    private void Build_ItemTextInit() {
        itemimgs.gameObject.SetActive(true);
        foreach (Transform item in itemimgs.transform) {
            item.gameObject.SetActive(true);
        }
        itemtexts.gameObject.SetActive(true);
        foreach (Transform item in itemtexts.transform) {
            item.gameObject.SetActive(true);
        }
    }

    // ������ ������
    private void Build_ItemText() {
        int buildingCheckCount = 0;

        for (int i = 0; i < currentBuildDetail.needItems.Length; i++) {
            int needItem = currentBuildDetail.needItems[i].ItemNeedNum;
            int currentItem = tooltipNum.InvenItemGet(currentBuildDetail.needItems[i].ItemKey);
            if (needItem == 0) {
                isBuildAvailable = true;
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

        // 24 07 16 ����� �Ǽ� ��ġ bool�߰� -> �κ� ������ ���� Ȯ��
        if (buildingCheckCount == currentBuildDetail.needItems.Length)
            isBuildAvailable = true;
        else isBuildAvailable = false;
    }

    public void OnWorkshopButton() {
        btnNum[3] = 1;
    }

}
