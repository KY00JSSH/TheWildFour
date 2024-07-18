using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkShopUI : MonoBehaviour {
    /*
     * 작업장 아이템 스크롤은 자동
     1. 작업장 - 작업장 레벨 비교 : TooltipNum
      2. scrollview -> content 아이템들 확인
        - 레벨따라서 잠금 표시 & 버튼 막아야함
     - 아이템 레벨은 버튼이름 마지막
     */
    private WorkshopManager workshopManager;
    private TooltipNum tooltipNum;

    [SerializeField] private Text workshopLevel;       // shelter 레벨 표시
    [Space((int)2)]
    [Header("Main Button Disapear")]
    [SerializeField] private GameObject menuButton;
    public GameObject content;

    public UpgradeDetail CurrentupgradeDetail { get; private set; }

    private void Awake() {
        workshopManager = FindObjectOfType<WorkshopManager>();
        workshopLevel = transform.Find("Text_Ws_Level").GetComponent<Text>();
        tooltipNum = FindObjectOfType<TooltipNum>();
    }
    private void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape)) Escape();
    }

    private void OnEnable() {
        WorkshopInit();
        FindButtonLevel();
        menuButton.SetActive(false);
    }
    private void OnDisable() {
        menuButton.SetActive(true);
    }

    public void Escape() {
        menuButton.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    // 시작할 때 레벨 확인 
    private void WorkshopInit() {
        Debug.Log(workshopManager.WorkshopLevel);
        CurrentupgradeDetail = tooltipNum.UpgradeItemCheck(UpgradeType.Workshop, workshopManager.WorkshopLevel);
    }

    // 받은 정보로 content 안의 버튼 돌릴 것 
    private void FindButtonLevel() {
        Item nowbtnkey = new Item(); 
        foreach (Transform child in content.transform) {
            Button childbutton = child.GetComponent<Button>();
            nowbtnkey = tooltipNum.FindButtonItemKey(childbutton);
            if (nowbtnkey.itemData is WeaponItemData weap ) {
                LockButtonWithLevel(childbutton, weap.Level) ;
            }
            else if (nowbtnkey.itemData is MedicItemData medi) {
                //TODO: 의약품 현재 레벨 없음 추가해야함!!!
                LockButtonWithLevel(childbutton , 99);
            }            
        }
    }

    private void LockButtonWithLevel(Button childbutton, int level) {
        if (level <= workshopManager.WorkshopLevel) {
            childbutton.enabled = true;
        }
        else {
            childbutton.enabled = false;
        }
    }

}
