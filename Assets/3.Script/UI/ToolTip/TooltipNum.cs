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
     1. 버튼 정보 (Dictionary num) 
     2. 인벤토리 필요한 아이템 키 확인 => 아이템 합산
        - 아이템 키별로 int값 저장할 수 있게 
     2. 아이템 개수 비교 -> 알아서 잘 해     
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
                        //TODO: 해당 파일을 return 해야하나?
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
                    // 쉘터레벨이랑 같으면 정보 들고와야함 => +1 인지는 나중에 확인해
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

    // 저장했던 Json에서 NeedItem을 받아와서 아이템들의 Key와 비교
    // 비교해서 맞으면 해당 키의 리스트 저장 없으면 0개
    public int InvenItemGet(int itemKey) {
        int cntitemnum = 0;
        foreach (Item each in invenController.Inventory) {
            if (each.itemData.Key == itemKey) {
                // 키가 같음 갯수 세야하는데
                if (each is CountableItem _item)
                    cntitemnum += _item.CurrStackCount;
            }
        }
        return cntitemnum;
    }


}
