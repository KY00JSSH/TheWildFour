using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkShopUI : MonoBehaviour {
    /*
     * �۾��� ������ ��ũ���� �ڵ�
     1. �۾��� - �۾��� ���� �� : TooltipNum
      2. scrollview -> content �����۵� Ȯ��
        - �������� ��� ǥ�� & ��ư ���ƾ���
     - ������ ������ ��ư�̸� ������
     */
    private WorkshopManager workshopManager;
    private TooltipNum tooltipNum;

    [SerializeField] private Text workshopLevel;       // shelter ���� ǥ��
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

    // ������ �� ���� Ȯ�� 
    private void WorkshopInit() {
        Debug.Log(workshopManager.WorkshopLevel);
        CurrentupgradeDetail = tooltipNum.UpgradeItemCheck(UpgradeType.Workshop, workshopManager.WorkshopLevel);
    }

    // ���� ������ content ���� ��ư ���� �� 
    private void FindButtonLevel() {
        Item nowbtnkey = new Item(); 
        foreach (Transform child in content.transform) {
            Button childbutton = child.GetComponent<Button>();
            nowbtnkey = tooltipNum.FindButtonItemKey(childbutton);
            if (nowbtnkey.itemData is WeaponItemData weap ) {
                LockButtonWithLevel(childbutton, weap.Level) ;
            }
            else if (nowbtnkey.itemData is MedicItemData medi) {
                //TODO: �Ǿ�ǰ ���� ���� ���� �߰��ؾ���!!!
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
