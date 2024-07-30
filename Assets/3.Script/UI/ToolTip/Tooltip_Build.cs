using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tooltip_Build : MonoBehaviour, IPointerEnterHandler {

    private Menu_Controll menuControll;
    public BuildDetail currentBuild { get { return currentBuildDetail; } }
    private BuildDetail currentBuildDetail;
    private TooltipNum tooltipNum;

    [SerializeField] private Button[] buttons;
    public GameObject tooltipbox;
    private Text tooltipTitle;   // 아이템 이름 텍스트
    private Text tooltipMain; // 아이템 설명 텍스트
    private WorkshopCreate workshopCreate;

    [SerializeField] private GameObject itemimgs;
    [SerializeField] private GameObject itemtexts;
    Button btn;
    public bool isBuildAvailable = false;
    int btnIndex = 0;
    private int[] btnNum = new int[5];

    // 작업장 1회 설치 후 비용 없음
    private bool isBuiltFirst = false;

    private void Awake() {
        if (buttons == null) buttons = transform.GetComponentsInChildren<Button>();
        menuControll = FindObjectOfType<Menu_Controll>();
        tooltipNum = FindObjectOfType<TooltipNum>();
        if (tooltipTitle == null) tooltipTitle = tooltipbox.transform.GetChild(1).GetComponent<Text>();
        if (tooltipMain == null) tooltipMain = tooltipbox.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        workshopCreate = FindObjectOfType<WorkshopCreate>();
        isBuiltFirst = false;
    }


    private void Update() {
        if (currentBuildDetail != null) {
            // Text 표시
            Build_ItemText();
        }
        if (workshopCreate.isExist) { if (!isBuiltFirst) btnNum[3] = 1; isBuiltFirst = true; }
    }
    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null) {
            btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null && IsButtonInArray(btn)) {
                menuControll.ButtonMove(400, false);
                tooltipbox.gameObject.SetActive(true);
                //buttonNum = FindDictionaryKey(btn);
                btnIndex = FindDictionaryKey(btn);
                int buttonValue = btnNum[btnIndex];
                currentBuildDetail = tooltipNum.BuildItemCheck(btnIndex, buttonValue);
                if (currentBuildDetail != null) {
                    // Text 표시
                    BuildTooltipShow();
                    Build_ItemTextInit();
                    Build_ItemText();
                }
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

    // tooltip 이미지 활성화
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

    // 아이템 개수비교
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

        // 24 07 16 김수주 건설 설치 bool추가 -> 인벤 아이템 개수 확인
        if (buildingCheckCount == currentBuildDetail.needItems.Length)
            isBuildAvailable = true;
        else isBuildAvailable = false;
        //TODO: !!!!!!!!!!!!!!!인벤아이템 개수 false => true 로 임시 변경 바꿔야함
    }


}
