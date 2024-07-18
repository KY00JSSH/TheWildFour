using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotControll : MonoBehaviour {

    [SerializeField]
    private Image itemIcon;

    private Item currentItem;
    public Item CurrentItem { get { return currentItem; } }

    public void setWeaponSlot(ItemData item = null) {
        if (item != null) {
            Item newItem = new Item();
            newItem.itemData = item;
            currentItem = newItem;
            itemIcon.sprite = currentItem.itemData.Icon;
            itemIcon.gameObject.SetActive(true);
            //������ �����̴� �߰�
        }
        else {
            currentItem = null;
            itemIcon.sprite = null;
        }
    }

    public ItemData returnItem() {
        if(currentItem?.itemData == null) {
            return null;
        }
        else {
            return currentItem.itemData;
        }
    }
}
