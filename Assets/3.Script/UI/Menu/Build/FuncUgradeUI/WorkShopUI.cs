using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkShopUI : UIInfo {
    /*
     * 작업장 아이템 스크롤은 자동
     1. 작업장 - 작업장 레벨 비교 : TooltipNum
      2. scrollview -> content 아이템들 확인
        - 레벨따라서 잠금 표시 & 버튼 막아야함
     - 아이템 레벨은 버튼이름 마지막
     */

    [Space((int)2)]
    [Header("Main Button Disapear")]
    private WorkshopManager workshopManager;
    private TooltipNum tooltipNum;

    public UpgradeDetail UpgradeDetail { get { return currentupgradeDetail; } }
    public PackingDetail PackingDetail { get { return currentpackingDetail; } }
    private UpgradeDetail currentupgradeDetail;
    private PackingDetail currentpackingDetail;

    static public bool isWorkshopUIOpen { get { return _isWorkshopUIOpen; } }
    static private bool _isWorkshopUIOpen = false;
    private void Awake() {
        tooltipNum = FindObjectOfType<TooltipNum>();
        workshopManager = FindObjectOfType<WorkshopManager>();
    }

    protected override void Update() {
        base.Update();
        if (workshopManager.WorkshopLevel +1 != currentupgradeDetail.upgradeLevel) WorkshopInit();
    }

    protected override void OnEnable() {
        base.OnEnable();
        _isWorkshopUIOpen = true;
        WorkshopInit();
    }
    protected override void OnDisable() {
        _isWorkshopUIOpen = false;
        base.OnDisable();
    }


    // 시작할 때 레벨 확인 
    private void WorkshopInit() {
        Debug.Log(workshopManager.WorkshopLevel);
        currentupgradeDetail = tooltipNum.UpgradeItemCheck(UpgradeType.Workshop, workshopManager.WorkshopLevel + 1);
        currentpackingDetail = tooltipNum.PackingItemCheck();
    }
}
