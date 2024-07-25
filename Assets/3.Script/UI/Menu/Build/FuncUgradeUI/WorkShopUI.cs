using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkShopUI : UIInfo {
    /*
     * �۾��� ������ ��ũ���� �ڵ�
     1. �۾��� - �۾��� ���� �� : TooltipNum
      2. scrollview -> content �����۵� Ȯ��
        - �������� ��� ǥ�� & ��ư ���ƾ���
     - ������ ������ ��ư�̸� ������
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

    public override void Escape() {
        base.Escape();
        WorkshopCreate workshopCreate = FindObjectOfType<WorkshopCreate>();
        workshopCreate.Building.GetComponent<BuildingInteraction>().PlayerExitBuilding<WorkshopCreate>();
    }

    // ������ �� ���� Ȯ�� 
    private void WorkshopInit() {
        Debug.Log(workshopManager.WorkshopLevel);
        currentupgradeDetail = tooltipNum.UpgradeItemCheck(UpgradeType.Workshop, workshopManager.WorkshopLevel + 1);
        currentpackingDetail = tooltipNum.PackingItemCheck();
    }
}
