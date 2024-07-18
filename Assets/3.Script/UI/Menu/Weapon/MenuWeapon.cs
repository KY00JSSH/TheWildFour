using System.Collections;
using System.Collections.Generic;
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

    public ItemData addItemBox(int index, ItemData item) {
        if (index == 1) {
            //1�� ���Կ� ������ �߰�
            ItemData reItem = getcurrentItem(index);
            firstSlot.GetComponent<WeaponSlotControll>().setWeaponSlot(item);
            return reItem != null ? reItem : null;
        }
        else {
            //2�� ���Կ� ������ �߰�
            ItemData reItem = getcurrentItem(index);
            secondSlot.GetComponent<WeaponSlotControll>().setWeaponSlot(item);
            return reItem == null ? null : reItem;
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

    public ItemData getcurrentItem(int index) {
        //������ �ʵ��� item return
        if (index == 1) {
            ItemData item = firstSlot?.GetComponent<WeaponSlotControll>().returnItem();
            Debug.Log("ITEM " + item);

            return item != null ? item : null;
        }
        else {
            return secondSlot.GetComponent<WeaponSlotControll>().returnItem();
        }
    }
}

