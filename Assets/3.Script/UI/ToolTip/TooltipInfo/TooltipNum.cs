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

    // 저장했던 Json에서 NeedItem을 받아와서 아이템들의 Key와 비교
    // 비교해서 맞으면 해당 키의 리스트 저장 없으면 0개
    /*
    public int InvenItemGet(int itemKey) {
        int cntitemnum = 0;
        foreach (Item each in invenController.Inventory) {
            if (each == null) return 0;

            if (each.itemData.Key == itemKey) {
                // 키가 같음 갯수 세야하는데
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
            if (invenController.Inventory[i] == null) continue;
            if (invenController.Inventory[i].GetComponent<Item>().Key == itemKey) {
                // 키가 같음 갯수 세야하는데z`
                if (invenController.Inventory[i].GetComponent<CountableItem>() != null)
                    cntitemnum += invenController.Inventory[i].GetComponent<CountableItem>().CurrStackCount;
                else {
                    cntitemnum++;
                }
            }
        }
        return cntitemnum;
    }


    // workshop -> 합성 item key 찾기
    public Item FindButtonItemKey(Button btn) {
        int itemKeyNum;
        
        if (int.TryParse(btn.name, out itemKeyNum)) {
            // 아이템의 합성 재료를 찾기
            foreach(Item finditem in workshopitemSpawner.Workshopitems) {
                if(finditem.Key == itemKeyNum) {
                    return finditem;
                }
            }
        }
        return null;
    }

}
