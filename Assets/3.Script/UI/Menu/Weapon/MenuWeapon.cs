using UnityEngine;

public class MenuWeapon : MonoBehaviour {

    [SerializeField]
    private GameObject firstSlot;
    [SerializeField]
    private GameObject secondSlot;

    private RectTransform firstBoxTransf;
    public RectTransform WeapFirstBoxPos { get { return firstBoxTransf; } }
    private RectTransform secondBoxTransf;
    public RectTransform WeapSecondBoxPos { get { return secondBoxTransf; } }

    private WeaponSlotControll firstCont;
    private WeaponSlotControll secondCont;

    private InvenController invenCont;

    private int currSelectSlot = 1; //현재 선택된 장비창 슬롯 기본값 1

    private void Start() {
        firstBoxTransf = firstSlot.GetComponent<RectTransform>();
        secondBoxTransf = secondSlot.GetComponent<RectTransform>();
        firstCont = firstSlot.GetComponent<WeaponSlotControll>();
        secondCont = secondSlot.GetComponent<WeaponSlotControll>();
        invenCont = FindObjectOfType<InvenController>();
    }

    public void setCurrSelectSlot(int slotNum) {
        currSelectSlot = slotNum;
    }

    public WeaponItemData addItemBox(int index, WeaponItemData item) {
        if (index == 1) {
            //1번 슬롯에 아이템 추가
            WeaponItemData reItem = getcurrentItem(index);
            firstCont.setWeaponSlot(item);
            return reItem;
        }
        else {
            //2번 슬롯에 아이템 추가
            WeaponItemData reItem = getcurrentItem(index);
            secondCont.setWeaponSlot(item);
            return reItem;
        }
    }

    public void removeItem(int index) {
        if (index == 1) {
            //1번 슬롯에 아이템 삭제
            firstCont.setWeaponSlot(null);
        }
        else {
            //2번 슬롯에 아이템 삭제
            secondCont.setWeaponSlot(null);
        }
    }

    public WeaponItemData getcurrentItem(int index) {
        //선택한 필드의 item return
        if (index == 1) {
            WeaponItemData item = firstCont.returnItem();
            return item;
        }
        else {
            WeaponItemData item = secondCont.returnItem();
            return item;
        }
    }

    public void switchingSlot(int index) {
        WeaponItemData firstWeapon = firstCont?.CurrentItem?.itemData as WeaponItemData;
        WeaponItemData secondWeapon = secondCont?.CurrentItem?.itemData as WeaponItemData;

        if (firstWeapon && secondWeapon) {
            //둘다 아이템 있으면 서로 스위칭
            firstCont.setWeaponSlot(secondWeapon);
            secondCont.setWeaponSlot(firstWeapon);
        }
        else {
            if (index == 1) {
                //선택한 아이템이 1번 슬롯 -> 2번 슬롯으로 드래그
                firstCont.setWeaponSlot(null);
                secondCont.setWeaponSlot(firstWeapon);
            }
            else {
                //선택한 아이템이 2번 슬롯 -> 1번 슬롯으로 드래그
                firstCont.setWeaponSlot(secondWeapon);
                secondCont.setWeaponSlot(null);
            }
        }
    }

    public void addToInventory(int index, int target) {
        var weaponItem = invenCont.Inventory[target]?.itemData as WeaponItemData;
        var countableItem = invenCont.Inventory[target]?.itemData as CountableItemData;
        var foodItem = invenCont.Inventory[target]?.itemData as FoodItemData;
        var equipItem = invenCont.Inventory[target]?.itemData as EquipItemData;
        var medicItem = invenCont.Inventory[target]?.itemData as MedicItemData;

        if (weaponItem) {
            //해당칸에 무기 아이템 있으면 스위칭
            if (index == 1) {
                invenCont.addWeaponItem(firstCont.CurrentItem.itemData as WeaponItemData, target);
                firstCont.setWeaponSlot(weaponItem);
            }
            else {
                invenCont.addWeaponItem(secondCont.CurrentItem.itemData as WeaponItemData, target);
                secondCont.setWeaponSlot(weaponItem);
            }
        }
        else if (countableItem != null || foodItem != null || equipItem != null || medicItem != null || invenCont.Inventory[index] != null) {
            //해당칸에 그냥 아이템 있으면 그냥 return
            return;
        }
        else {
            //해당칸에 아이템 추가
            if (index == 1) {
                invenCont.addWeaponItem(firstCont.CurrentItem.itemData as WeaponItemData, target);
                firstCont.setWeaponSlot(null);
            }
            else {
                invenCont.addWeaponItem(secondCont.CurrentItem.itemData as WeaponItemData, target);
                secondCont.setWeaponSlot(null);
            }
        }
    }

    public void addSlotFromInvenWeapon(WeaponItemData weapItem, int target) {
        if (currSelectSlot == 1) {
            WeaponItemData firstWeapon = firstCont?.CurrentItem?.itemData as WeaponItemData;
            if (firstWeapon) {
                firstCont.setWeaponSlot(weapItem);
                invenCont.addWeaponItem(firstWeapon, target);
            }
            else {
                firstCont.setWeaponSlot(weapItem);
                invenCont.removeItem(target);
            }
        }
        else {
            WeaponItemData secondWeapon = secondCont?.CurrentItem?.itemData as WeaponItemData;
            if (secondWeapon) {
                secondCont.setWeaponSlot(weapItem);
                invenCont.addWeaponItem(secondWeapon, target);
            }
            else {
                secondCont.setWeaponSlot(weapItem);
                invenCont.removeItem(target);
            }
        }
    }
}

