using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenDrop : MonoBehaviour {
    private InvenController invenController;
    [SerializeField]
    private GameObject player;

    private void Awake() {
        invenController = FindObjectOfType<InvenController>();
    }

    public void dropItem(int selectBoxKey) {

        List<Item> inven = invenController.Inventory;

        var weaponItem = inven[selectBoxKey]?.itemData as WeaponItemData;
        var countableItem = inven[selectBoxKey]?.itemData as CountableItemData;
        var foodItem = inven[selectBoxKey]?.itemData as FoodItemData;
        var equipItem = inven[selectBoxKey]?.itemData as EquipItemData;
        var medicItem = inven[selectBoxKey]?.itemData as MedicItemData;

        if (weaponItem != null || countableItem != null || foodItem != null ||
            equipItem != null || medicItem != null || inven[selectBoxKey] != null) {

            if (selectBoxKey >= 0 && selectBoxKey < inven.Count) {
                Item itemComponent = inven[selectBoxKey];
                Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);
                GameObject dropItem = Instantiate(itemComponent.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);
                if (invenController.checkItemType(selectBoxKey) == 1) {
                    //µ¹, ³ª¹«ÀÌ¸é 8°³ ¾¿ ¶³±À, 8°³º¸´Ù ÀûÀ¸¸é ÀüÃ¼ ¶³±À
                    if (itemComponent is CountableItem countItem) {
                        CountableItem dropCountItem = dropItem.GetComponent<CountableItem>();
                        if (dropCountItem != null) {
                            if (countItem.CurrStackCount >= 8) {
                                dropCountItem.addCurrStack(7);
                            }
                            else {
                                dropCountItem.addCurrStack(countItem.CurrStackCount - 1);
                            }
                        }
                    }
                }
                invenController.dropItem(selectBoxKey);
                invenController.invenFullFlagReset();
            }
            else {
                Debug.Log("Invalid selectBoxKey");
            }
        }
    }

    public void dropItemAll(int selectBoxKey) {

        List<Item> inven = invenController.Inventory;

        var weaponItem = inven[selectBoxKey]?.itemData as WeaponItemData;
        var countableItem = inven[selectBoxKey]?.itemData as CountableItemData;
        var foodItem = inven[selectBoxKey]?.itemData as FoodItemData;
        var equipItem = inven[selectBoxKey]?.itemData as EquipItemData;
        var medicItem = inven[selectBoxKey]?.itemData as MedicItemData;

        if (weaponItem != null || countableItem != null || foodItem != null || equipItem != null || medicItem != null || inven[selectBoxKey] != null) {

            if (selectBoxKey >= 0 && selectBoxKey < inven.Count) {
                Item itemComponent = inven[selectBoxKey];
                Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);
                GameObject dropItem = Instantiate(itemComponent.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);

                if (itemComponent is CountableItem countItem) {
                    CountableItem dropCountItem = dropItem.GetComponent<CountableItem>();
                    if (dropCountItem != null) {
                        dropCountItem.addCurrStack(countItem.CurrStackCount - 1);
                    }
                }

                invenController.removeItem(selectBoxKey);
                invenController.invenFullFlagReset();
            }
            else {
                Debug.Log("Invalid selectBoxKey");
            }
        }
    }

    public void dropAllSlotItems() {
        List<Item> inven = invenController.Inventory;

        for (int i = 0; i < inven.Count; i++) {
            var weaponItem = inven[i]?.itemData as WeaponItemData;
            var countableItem = inven[i]?.itemData as CountableItemData;
            var foodItem = inven[i]?.itemData as FoodItemData;
            var equipItem = inven[i]?.itemData as EquipItemData;
            var medicItem = inven[i]?.itemData as MedicItemData;

            if (weaponItem != null || countableItem != null || foodItem != null || equipItem != null || medicItem != null || inven[i] != null) {
                Item itemComponent = inven[i];
                Vector3 itemDropPosition = new Vector3(player.transform.position.x - 0.1f, player.transform.position.y + 1.5f, player.transform.position.z - 0.1f);
                GameObject dropItem = Instantiate(itemComponent.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);

                if (itemComponent is CountableItem countItem) {
                    CountableItem dropCountItem = dropItem.GetComponent<CountableItem>();
                    if (dropCountItem != null) {
                        dropCountItem.addCurrStack(countItem.CurrStackCount - 1);
                    }
                }
                invenController.removeItem(i);
            }
        }
        invenController.invenFullFlagReset();
    }
}
