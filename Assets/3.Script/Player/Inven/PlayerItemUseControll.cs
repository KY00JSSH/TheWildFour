using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUseControll : MonoBehaviour {
    [SerializeField]
    private float holdTime = 1.5f;
    private float shortClickTime = 0.1f;
    private float timer = 0f;
    private bool isKeyPressed = false;
    private bool isLong = false;

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
            isLong = true;
            timer = 0f; // 타이머 초기화
        }

        if (isKeyPressed) {
            timer += Time.deltaTime;

            if (timer >= holdTime && isLong) {
                // 길게 누르기 - 전체 아이템 드랍
                List<Item> inven = invenController.Inventory;

                var weaponItem = inven[selectBoxKey]?.itemData as WeaponItemData;
                var countableItem = inven[selectBoxKey]?.itemData as CountableItemData;
                var foodItem = inven[selectBoxKey]?.itemData as FoodItemData;
                var equipItem = inven[selectBoxKey]?.itemData as EquipItemData;
                var medicItem = inven[selectBoxKey]?.itemData as MedicItemData;

                if (weaponItem != null || countableItem != null || foodItem != null ||
                    equipItem != null || medicItem != null || inven[selectBoxKey] != null) {
                    //아이템이 존재하는지 체크 후 Drop
                    DropItemAll(inven);
                }
                else {
                    Debug.Log("item null");
                }
                isKeyPressed = false;
                isLong = false;
                timer = 0f;
            }

            if (Input.GetKeyUp(KeyCode.T)) {
                if (timer < shortClickTime) {
                    List<Item> inven = invenController.Inventory;

                    var weaponItem = inven[selectBoxKey]?.itemData as WeaponItemData;
                    var countableItem = inven[selectBoxKey]?.itemData as CountableItemData;
                    var foodItem = inven[selectBoxKey]?.itemData as FoodItemData;
                    var equipItem = inven[selectBoxKey]?.itemData as EquipItemData;
                    var medicItem = inven[selectBoxKey]?.itemData as MedicItemData;

                    if (weaponItem != null || countableItem != null || foodItem != null ||
                        equipItem != null || medicItem != null || inven[selectBoxKey] != null) {
                        DropItem(invenController.Inventory); // 짧은 클릭으로 아이템 한 개 떨어뜨리기
                    }
                    else {
                        Debug.Log("item null");
                    }
                }
                isKeyPressed = false;
                timer = 0f;
            }
        }
    }

    private void DropItem(List<Item> inven) {
        if (selectBoxKey >= 0 && selectBoxKey < inven.Count) {
            Item itemComponent = inven[selectBoxKey];
            Vector3 itemDropPosition = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z - 0.1f);
            GameObject dropItem = Instantiate(itemComponent.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);
            if(invenController.checkItemType(selectBoxKey)==1) {
                //돌, 나무이면 8개 씩 떨굼, 8개보다 적으면 전체 떨굼
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

    private void DropItemAll(List<Item> inven) {
        if (selectBoxKey >= 0 && selectBoxKey < inven.Count) {
            Item itemComponent = inven[selectBoxKey];
            Vector3 itemDropPosition = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z - 0.1f);
            GameObject dropItem  =  Instantiate(itemComponent.itemData.DropItemPrefab, itemDropPosition, Quaternion.identity);

            if (itemComponent is CountableItem countItem) {
                CountableItem dropCountItem = dropItem.GetComponent<CountableItem>();
                if (dropCountItem != null) {
                    dropCountItem.addCurrStack(countItem.CurrStackCount -1); 
                }
            }

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
