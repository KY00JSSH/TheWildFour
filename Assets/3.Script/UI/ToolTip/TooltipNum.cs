using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InvenCountItem {
    public int _cntItemKey;
    public int _cntItemNum;
}

public class TooltipNum : MonoBehaviour {
    /*
     1. ��ư ���� (Dictionary num) 
     2. �κ��丮 �ʿ��� ������ Ű Ȯ�� => ������ �ջ�
        - ������ Ű���� int�� ������ �� �ְ� 
     2. ������ ���� �� -> �˾Ƽ� �� ��     
     */

    private TooltipDetail tooltipDetail;
    private InvenController invenController;
    private List<InvenCountItem> invenCountItems = new List<InvenCountItem>();
    public List<InvenCountItem> InvenCountItems { get { return invenCountItems; } }

    private void Awake() {
        tooltipDetail = GetComponent<TooltipDetail>();
        invenController = FindObjectOfType<InvenController>();
    }

    // skill
    public SkillDetail ShelterItemCheck(GameObject btn) {
        string btnName = btn.name;
        int skillnum = 0;
        char lastChar = btn.name[btn.name.Length - 1];
        foreach (SkillDetail each in tooltipDetail.skillDetailList.skillDetails) {
            if (btnName.Contains(each.skillType.ToString())) {
                if (char.IsDigit(lastChar)) {
                    skillnum = lastChar - '0';
                    if (each.skillNum == skillnum) {
                        //TODO: �ش� ������ return �ؾ��ϳ�?
                        return each;
                    }
                }

            }
        }
        return null;
    }

    // upgrade
    public UpgradeDetail UpgradeItemCheck(UpgradeType upgradeType, int Level) {
        // invenCountItems.Clear();
        foreach(UpgradeDetail each in tooltipDetail.upgradeDetailList.upgradeDetails) {
            if(upgradeType == each.upgradeType) {
                if (each.upgradeLevel == Level) {
                    // ���ͷ����̶� ������ ���� ���;��� => +1 ������ ���߿� Ȯ����
                    return each;
                }
            }
        }
        return null;
    }


    // build
    public BuildDetail BuildItemCheck(int DictionaryKey) {
        // invenCountItems.Clear();
        foreach (BuildDetail each in tooltipDetail.buildDetailList.buildDetails) {
            if (each.buttonNum == DictionaryKey) {
                //InvenItemGet(each.needItems);
                return each;
            }
        }
        return null;
    }

    public SleepDetail SleepItemCheck() {
        foreach (SleepDetail each in tooltipDetail.sleepDetailList.sleepDetails) {
            return each;
        }
        return null;
    }
    public PackingDetail PackingItemCheck() {
        foreach (PackingDetail each in tooltipDetail.packingDetailList.packingDetails) {
            return each;
        }
        return null;
    }

    // �����ߴ� Json���� NeedItem�� �޾ƿͼ� �����۵��� Key�� ��
    // ���ؼ� ������ �ش� Ű�� ����Ʈ ���� ������ 0��
    public int InvenItemGet(int itemKey) {
        int cntitemnum = 0;
        foreach (Item each in invenController.Inventory) {
            if (each.itemData.Key == itemKey) {
                // Ű�� ���� ���� �����ϴµ�
                if (each is CountableItem _item)
                    cntitemnum += _item.CurrStackCount;
            }
        }
        return cntitemnum;
    }


}
