using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip_InvenWeap : TooltipInfo_Inven, IPointerEnterHandler, IPointerExitHandler {

    private WeaponSlotControll weaponSlotControll;
   private EquipItem currentEquip;
   private WeaponItem currentWeap;

    protected override void Awake() {
        base.Awake();
        weaponSlotControll = GetComponent<WeaponSlotControll>();
    }

    private void Update() {
        // 아이템이 들어왔을 경우

        if (weaponSlotControll.CurrentItem) {
            Debug.Log("?????????");
            currentWeap = weaponSlotControll.CurrentItem;

            _item = weaponSlotControll.CurrentItem;
            Debug.Log(weaponSlotControll.CurrentItem?.itemData);
            Debug.Log(weaponSlotControll.CurrentItem);
            Debug.Log(_item);
        }

        if (currentWeap != null) {
            Debug.Log("!currentWeap!!!!!!!!!!!"+ currentWeap.name);
            if (weaponSlotControll.CurrentItem?.itemData) {
                weapSlider.gameObject.SetActive(true);
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                InvenItemText_Weap(currentWeap);
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
                _item = weaponSlotControll.CurrentItem;
                currentWeap = weaponSlotControll.CurrentItem;
                Tooltip_inven.SetActive(true);
                InvenBoxItemInfo();
            }
            else {
                Debug.Log("!!!!!!!!!!!!" + weaponSlotControll.CurrentItem.itemData);
                Debug.Log("!!!!!!!!!!!!???????????" + weaponSlotControll.CurrentItem);
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
