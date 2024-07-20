using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenController : MonoBehaviour {
    private bool isInvenFull = false;               //인벤토리 전체 차있고 기존에도 추가 못함 여부
    public bool IsInvenFull { get { return isInvenFull; } }
    private List<Item> inventory;
    public List<Item> Inventory { get { return inventory; } }

    private InvenUIController invenUi;
    private MenuWeapon menuWeapon;

    public GameObject itemObject;

    public bool itemTest = false;
    public ItemData testItem;

    public delegate void OnInvenChanged(List<Item> inventory);
    public event OnInvenChanged InvenChanged;

    private void Start() {
        inventory = new List<Item>();
        invenUi = FindObjectOfType<InvenUIController>();
        menuWeapon = FindObjectOfType<MenuWeapon>();
        initInven();
    }

    private void initInven() {
        for (int i = 0; i < invenUi.CurrInvenCount; i++) {
            inventory.Add(null);
        }
    }

    public void invenFullReset() {
        isInvenFull = false;
    }

    public void addInvenBox() {
        inventory.Add(null);
    }

    //if 해당 아이템이 inven에 있고,(해당 box item count < itemMaxStackCount)
    // 해당 칸에 아이템 추가
    //  else if(!full)  새박스에 아이템 add
    //else isInvenFull = true
    public void ItemAdd() {
        Item item = itemObject.GetComponent<Item>();

        int checkNum = canAddThisBox(item.Key);

        if (checkNum != 99) {
            //해당칸에 아이템 추가
            if (item is CountableItem countItem &&
                inventory[checkNum] is CountableItem newCountItem) {
                newCountItem.addCurrStack(countItem.CurrStackCount);
            }
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox != 99) {
                //null로 비워둔 inventory에 추가
                inventory[existBox] = item;
            }
            else {
                isInvenFull = true;
            }
        }
        InvenChanged?.Invoke(inventory);
    }

    //아이템 추가 가능여부
    public bool canItemAdd() {
        Item item = itemObject.GetComponent<Item>();
        //아이템을 겹쳐서 넣을 수 있는지 확인
        int checkNum = canAddThisBox(item.Key);
        if (checkNum == 16 || checkNum != 99) {
            return true;
        }
        else {
            int existBox = isExistEmptyBox();
            if (existBox == 17 || existBox != 99) {
                return true;
            }
            else {
                Debug.Log("기존 box에 추가 못함");
                return false;
            }
        }
    }

    //현재 박스에 해당 item이 있고, 해당 칸에 추가 가능할 때 해당 칸 num을 return, 없을때 99 return
    private int canAddThisBox(int itemKey) {
        for (int i = 0; i < inventory.Count; i++) {
            var weaponItem = inventory[i]?.itemData as WeaponItemData;
            var countableItem = inventory[i]?.itemData as CountableItemData;
            var foodItem = inventory[i]?.itemData as FoodItemData;
            var equipItem = inventory[i]?.itemData as EquipItemData;
            var medicItem = inventory[i]?.itemData as MedicItemData;

            if (weaponItem != null || countableItem != null || foodItem != null || equipItem != null || medicItem != null) {
                if (inventory[i]?.Key != null && inventory[i]?.Key == itemKey) {
                    if (inventory[i] is CountableItem countItem) {
                        if (countItem?.CurrStackCount < countItem?.MaxStackCount) {
                            return i;
                        }
                    }
                }
            }
        }
        return 99;
    }

    //빈 inven box가 있는지 여부
    public int isExistEmptyBox() {
        for (int i = 0; i < inventory.Count; i++) {
            var weaponItem = inventory[i]?.itemData as WeaponItemData;
            var countableItem = inventory[i]?.itemData as CountableItemData;
            var foodItem = inventory[i]?.itemData as FoodItemData;
            var equipItem = inventory[i]?.itemData as EquipItemData;
            var medicItem = inventory[i]?.itemData as MedicItemData;

            if (weaponItem == null && countableItem == null && foodItem == null && equipItem == null && medicItem == null) {
                if (inventory[i] == null) {
                    //기존에 생성했지만 null로 초기화 한 inventory일때는 해당 index return
                    return i;
                }
            }
        }
        return 99;  //아예 빈 박스를 사용못할때
    }

    //인벤에 절대 추가 못함 flag reset
    public void invenFullFlagReset() {
        isInvenFull = false;
    }
    //아이템 뭉텅이 인벤에서 삭제
    public void removeItem(int index) {
        inventory[index] = null;
        invenFullFlagReset();
        InvenChanged?.Invoke(inventory);
    }

    //아이템 1개 사용
    public void useItem(int index) {
        if (index >= 0 && index < inventory.Count && inventory[index] != null) {
            if (inventory[index] is CountableItem countItem) {
                if (countItem.CurrStackCount == 1) {
                    removeItem(index);
                }
                else {
                    countItem.useCurrStack(1);
                }
            }
            else {
                removeItem(index);
            }
        }
        InvenChanged?.Invoke(inventory);
    }

    //아이템 타입 체크
    //type 1: 돌,나무 , 2: 카운트 되는 item, 3: 카운트 없는 아이템
    public int checkItemType(int index) {
        if (index >= 0 && index < inventory.Count) {
            if (inventory[index].itemData.Key != 1 && inventory[index].itemData.Key != 2) {
                //돌, 나무 아닐때
                if (inventory[index] is CountableItem countItem) {
                    return 2;
                }
                else {
                    return 3;
                }
            }
            else {
                return 1;   //돌, 나무일때
            }
        }
        else {
            return 0;   //아이템 없을때
        }
    }

    //아이템 드랍시 아이템 삭제
    public void dropItem(int index) {
        int itemType = checkItemType(index);
        if (itemType > 0) {
            if (itemType == 1) {
                if (inventory[index] is CountableItem countItem) {
                    if (countItem.CurrStackCount > 8) {
                        countItem.useCurrStack(8);
                    }
                    else if (countItem.CurrStackCount == 8) {
                        countItem.useCurrStack(8);
                        removeItem(index);
                    }
                    else {
                        countItem.useCurrStack(countItem.CurrStackCount);
                        removeItem(index);
                    }
                    InvenChanged?.Invoke(inventory);
                }
            }
            else if (itemType == 2) {
                if (inventory[index] is CountableItem countItems) {
                    countItems.useCurrStack(1);
                    if (countItems.CurrStackCount == 0) {
                        removeItem(index);
                    }
                }
                InvenChanged?.Invoke(inventory);
            }
            else {
                removeItem(index);
                InvenChanged?.Invoke(inventory);
            }
        }
        else {
            Debug.Log("no item");
        }
    }

    //인벤 아이템끼리 스위칭
    public void changeInvenIndex(int currentIndex, int changeIndex) {
        if (currentIndex != changeIndex && changeIndex != 99) {
            var weaponItem = inventory[changeIndex]?.itemData as WeaponItemData;
            var countableItem = inventory[changeIndex]?.itemData as CountableItemData;
            var foodItem = inventory[changeIndex]?.itemData as FoodItemData;
            var equipItem = inventory[changeIndex]?.itemData as EquipItemData;
            var medicItem = inventory[changeIndex]?.itemData as MedicItemData;

            if (weaponItem != null || countableItem != null || foodItem != null ||
                equipItem != null || medicItem != null || inventory[changeIndex] != null) {
                Item changeItem = inventory[changeIndex];
                inventory[changeIndex] = inventory[currentIndex];
                inventory[currentIndex] = changeItem;
                InvenChanged?.Invoke(inventory);
            }
            else {
                inventory[changeIndex] = inventory[currentIndex];
                inventory[currentIndex] = null;
                invenFullFlagReset();
                InvenChanged?.Invoke(inventory);
            }
        }
    }

    //장비창과 인벤창 아이템 스위칭
    public void changeItemIntoWeapSlot(WeaponItemData item, int index) {
        //무기가 이미 있을때 인벤창이랑 스위칭 아니면 인벤에 있는 아이템 지움
        if (item != null) {
            WeaponItem newItem = new WeaponItem();
            newItem.WeaponItemData = item;
            newItem.equipItemData = item;
            newItem.itemData = item;
            inventory[index] = newItem;
            InvenChanged?.Invoke(inventory);
        }
        else {
            inventory[index] = null;
            invenFullFlagReset();
            InvenChanged?.Invoke(inventory);
        }
    }

    //특정 인덱스에 무기 아이템 있는지 확인
    public WeaponItemData getIndexItem(int index) {
        if (inventory[index]?.itemData is WeaponItemData weapItem) {
            return weapItem;
        }
        else {
            return null;
        }
    }

    //특정 인덱스에 무기 아이템 추가
    public void addWeaponItem(WeaponItemData weapItem, int index) {
        WeaponItem newItem = new WeaponItem();
        newItem.WeaponItemData = weapItem;
        newItem.equipItemData = weapItem;
        newItem.itemData = weapItem;
        inventory[index] = newItem;
        InvenChanged?.Invoke(inventory);
    }

    //아이템 F로 사용
    public void useInvenItem(int index) {
        if (inventory[index]?.itemData is MedicItemData medicItem) {
            //선택한 아이템이 약품이면 1개 사용
            var countItem = inventory[index] as CountableItem;
            countItem.useCurrStack(1);
            if (countItem.CurrStackCount == 0) {
                inventory[index] = null;
            }
        }
        else if (inventory[index]?.itemData is FoodItemData foodItem) {
            //선택한 아이템이 음식이면 1개 사용
            var countItem = inventory[index] as CountableItem;
            countItem.useCurrStack(1);
            if (countItem.CurrStackCount == 0) {
                inventory[index] = null;
            }
        }
        else if (inventory[index]?.itemData is WeaponItemData weapItem) {
            //도구면 장착 - 이미 슬롯 장착 되어 있으면 스위칭
            menuWeapon.addSlotFromInvenWeapon(weapItem, index);
        }
        else {
            return;
        }
    }

    //TODO: 제작시 사용하는 필요 아이템 있으면 사용
    //TODO: 제작 후 인벤 차있으면 드랍
}