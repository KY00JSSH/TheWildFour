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
    private void FixedUpdate() {
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



    // ���� ������ content ���� ��ư ���� �� 
    private void FindButtonLevel() {
        foreach (Transform child in content.transform) {
            Button childbutton = child.GetComponent<Button>();

            // ��ư�� ��Ȱ��ȭ�Ǿ� �ִٸ� �ǳʶ�
            if (!child.gameObject.activeSelf) {
                Debug.Log("��Ȱ��ȭ �̸� Ȯ��" + child.name);
                continue;
            }

            // TooltipNum.FindButtonItemKey���� null üũ �߰�
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
        Debug.Log(level +"/"+ workshopManager.WorkshopLevel);
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
