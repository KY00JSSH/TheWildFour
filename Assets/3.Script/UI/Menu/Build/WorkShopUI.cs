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

    [Space((int)2)]
    [Header("Main Button Disapear")]
    [SerializeField] private GameObject menuButton;
    public GameObject content;

    public Dictionary<Button, Item> BtnItem { get; private set; }

    private void Awake() {
        workshopManager = FindObjectOfType<WorkshopManager>();
        tooltipNum = FindObjectOfType<TooltipNum>();
        BtnItem = new Dictionary<Button, Item>();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) Escape();
    }

    private void OnEnable() {
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



    // 받은 정보로 content 안의 버튼 돌릴 것 
    private void FindButtonLevel() {
        foreach (Transform child in content.transform) {
            Button childbutton = child.GetComponent<Button>();

            // 버튼이 비활성화되어 있다면 건너뜀
            if (!child.gameObject.activeSelf) {
                Debug.Log("비활성화 이름 확인" + child.name);
                continue;
            }

            // TooltipNum.FindButtonItemKey에서 null 체크 추가
            Item nowbtnkey = tooltipNum.FindButtonItemKey(childbutton);
            if (nowbtnkey == null || nowbtnkey.itemData == null) {
                Debug.LogWarning("Item or ItemData is null");
                continue;
            }
            else {
                if (nowbtnkey.itemData is WeaponItemData weap) {
                    LockButtonWithLevel(childbutton, weap.Level);
                }
                else if (nowbtnkey.itemData is MedicItemData medi) {
                    LockButtonWithLevel(childbutton, medi.Level);
                }
                BtnItem.Add(childbutton, nowbtnkey);
            }           
        }
    }

    private void LockButtonWithLevel(Button childbutton, int level) {
        if (level <= workshopManager.WorkshopLevel) {
            childbutton.transform.GetChild(2).gameObject.SetActive(false);
            childbutton.enabled = true;
        }
        else {
            childbutton.transform.GetChild(2).gameObject.SetActive(true);
            childbutton.enabled = false;
        }
    }

}
