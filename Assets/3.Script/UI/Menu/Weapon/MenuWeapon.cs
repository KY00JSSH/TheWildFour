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

    private void Start() {
        firstBoxTransf = firstSlot.GetComponent<RectTransform>();
        secondBoxTransf = secondSlot.GetComponent<RectTransform>();
    }

    public WeaponItemData addItemBox(int index, WeaponItemData item) {
        if (index == 1) {
            //1번 슬롯에 아이템 추가
            WeaponItemData reItem = getcurrentItem(index);
            firstSlot.GetComponent<WeaponSlotControll>().setWeaponSlot(item);
            return reItem;
        }
        else {
            //2번 슬롯에 아이템 추가
            WeaponItemData reItem = getcurrentItem(index);
            secondSlot.GetComponent<WeaponSlotControll>().setWeaponSlot(item);
            return reItem;
        }
    }

    public void removeItem(int index) {
        if (index == 1) {
            //1번 슬롯에 아이템 삭제
            firstSlot.GetComponent<WeaponSlotControll>().setWeaponSlot(null);
        }
        else {
            //2번 슬롯에 아이템 삭제
            secondSlot.GetComponent<WeaponSlotControll>().setWeaponSlot(null);
        }
    }

    public WeaponItemData getcurrentItem(int index) {
        //선택한 필드의 item return
        if (index == 1) {
            WeaponItemData item = firstSlot?.GetComponent<WeaponSlotControll>().returnItem();
            return item;
        }
        else {
            WeaponItemData item = secondSlot.GetComponent<WeaponSlotControll>().returnItem();
            return item;
        }
    }
}

