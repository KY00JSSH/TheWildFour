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
    private Text tooltipTitle;   // 아이템 이름 텍스트
    private Text tooltipMain; // 아이템 설명 텍스트


    [SerializeField] private GameObject itemimgs;
    [SerializeField] private GameObject itemtexts;

    public bool isStartBuildingNumCheck = false;
    private int buttonNum;

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
            if (btn != null) {
                menuControll.ButtonMove(400, false);
                tooltipbox.gameObject.SetActive(true);
                buttonNum = FindDictionaryKey(btn);
                currentBuildDetail = tooltipNum.BuildItemCheck(buttonNum);
                Debug.Log(currentBuildDetail.name);
                if (currentBuildDetail != null) {
                    // Text 표시
                    BuildTooltipShow();
                    UpgradeFunc_ItemTextInit();
                    UpgradeFunc_ItemText();
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.gameObject != null) {
            Button btn = eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();
            if (btn != null) {
                buttonNum = 0;
                //currentBuildDetail = null;
            }
        }
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

    // 아이템 개수비교

    private void UpgradeFunc_ItemText() {
        int buildingCheckCount = 0;
        for (int i = 0; i < currentBuildDetail.needItems.Length; i++) {
            int needItem = currentBuildDetail.needItems[i].ItemNeedNum;
            int currentItem = tooltipNum.InvenItemGet(currentBuildDetail.needItems[i].ItemKey);
            if (needItem == 0) {
                isStartBuildingNumCheck = true;
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
        Debug.Log("아이템 인벤토리 개수 확인" + buildingCheckCount + " / " + itemtexts.transform.childCount);
        // 24 07 16 김수주 건설 설치 bool추가 -> 인벤 아이템 개수 확인
        if (buildingCheckCount == itemtexts.transform.childCount)
            isStartBuildingNumCheck = true;
        else isStartBuildingNumCheck = false;

        Debug.Log("아이템 인벤토리 bool 값 확인" + isStartBuildingNumCheck);
    }


}
