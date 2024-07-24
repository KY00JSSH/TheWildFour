using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Tooltip_Inven : TooltipInfo_Inven, IPointerEnterHandler, IPointerExitHandler {
    /*
     inven box�� ����
    // weap �� ��� tooltip�� ������ + �����̴� �ؾ��� + ���ݷµ� ����
     */
    private InventoryBox inventoryBox;

    private WeaponItem currentWeap;
    private FoodItem currentFood;

    protected override void Awake() {
        base.Awake();
        inventoryBox = GetComponent<InventoryBox>();
    }

    private void Update() {
        if (inventoryBox.isItemIn) {

            // �������� ������ ���
            //TODO: Ȯ���ʿ� (0724)
            if (inventoryBox.CurrentItem.GetComponent<FoodItem>() != null) {
                _item = inventoryBox.CurrentItem.GetComponent<FoodItem>();
                if (durability_Weap.gameObject.activeSelf) {
                    durability_Weap.gameObject.SetActive(false);
                    invenBoxSlider.gameObject.SetActive(false);
                }
                InvenItemText_Food(inventoryBox.CurrentItem.GetComponent<FoodItem>());
            }
            else if (inventoryBox.CurrentItem.GetComponent<WeaponItem>()) {
                if (invenBoxSlider.gameObject.activeSelf) {
                    invenBoxSlider.gameObject.SetActive(false);
                    durability_Food.gameObject.SetActive(false);
                }
                _item = inventoryBox.CurrentItem.GetComponent<WeaponItem>();
                weapSlider.gameObject.SetActive(true);
                InvenItemText_Weap(inventoryBox.CurrentItem.GetComponent<WeaponItem>());
            }
        }
        else {
            // �������� ����߰ų� ������ ���
            invenBoxSlider.transform.Find("Background").GetComponent<Image>().color = Color.white;
            WeapItemOff();
            FoodItemOff();
            _item = null;
        }
    }


    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter == gameObject) {
            if (inventoryBox.CurrentItem != null) {
                Tooltip_inven.SetActive(true);
                //TODO: Ȯ���ʿ� (0724)
                _item = inventoryBox.CurrentItem.GetComponent<Item>();
                InvenBoxItemInfo();
            }
            else {
                Debug.Log("�κ��丮�� null��");
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (Tooltip_inven.activeSelf) {
            Tooltip_inven.SetActive(false);
        }
    }
}
