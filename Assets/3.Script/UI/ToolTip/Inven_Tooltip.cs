using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inven_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    /*
     * 인벤토리 아이템 Tooltip 
     * inventory box button의 component -> itemdata가 있으면 Tooltip 표시 없으면 리턴
     //TODO: 아이템 신선함 표시 slide
     */

    [SerializeField] private Item item;

    public GameObject tooltipbox;
    [SerializeField] private Text _titleText;   // 아이템 이름 텍스트
    [SerializeField] private Text _contentText; // 아이템 설명 텍스트


    private void InvenItemTooltip(Button btn) {
        item = btn.GetComponent<InventoryBox>().CurrentItem;
        if (item != null) {
            Debug.Log("Item_Input 아이템이 있음" + item.name);
            tooltipbox.gameObject.SetActive(true);
            _titleText.text = item.name;
            _contentText.text = item.itemData.Description;
        }
        else {
            Debug.Log("Item_Input 아이템이 없음");
            return;
        }
    }


    // inventory box button의 component -> itemdata가 있으면 Tooltip 표시 없으면 리턴
    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerEnter != null && Input.mousePosition.y <= 100) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (eventData.position.y <= 520 && eventData.position.x >= 210) {
                if (btn != null) {
                    Debug.Log(btn.gameObject.name + " - Mouse enter");
                    InvenItemTooltip(btn);
                }
            }

        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerEnter != null || Input.mousePosition.y > 100) {
            Button btn = eventData.pointerEnter.GetComponent<Button>();
            if (btn != null) {
                tooltipbox.gameObject.SetActive(false);
                _titleText.text = "";
                _contentText.text = "";
            }
        }
    }
}
