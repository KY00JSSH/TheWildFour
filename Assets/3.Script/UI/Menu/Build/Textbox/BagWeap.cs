using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagWeap : MonoBehaviour
{
    [SerializeField] private GameObject weaponslot;
    private WeaponSlotControll weaponSlotCtrl;
    private Image slotImg;
    private Sprite defaultSlot;
    private Vector2 defaultSlotSize;

    private void Awake() {
        weaponSlotCtrl = weaponslot.GetComponent<WeaponSlotControll>();
        slotImg = transform.GetChild(0).GetComponent<Image>();
        defaultSlot = slotImg.sprite;
        defaultSlotSize = new Vector2(slotImg.sprite.rect.width, slotImg.sprite.rect.height);
    }

    private void OnEnable() {
        CheckWeapInvenBox();
    }
    private void CheckWeapInvenBox() {
        if (weaponSlotCtrl.CurrentItem != null) ChangeSlotState();
        else DefaultSlotState();
    }


    private void DefaultSlotState() {
        slotImg.sprite = defaultSlot;
        slotImg.rectTransform.sizeDelta = defaultSlotSize;
    }

    private void ChangeSlotState() {
        Debug.Log(weaponSlotCtrl.CurrentItem.GetComponent<WeaponItem>().itemData.name);
        slotImg.sprite = weaponSlotCtrl.CurrentItem.GetComponent<WeaponItem>().itemData.Icon;
        slotImg.rectTransform.sizeDelta = new Vector2(65 , 65);
    }
}
