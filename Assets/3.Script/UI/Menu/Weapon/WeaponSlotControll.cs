using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotControll : MonoBehaviour {

    [SerializeField]
    public Image itemIcon;

    private Item currentItem;
    public Item CurrentItem { get { return currentItem; } }

    public void setWeaponSlot(WeaponItemData item = null) {
        if (item != null) {
            WeaponItem newItem = new WeaponItem();
            newItem.WeaponItemData = item;
            newItem.equipItemData = item;
            newItem.itemData = item;
            currentItem = newItem;
            itemIcon.sprite = currentItem.itemData.Icon;
            itemIcon.enabled = true;
            //내구도 슬라이더 추가
        }
        else {
            currentItem = null;
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }
    }

    public WeaponItemData returnItem() {
        if(currentItem?.itemData is WeaponItemData weapItem) {
            return weapItem;
        }
        else {
            return null;
        }
    }
}
