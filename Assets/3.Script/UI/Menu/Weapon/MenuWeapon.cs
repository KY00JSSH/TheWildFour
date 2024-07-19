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
            //1�� ���Կ� ������ �߰�
            WeaponItemData reItem = getcurrentItem(index);
            firstSlot.GetComponent<WeaponSlotControll>().setWeaponSlot(item);
            return reItem;
        }
        else {
            //2�� ���Կ� ������ �߰�
            WeaponItemData reItem = getcurrentItem(index);
            secondSlot.GetComponent<WeaponSlotControll>().setWeaponSlot(item);
            return reItem;
        }
    }

    public void removeItem(int index) {
        if (index == 1) {
            //1�� ���Կ� ������ ����
            firstSlot.GetComponent<WeaponSlotControll>().setWeaponSlot(null);
        }
        else {
            //2�� ���Կ� ������ ����
            secondSlot.GetComponent<WeaponSlotControll>().setWeaponSlot(null);
        }
    }

    public WeaponItemData getcurrentItem(int index) {
        //������ �ʵ��� item return
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

