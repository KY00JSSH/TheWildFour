using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip_InvenWeap : TooltipInfo_Inven, IPointerEnterHandler, IPointerExitHandler {

    private WeaponSlotControll weaponSlotControll;
    private GameObject currentWeap;

    protected override void Awake() {
        base.Awake();
        weaponSlotControll = GetComponent<WeaponSlotControll>();
    }

    private void Update() {
        // 아이템이 들어왔을 경우
        if (weaponSlotControll.CurrentItem) {
            currentWeap = weaponSlotControll.CurrentItem;
            _item = weaponSlotControll.GetComponent<WeaponItem>();
        }

        if (currentWeap != null) {
            if (weaponSlotControll.CurrentItem != null) {
                weapSlider.gameObject.SetActive(true);
                InvenItemText_Weap(currentWeap.GetComponent<WeaponItem>());
            }
            else {

                // 아이템을 사용했거나 버렸을 경우
                invenBoxSlider.transform.Find("Background").GetComponent<Image>().color = Color.white;
                invenBoxSlider.value = 1;
                invenBoxSlider.gameObject.SetActive(false);
                weapSlider.gameObject.SetActive(false);
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape)) Tooltip_inven.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter == gameObject) {
            if (weaponSlotControll.CurrentItem) {
                _item = weaponSlotControll.CurrentItem.GetComponent<WeaponItem>();
                currentWeap = weaponSlotControll.CurrentItem;
                Tooltip_inven.SetActive(true);
                InvenBoxItemInfo();
            }
            else {
                Debug.Log("인벤토리가 null임");
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (Tooltip_inven.activeSelf) {
            Tooltip_inven.SetActive(false);
        }
    }
}
