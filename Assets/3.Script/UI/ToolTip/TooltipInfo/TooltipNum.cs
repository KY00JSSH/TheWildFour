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

    private TooltipDetail tooltipDetail;
    private InvenController invenController;
    private WorkshopItemSpawner workshopitemSpawner;
    

    private void Awake() {
        tooltipDetail = GetComponent<TooltipDetail>();
        invenController = FindObjectOfType<InvenController>();
        workshopitemSpawner = FindObjectOfType<WorkshopItemSpawner>();
    }

    // skill
    public SkillDetail ShelterItemCheck(SkillType skillType, GameObject btn) {
        int skillnum = 0;
        char lastChar = btn.name[btn.name.Length - 1];
        foreach (SkillDetail each in tooltipDetail.skillDetailList.skillDetails) {
            if (skillType == each.skillType) {
                if (char.IsDigit(lastChar)) {
                    skillnum = lastChar - '0';
                    if (each.skillNum == skillnum) {
                        return each;
                    }
                }
            }
        }
        return null;
    }

    // upgrade
    public UpgradeDetail UpgradeItemCheck(UpgradeType upgradeType, int Level) {
        foreach (UpgradeDetail each in tooltipDetail.upgradeDetailList.upgradeDetails) {
            if (upgradeType == each.upgradeType) {
                if (each.upgradeLevel == Level) {
                    return each;
                }
            }
        }
        return null;
    }

    // build
    public BuildDetail BuildItemCheck(int btnNum, int buildLevel) {
        foreach (BuildDetail each in tooltipDetail.buildDetailList.buildDetails) {
            if (each.buttonNum == btnNum) {
                if (each.buildLevel == buildLevel)
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
    /*
    public int InvenItemGet(int itemKey) {
        int cntitemnum = 0;
        foreach (Item each in invenController.Inventory) {
            if (each == null) return 0;

            if (each.itemData.Key == itemKey) {
                // Ű�� ���� ���� �����ϴµ�
                if (each is CountableItem _item)
                    cntitemnum += _item.CurrStackCount;
            }
        }
        return cntitemnum;
    }
    */
    public int InvenItemGet(int itemKey) {
        int cntitemnum = 0;
        for (int i = 0; i < invenController.Inventory.Count; i++) {
            if(invenController.Inventory[i]?.itemData?.Key == itemKey) {
                // Ű�� ���� ���� �����ϴµ�z`
                if (invenController.Inventory[i] is CountableItem _item)
                    cntitemnum += _item.CurrStackCount;
                else {
                    cntitemnum++;
                }
            }
        }
        return cntitemnum;
    }


    // workshop -> �ռ� item key ã��
    public Item FindButtonItemKey(Button btn) {
        int itemKeyNum;
        
        if (int.TryParse(btn.name, out itemKeyNum)) {
            // �������� �ռ� ��Ḧ ã��
            foreach(Item finditem in workshopitemSpawner.Workshopitems) {
                if(finditem.Key == itemKeyNum) {
                    return finditem;
                }
            }
        }
        return null;
    }

}
