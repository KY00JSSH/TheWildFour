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

    // PlayerWeaponEquip 에서 받아가기 위한 프로퍼티입니다.
    public int CurrentSelectSlot { get { return currSelectSlot; } }

    private void Update() {
        if (PlayerStatus.isDead) return;

        if (Input.GetKeyDown(KeyCode.X)) {
            if(currSelectSlot == 1) {
                setCurrSelectSlot(2);
            }
            else {
                setCurrSelectSlot(1);
            }
        }
    }

    private void Start() {
        firstBoxTransf = firstSlot.GetComponent<RectTransform>();
        secondBoxTransf = secondSlot.GetComponent<RectTransform>();
        firstCont = firstSlot.GetComponent<WeaponSlotControll>();
        secondCont = secondSlot.GetComponent<WeaponSlotControll>();
        invenCont = FindObjectOfType<InvenController>();
    }

    public void setCurrSelectSlot(int slotNum) {
        currSelectSlot = slotNum;
        if(slotNum ==1) {
            firstCont.enableCursor();
            secondCont.disableCursor();
        }
        else {
            firstCont.disableCursor();
            secondCont.enableCursor();
        }
    }

    public GameObject addItemBox(int index, GameObject item) {
        if (index == 1) {
            //1번 슬롯에 아이템 추가
            GameObject reItem = getcurrentItem(index);
            firstCont.setWeaponSlot(item);
            return reItem;
        }
        else {
            //2번 슬롯에 아이템 추가
            GameObject reItem = getcurrentItem(index);
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

    public GameObject getcurrentItem(int index) {
        //선택한 필드의 item return
        if (index == 1) {
            GameObject item = firstCont.returnItem();
            return item;
        }
        else {
            GameObject item = secondCont.returnItem();
            return item;
        }
    }

    public void switchingSlot(int index) {
        GameObject firstWeapon = firstCont?.CurrentItem ;
        GameObject secondWeapon = secondCont?.CurrentItem ;

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
        if (invenCont?.Inventory[target] !=null) {
            //해당칸에 무기 아이템 있으면 스위칭
            if(invenCont?.Inventory[target].GetComponent<WeaponItem>() != null) {
                if (index == 1) {
                    invenCont.addIndexItem(target, firstCont.CurrentItem);
                    firstCont.setWeaponSlot(invenCont?.Inventory[target]);
                }
                else {
                    invenCont.addIndexItem(target, secondCont.CurrentItem);
                    secondCont.setWeaponSlot(invenCont?.Inventory[target]);
                }
            }
            else {
                return;
            }
        }
        else {
            //해당칸에 아이템 추가
            if (index == 1) {
                invenCont.addIndexItem(target, firstCont.CurrentItem);
                firstCont.setWeaponSlot(null);
            }
            else {
                invenCont.addIndexItem( target, secondCont.CurrentItem);
                secondCont.setWeaponSlot(null);
            }
        }
    }

    public void addSlotFromInvenWeapon(int target, GameObject item) {
        if (currSelectSlot == 1) {
            GameObject firstWeapon = firstCont?.CurrentItem;
            if (firstWeapon) {
                firstCont.setWeaponSlot(item);
                invenCont.addIndexItem( target , firstWeapon);
            }
            else {
                firstCont.setWeaponSlot(item);
                invenCont.removeItem(target);
            }
        }
        else {
            GameObject secondWeapon = secondCont?.CurrentItem;
            if (secondWeapon) {
                secondCont.setWeaponSlot(item);
                invenCont.addIndexItem(target, secondWeapon);
            }
            else {
                secondCont.setWeaponSlot(item);
                invenCont.removeItem(target);
            }
        }
    }
}

