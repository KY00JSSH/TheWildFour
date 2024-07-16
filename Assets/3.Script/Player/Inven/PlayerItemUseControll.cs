using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUseControll : MonoBehaviour {
    [SerializeField]
    private float holdTime = 2f;
    private float shortClickTime = 0.1f;
    private float timer = 0f;
    private bool isKeyPressed = false;

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

        if (Input.GetKeyDown(KeyCode.T)) {
            isKeyPressed = true;
            timer = 0f; // 타이머 초기화
        }

        if (isKeyPressed) {
            timer += Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.T)) {
                isKeyPressed = false;
                List<Item> inven = invenController.Inventory;

                var weaponItem = inven[selectBoxKey]?.itemData as WeaponItemData;
                var countableItem = inven[selectBoxKey]?.itemData as CountableItemData;
                var foodItem = inven[selectBoxKey]?.itemData as FoodItemData;
                var equipItem = inven[selectBoxKey]?.itemData as EquipItemData;
                var medicItem = inven[selectBoxKey]?.itemData as MedicItemData;

                if (weaponItem != null || countableItem != null || foodItem != null ||
                    equipItem != null || medicItem != null || inven[selectBoxKey] != null) {

                    if (timer < shortClickTime) {
                        // 짧은 클릭으로 간주하여 동작 수행
                        DropItem(inven);
                    }
                    else if (timer >= holdTime) {
                        // 길게 누르기로 간주하여 동작 수행
                        DropItemAll(inven);
                    }
                    timer = 0f;
                }
                else {
                    Debug.Log("item null");
                }
            }
        }
    }

    private void DropItem(List<Item> inven) {
        if (selectBoxKey >= 0 && selectBoxKey < inven.Count) {
            Item itemComponent = inven[selectBoxKey];
            Vector3 itemDropPosition = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z - 0.1f);
            Instantiate(itemComponent.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);
            invenController.dropItem(selectBoxKey);
            invenController.invenFullFlagReset();
        }
        else {
            Debug.Log("Invalid selectBoxKey");
        }
    }

    private void DropItemAll(List<Item> inven) {
        if (selectBoxKey >= 0 && selectBoxKey < inven.Count) {
            Item itemComponent = inven[selectBoxKey];
            Vector3 itemDropPosition = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z - 0.1f);
            Instantiate(itemComponent.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);
            invenController.removeItem(selectBoxKey);
            invenController.invenFullFlagReset();
        }
        else {
            Debug.Log("Invalid selectBoxKey");
        }
    }

    public void SetSelectedBoxKey(int key) {
        selectBoxKey = key;
    }
}
