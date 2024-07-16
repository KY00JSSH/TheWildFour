using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUseControll : MonoBehaviour {
    [SerializeField]
    private float holdTime = 2f;
    private float shortClickTime = 0.1f;
    private float timer = 0f;
    private bool isHolding = false;

    private InvenController invenController;
    private InventoryBox invenBox;

    int selectBoxKey;

    private void Start() {
        invenController = FindObjectOfType<InvenController>();
        invenBox = FindObjectOfType<InventoryBox>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            //아이템 사용
        }

        if (Input.GetKey(KeyCode.T)) {
            List<Item> inven = invenController.Inventory;

            var weaponItem = inven[selectBoxKey]?.itemData as WeaponItemData;
            var countableItem = inven[selectBoxKey]?.itemData as CountableItemData;
            var foodItem = inven[selectBoxKey]?.itemData as FoodItemData;
            var equipItem = inven[selectBoxKey]?.itemData as EquipItemData;
            var medicItem = inven[selectBoxKey]?.itemData as MedicItemData;

            if (weaponItem != null || countableItem != null || foodItem != null ||
                    equipItem != null || medicItem != null || inven[selectBoxKey] != null) {
                if (!isHolding) {
                    isHolding = true;
                    timer = 0f;
                }
                else {
                    timer += Time.deltaTime;

                    if (timer >= holdTime) {
                        // 아이템 한뭉텅이 떨구기
                        Vector3 itemDropPosition = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z - 0.1f);
                        Item itemComponent = inven[selectBoxKey];
                        Instantiate(itemComponent.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);
                        invenController.removeItem(selectBoxKey);
                        invenController.invenFullFlagReset();
                        isHolding = false;
                        timer = 0f;
                    }

                    if (timer < shortClickTime) {
                        // 짧은 클릭으로 간주하여 동작 수행
                        Debug.Log("클릭 챱");
                        Vector3 itemDropPosition = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z - 0.1f);
                        Item itemComponent = inven[selectBoxKey];
                        Instantiate(itemComponent.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);
                        invenController.dropItem(selectBoxKey);
                        invenController.invenFullFlagReset();
                        isHolding = false;
                        timer = 0f;
                    }
                }
            }
            else {
                Debug.Log("333333333");
            }
        }
    }


    public void SetSelectedBoxKey(int key) {
        selectBoxKey = key;
    }
}
